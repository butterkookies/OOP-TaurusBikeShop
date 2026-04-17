// WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Polls active deliveries every 5 minutes to pick up courier-reported status
/// changes and propagate them to the Delivery table.
/// Flowchart: Part 8 — "Monitor Delivery Progress" (O11).
/// <para>
/// <b>Scope of responsibility — AdminSystem books, this job polls:</b>
/// The actual courier booking (selecting Lalamove or LBC, calling their APIs,
/// and receiving a booking reference) is an AdminSystem action — flowchart Part 8
/// nodes O6A through O9. This job is invoked only AFTER those booking references
/// are written to <c>LalamoveDelivery.BookingRef</c> or
/// <c>LBCDelivery.TrackingNumber</c>. Deliveries with a null booking reference
/// are skipped each cycle with a debug-level log.
/// </para>
/// <para>
/// <b>Current integration state:</b>
/// Lalamove and LBC API clients are not yet connected. Both
/// <c>PollLalamoveAsync</c> and <c>PollLBCAsync</c> are stubs that return
/// <c>null</c> (no status change). See their XML docs for integration steps.
/// </para>
/// <para>
/// <b>Notification pattern:</b>
/// Status changes are persisted via <c>SaveChangesAsync</c> before notifications
/// are queued. A notification failure therefore never rolls back a status update.
/// Only terminal or customer-visible transitions queue notifications:
/// Delivered → <c>DeliveryConfirmation</c> (flowchart O12),
/// Failed → <c>TrackingUpdate</c> alert (flowchart O11C).
/// </para>
/// </summary>
public sealed class DeliveryStatusPollJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory          _scopeFactory;
    private readonly ILogger<DeliveryStatusPollJob> _logger;

    // Exponential backoff: tracks consecutive failures to avoid log flooding
    // when the database is unavailable.
    private int _consecutiveFailures;
    private static readonly TimeSpan MaxBackoff = TimeSpan.FromMinutes(2);

    public DeliveryStatusPollJob(
        IServiceScopeFactory          scopeFactory,
        ILogger<DeliveryStatusPollJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeliveryStatusPollJob started.");

        // Staggered startup: delay before first cycle to prevent all background
        // services from hitting the DB simultaneously at boot.
        await Task.Delay(TimeSpan.FromSeconds(14), stoppingToken).ConfigureAwait(false);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCycleAsync(stoppingToken);
                _consecutiveFailures = 0;
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _consecutiveFailures++;
                _logger.LogError(ex, "{Job} unhandled exception (failure #{Count}) — will retry next cycle.",
                    GetType().Name, _consecutiveFailures);
            }

            TimeSpan delay = _consecutiveFailures > 0
                ? TimeSpan.FromSeconds(Math.Min(MaxBackoff.TotalSeconds,
                    CycleInterval.TotalSeconds * Math.Pow(2, _consecutiveFailures - 1)))
                : CycleInterval;
            await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
        }
        _logger.LogInformation("DeliveryStatusPollJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext         context       = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        INotificationService notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // -- SystemLog: cycle start --
        try
        {
            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobStart,
                EventDescription = $"{nameof(DeliveryStatusPollJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(DeliveryStatusPollJob));
        }

        try
        {
            // Include courier sub-records so PollCourierStatusAsync can access
            // booking references. Include Order.User so customer notifications
            // can be addressed after status transitions are committed.
            List<Delivery> activeDeliveries = await context.Deliveries
                .AsTracking() // Entities are modified (DeliveryStatus) then saved
                .Include(d => d.LalamoveDelivery)
                .Include(d => d.LBCDelivery)
                .Include(d => d.Order)
                    .ThenInclude(o => o.User)
                .Where(d => d.DeliveryStatus == DeliveryStatuses.PickedUp
                         || d.DeliveryStatus == DeliveryStatuses.InTransit)
                .ToListAsync(cancellationToken);

            // Accumulate transitions so notifications can be sent AFTER
            // SaveChangesAsync — status committed first, then notifications.
            List<(Delivery Delivery, string NewStatus)> transitioned = [];

            int pollCount = 0;
            foreach (Delivery delivery in activeDeliveries)
            {
                pollCount++;
                string? newStatus = await PollCourierStatusAsync(delivery, cancellationToken);
                if (newStatus is null || newStatus == delivery.DeliveryStatus)
                    continue;

                _logger.LogInformation(
                    "DeliveryStatusPollJob: delivery {DeliveryId} {OldStatus} \u2192 {NewStatus}.",
                    delivery.DeliveryId, delivery.DeliveryStatus, newStatus);

                delivery.DeliveryStatus = newStatus;
                if (newStatus == DeliveryStatuses.Delivered)
                    delivery.ActualDeliveryTime = DateTime.UtcNow;

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.DeliveryStatusPoll,
                    EventDescription = $"Delivery {delivery.DeliveryId} (order {delivery.OrderId}): status updated to {newStatus}.",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);

                transitioned.Add((delivery, newStatus));
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(DeliveryStatusPollJob)} cycle completed. {pollCount} active delivery/deliveries polled.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);

            // Commit all status changes and SystemLog entries before notification work.
            await context.SaveChangesAsync(cancellationToken);

            // Queue customer notifications for terminal or customer-visible transitions.
            // Executed after SaveChangesAsync — notification failures cannot roll back
            // already-committed delivery status changes.
            foreach (var (delivery, newStatus) in transitioned)
                await QueueStatusChangeNotificationAsync(delivery, newStatus, notifications, cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "DeliveryStatusPollJob cycle failed.");

            bool isDbError = ex is Microsoft.EntityFrameworkCore.Storage.RetryLimitExceededException
                          || ex.InnerException is Microsoft.Data.SqlClient.SqlException;
            if (!isDbError)
            {
                try
                {
                    await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                    AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                    await ec.SystemLogs.AddAsync(new SystemLog
                    {
                        EventType        = SystemLogEvents.BackgroundJobError,
                        EventDescription = $"{nameof(DeliveryStatusPollJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);
                    await ec.SaveChangesAsync(cancellationToken);
                }
                catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for DeliveryStatusPollJob."); }
            }
        }
    }

    // =========================================================================
    // Courier status polling — dispatches by Courier type
    // =========================================================================

    /// <summary>
    /// Routes the status poll to the correct courier-specific method based on
    /// <see cref="Delivery.Courier"/>. Returns the new status string, or
    /// <c>null</c> if the status is unchanged or the courier API is unavailable.
    /// </summary>
    private Task<string?> PollCourierStatusAsync(
        Delivery delivery,
        CancellationToken cancellationToken)
        => delivery.Courier switch
        {
            Couriers.Lalamove => PollLalamoveAsync(delivery, cancellationToken),
            Couriers.LBC      => PollLBCAsync(delivery, cancellationToken),
            _                 => LogAndSkipUnknownCourier(delivery)
        };

    private Task<string?> LogAndSkipUnknownCourier(Delivery delivery)
    {
        _logger.LogWarning(
            "DeliveryStatusPollJob: unknown courier '{Courier}' on delivery {DeliveryId} — skipping.",
            delivery.Courier, delivery.DeliveryId);
        return Task.FromResult<string?>(null);
    }

    /// <summary>
    /// Polls the Lalamove REST API for the current delivery status.
    /// <para>
    /// <b>Not yet integrated.</b> Returns <c>null</c> until the Lalamove API
    /// client is wired in. Integration steps:
    /// <list type="number">
    ///   <item>Inject an <c>ILalamoveClient</c> or <c>IHttpClientFactory</c>
    ///     (Lalamove uses a REST API with HMAC-SHA256 authentication).</item>
    ///   <item>Call the Lalamove "Get Order" endpoint using
    ///     <c>delivery.LalamoveDelivery.BookingRef</c>.</item>
    ///   <item>Map the Lalamove status string to a <see cref="DeliveryStatuses"/>
    ///     constant (e.g. ASSIGNING → PickedUp, IN_PROGRESS → InTransit,
    ///     COMPLETED → Delivered, CANCELED/REJECTED → Failed).</item>
    ///   <item>If a driver is assigned, update
    ///     <c>delivery.LalamoveDelivery.DriverName</c> and
    ///     <c>delivery.LalamoveDelivery.DriverPhone</c>.</item>
    /// </list>
    /// </para>
    /// </summary>
    private Task<string?> PollLalamoveAsync(
        Delivery delivery,
        CancellationToken cancellationToken)
    {
        // Guard: AdminSystem writes BookingRef when the Lalamove booking succeeds.
        // Skip silently if booking has not been confirmed yet.
        if (delivery.LalamoveDelivery?.BookingRef is null)
        {
            _logger.LogDebug(
                "DeliveryStatusPollJob: delivery {DeliveryId} has no Lalamove BookingRef — " +
                "awaiting AdminSystem booking.",
                delivery.DeliveryId);
            return Task.FromResult<string?>(null);
        }

        // TODO: Call Lalamove API with delivery.LalamoveDelivery.BookingRef.
        _logger.LogDebug(
            "DeliveryStatusPollJob: Lalamove API not yet integrated — " +
            "delivery {DeliveryId} (BookingRef: {BookingRef}) skipped.",
            delivery.DeliveryId, delivery.LalamoveDelivery.BookingRef);

        return Task.FromResult<string?>(null);
    }

    /// <summary>
    /// Polls the LBC tracking service for the current delivery status.
    /// <para>
    /// <b>Not yet integrated.</b> Returns <c>null</c> until the LBC tracking
    /// client is wired in. Integration steps:
    /// <list type="number">
    ///   <item>Add an LBC tracking client (LBC does not expose an official public
    ///     REST API — integration typically uses their tracking portal or a
    ///     business API agreement).</item>
    ///   <item>Query using <c>delivery.LBCDelivery.TrackingNumber</c>
    ///     (the waybill number generated at booking).</item>
    ///   <item>Map LBC status codes to <see cref="DeliveryStatuses"/> constants.</item>
    /// </list>
    /// </para>
    /// </summary>
    private Task<string?> PollLBCAsync(
        Delivery delivery,
        CancellationToken cancellationToken)
    {
        // Guard: AdminSystem writes TrackingNumber (waybill) when LBC confirms booking.
        if (delivery.LBCDelivery?.TrackingNumber is null)
        {
            _logger.LogDebug(
                "DeliveryStatusPollJob: delivery {DeliveryId} has no LBC TrackingNumber — " +
                "awaiting AdminSystem booking.",
                delivery.DeliveryId);
            return Task.FromResult<string?>(null);
        }

        // TODO: Call LBC tracking service with delivery.LBCDelivery.TrackingNumber.
        _logger.LogDebug(
            "DeliveryStatusPollJob: LBC API not yet integrated — " +
            "delivery {DeliveryId} (TrackingNumber: {TrackingNumber}) skipped.",
            delivery.DeliveryId, delivery.LBCDelivery.TrackingNumber);

        return Task.FromResult<string?>(null);
    }

    // =========================================================================
    // Customer notifications on status transitions
    // =========================================================================

    /// <summary>
    /// Queues a customer email notification when a delivery reaches a
    /// customer-visible status (Delivered or Failed). Per-delivery try/catch
    /// ensures one bad customer address never blocks other notifications.
    /// </summary>
    private async Task QueueStatusChangeNotificationAsync(
        Delivery             delivery,
        string               newStatus,
        INotificationService notifications,
        CancellationToken    cancellationToken)
    {
        string? customerEmail = delivery.Order?.User?.Email;
        if (customerEmail is null)
        {
            _logger.LogWarning(
                "DeliveryStatusPollJob: delivery {DeliveryId} has no customer email — " +
                "skipping status notification.",
                delivery.DeliveryId);
            return;
        }

        int    userId      = delivery.Order!.UserId;
        int    orderId     = delivery.OrderId;
        string orderNumber = delivery.Order.OrderNumber;

        try
        {
            if (newStatus == DeliveryStatuses.Delivered)
            {
                // Flowchart Part 8 — O12: Queue Delivery Confirmation.
                await notifications.QueueAsync(
                    channel:           NotifChannels.Email,
                    notifType:         NotifTypes.DeliveryConfirmation,
                    recipient:         customerEmail,
                    subject:           $"Your order has been delivered \u2014 {orderNumber}",
                    body:              $"Your order {orderNumber} has been successfully delivered. " +
                                       $"Thank you for shopping with Taurus Bike Shop!",
                    userId:            userId,
                    orderId:           orderId,
                    cancellationToken: cancellationToken);
            }
            else if (newStatus == DeliveryStatuses.Failed)
            {
                // Flowchart Part 8 — O11C: Delivery Failed — Queue Alert.
                await notifications.QueueAsync(
                    channel:           NotifChannels.Email,
                    notifType:         NotifTypes.TrackingUpdate,
                    recipient:         customerEmail,
                    subject:           $"Delivery attempt failed \u2014 {orderNumber}",
                    body:              $"A delivery attempt for order {orderNumber} was unsuccessful. " +
                                       $"Please contact us to arrange a reschedule or an alternative pickup.",
                    userId:            userId,
                    orderId:           orderId,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex,
                "DeliveryStatusPollJob: failed to queue status notification for delivery {DeliveryId}.",
                delivery.DeliveryId);
        }
    }
}
