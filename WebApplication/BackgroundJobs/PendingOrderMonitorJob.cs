// WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Enforces the 24-hour Pending-order policy:
/// <list type="number">
///   <item>
///     Queues a <see cref="NotifTypes.PendingOrderReminder"/> email to the customer
///     once their order has sat in <see cref="OrderStatuses.Pending"/> past the
///     reminder threshold (4 hours before expiry). Deduplicated per order.
///   </item>
///   <item>
///     Auto-cancels any order still in Pending after 24 hours: sets
///     <see cref="OrderStatuses.Cancelled"/>, writes SystemLog entries, and
///     queues an <see cref="NotifTypes.OrderAutoCancelled"/> email.
///   </item>
/// </list>
/// Runs every 5 minutes.
/// </summary>
public sealed class PendingOrderMonitorJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval      = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan CancelAfter        = TimeSpan.FromHours(24);
    private static readonly TimeSpan ReminderLeadTime   = TimeSpan.FromHours(4);

    private readonly IServiceScopeFactory            _scopeFactory;
    private readonly ILogger<PendingOrderMonitorJob> _logger;

    // Exponential backoff: tracks consecutive failures to avoid log flooding
    // when the database is unavailable.
    private int _consecutiveFailures;
    private static readonly TimeSpan MaxBackoff = TimeSpan.FromMinutes(2);

    public PendingOrderMonitorJob(
        IServiceScopeFactory            scopeFactory,
        ILogger<PendingOrderMonitorJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PendingOrderMonitorJob started.");

        // Staggered startup: delay before first cycle to prevent all background
        // services from hitting the DB simultaneously at boot.
        await Task.Delay(TimeSpan.FromSeconds(11), stoppingToken).ConfigureAwait(false);

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
        _logger.LogInformation("PendingOrderMonitorJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext         context       = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        INotificationService notifications = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // ── Best-effort start log ──────────────────────────────────────────
        // If this fails we MUST clear the change tracker so the dirty
        // SystemLog entity (still in Added state) does not contaminate the
        // SaveChangesAsync call that commits the actual cancellations.
        try
        {
            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobStart,
                EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(PendingOrderMonitorJob));

            // ★ FIX: Clear the change tracker so the failed SystemLog entity
            // does not cause every subsequent SaveChangesAsync to also fail,
            // which was the root cause of orders never being auto-cancelled.
            context.ChangeTracker.Clear();
        }

        try
        {
            // NOTE: OrderDate is stored in LOCAL time (UTC+8) by the web app,
            // so we must use DateTime.Now (not UtcNow) to compute thresholds.
            DateTime now              = DateTime.Now;
            DateTime cancelThreshold  = now - CancelAfter;                   // older than this → cancel
            DateTime reminderThreshold = now - (CancelAfter - ReminderLeadTime); // older than this → remind

            // -- 1. Auto-cancel orders past the 24h mark --
            // ★ FIX: Include Items + Variants so we can unlock inventory.
            List<Order> expiredOrders = await context.Orders
                .AsTracking()
                .Include(o => o.User)
                .Include(o => o.Items)
                    .ThenInclude(i => i.Variant)
                .Where(o => o.OrderStatus == OrderStatuses.Pending
                         && o.OrderDate   < cancelThreshold)
                .ToListAsync(cancellationToken);

            foreach (Order order in expiredOrders)
            {
                order.OrderStatus = OrderStatuses.Cancelled;
                order.UpdatedAt   = now;

                // ★ FIX: Unlock inventory — restore StockQuantity and write
                // InventoryLog Unlock entries, matching OrderService.CancelOrderAsync.
                foreach (OrderItem item in order.Items)
                {
                    if (item.ProductVariantId is null) continue;

                    ProductVariant? variant = item.Variant
                        ?? await context.ProductVariants
                            .AsTracking()
                            .FirstOrDefaultAsync(
                                v => v.ProductVariantId == item.ProductVariantId,
                                cancellationToken);

                    if (variant != null)
                        variant.StockQuantity += item.Quantity;

                    await context.InventoryLogs.AddAsync(new InventoryLog
                    {
                        ProductId        = item.ProductId,
                        ProductVariantId = item.ProductVariantId,
                        OrderId          = order.OrderId,
                        ChangeQuantity   = item.Quantity,
                        ChangeType       = InventoryChangeTypes.Unlock,
                        Notes            = $"Stock unlocked — Order #{order.OrderNumber} auto-cancelled (24h pending timeout).",
                        CreatedAt        = now
                    }, cancellationToken);
                }

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription = $"Order {order.OrderNumber} (ID {order.OrderId}): Pending → Cancelled (24h pending timeout).",
                    CreatedAt        = now
                }, cancellationToken);
            }

            // -- 2. Find orders in the reminder window (past 20h, not yet cancelled, no reminder sent) --
            List<int> alreadyReminded = await context.Notifications
                .Where(n => n.NotifType == NotifTypes.PendingOrderReminder
                         && n.OrderId != null)
                .Select(n => n.OrderId!.Value)
                .Distinct()
                .ToListAsync(cancellationToken);

            List<Order> reminderOrders = await context.Orders
                .AsNoTracking()
                .Include(o => o.User)
                .Where(o => o.OrderStatus == OrderStatuses.Pending
                         && o.OrderDate   < reminderThreshold
                         && o.OrderDate   >= cancelThreshold
                         && !alreadyReminded.Contains(o.OrderId))
                .ToListAsync(cancellationToken);

            // Commit cancellations + inventory unlocks before firing notifications
            // so a mail failure never rolls back a status transition.
            await context.SaveChangesAsync(cancellationToken);

            // -- 3. Notify customers of cancellations --
            foreach (Order order in expiredOrders)
            {
                if (order.User?.Email is not string customerEmail)
                {
                    _logger.LogWarning(
                        "PendingOrderMonitorJob: cancelled order {OrderId} has no customer email — skipping notification.",
                        order.OrderId);
                    continue;
                }

                try
                {
                    await notifications.QueueAsync(
                        channel:           NotifChannels.Email,
                        notifType:         NotifTypes.OrderAutoCancelled,
                        recipient:         customerEmail,
                        subject:           $"Order {order.OrderNumber} cancelled — payment not received",
                        body:              $"Your order {order.OrderNumber} has been cancelled because payment was not " +
                                           $"submitted within 24 hours of placing it. If you still want the items, " +
                                           $"please place a new order.",
                        userId:            order.UserId,
                        orderId:           order.OrderId,
                        cancellationToken: cancellationToken);

                    await notifications.QueueAsync(
                        channel:           NotifChannels.InApp,
                        notifType:         NotifTypes.OrderAutoCancelled,
                        recipient:         customerEmail,
                        subject:           $"Order {order.OrderNumber} cancelled",
                        body:              $"Your order {order.OrderNumber} was cancelled because payment was not submitted within 24 hours.",
                        userId:            order.UserId,
                        orderId:           order.OrderId,
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex,
                        "PendingOrderMonitorJob: failed to queue OrderAutoCancelled notification for order {OrderId}.",
                        order.OrderId);
                }
            }

            // -- 4. Send pre-expiry reminders --
            foreach (Order order in reminderOrders)
            {
                if (order.User?.Email is not string customerEmail)
                {
                    _logger.LogWarning(
                        "PendingOrderMonitorJob: pending order {OrderId} has no customer email — skipping reminder.",
                        order.OrderId);
                    continue;
                }

                DateTime cancelAt = order.OrderDate + CancelAfter;

                try
                {
                    await notifications.QueueAsync(
                        channel:           NotifChannels.Email,
                        notifType:         NotifTypes.PendingOrderReminder,
                        recipient:         customerEmail,
                        subject:           $"Reminder — complete payment for order {order.OrderNumber}",
                        body:              $"Your order {order.OrderNumber} is still awaiting payment and will be " +
                                           $"automatically cancelled at {cancelAt:MMMM d, yyyy h:mm tt} UTC " +
                                           $"if we do not receive proof of payment. Please submit your payment proof to avoid cancellation.",
                        userId:            order.UserId,
                        orderId:           order.OrderId,
                        cancellationToken: cancellationToken);

                    await notifications.QueueAsync(
                        channel:           NotifChannels.InApp,
                        notifType:         NotifTypes.PendingOrderReminder,
                        recipient:         customerEmail,
                        subject:           $"Payment reminder — Order {order.OrderNumber}",
                        body:              $"Your order {order.OrderNumber} is awaiting payment and will be cancelled at {cancelAt:MMMM d, yyyy h:mm tt} UTC if unpaid.",
                        userId:            order.UserId,
                        orderId:           order.OrderId,
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex,
                        "PendingOrderMonitorJob: failed to queue PendingOrderReminder for order {OrderId}.",
                        order.OrderId);
                }
            }

            if (expiredOrders.Count > 0 || reminderOrders.Count > 0)
            {
                _logger.LogInformation(
                    "PendingOrderMonitorJob: cycle complete — cancelled {CancelCount}, reminded {RemindCount}.",
                    expiredOrders.Count, reminderOrders.Count);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle completed. " +
                                   $"{expiredOrders.Count} auto-cancelled, {reminderOrders.Count} reminded.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PendingOrderMonitorJob cycle failed.");

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
                        EventDescription = $"{nameof(PendingOrderMonitorJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);
                    await ec.SaveChangesAsync(cancellationToken);
                }
                catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for PendingOrderMonitorJob."); }
            }
        }
    }
}
