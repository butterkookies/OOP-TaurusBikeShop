// WebApplication/BackgroundJobs/PaymentTimeoutJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Checks every 5 minutes for orders in PendingVerification whose bank-transfer
/// payment proof has passed its <c>BankTransferPayment.VerificationDeadline</c>.
/// Moves them to OnHold and queues a PaymentHeld notification to the customer.
/// Flowchart: Part 13 / J3 — "Every 5 Minutes", J3B — "Queue Alert".
/// <para>
/// The deadline is read from <c>BankTransferPayment.VerificationDeadline</c> —
/// the admin-settable column written by <c>PaymentService</c> at proof-upload
/// time (<c>SubmittedAt + 24 h</c>). This column is the authoritative source:
/// it reflects the actual proof-upload timestamp and can be extended by an admin
/// in the AdminSystem. Using <c>Payment.CreatedAt</c> as a fallback would
/// ignore both of these facts and produce incorrect timeouts.
/// </para>
/// <para>
/// Execution order within each cycle:
/// <list type="number">
///   <item>Load expired orders — filter on <c>BankTransferPayment.VerificationDeadline &lt; UtcNow</c>.</item>
///   <item>Set OrderStatus = OnHold + write SystemLog entries for each order.</item>
///   <item>SaveChangesAsync — status transitions are committed before any notification
///     work, so a partial notification failure never leaves an order in a
///     corrupted state.</item>
///   <item>Queue PaymentHeld email per customer (per-order try/catch so one bad
///     address never prevents other customers from being notified).</item>
/// </list>
/// </para>
/// </summary>
public sealed class PaymentTimeoutJob : BackgroundService
{
    // Flowchart Part 13/J3: check every 5 minutes.
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory       _scopeFactory;
    private readonly ILogger<PaymentTimeoutJob> _logger;

    public PaymentTimeoutJob(
        IServiceScopeFactory       scopeFactory,
        ILogger<PaymentTimeoutJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentTimeoutJob started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunCycleAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogError(ex, "{Job} unhandled exception — will retry next cycle.",
                    GetType().Name);
            }
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }
        _logger.LogInformation("PaymentTimeoutJob stopping.");
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
                EventDescription = $"{nameof(PaymentTimeoutJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(PaymentTimeoutJob));
        }

        try
        {
            DateTime now          = DateTime.UtcNow;
            int      timeoutCount = 0;

            // Filter on BankTransferPayment.VerificationDeadline — the admin-settable
            // column written at proof-upload time. This is the authoritative deadline:
            // it is NULL until the customer submits proof (so orders with no proof
            // uploaded are never timed out), and it can be extended by an admin.
            // Include o.User so the customer's email address is available for the
            // PaymentHeld notification queued after the save (flowchart J3B).
            List<Order> timedOutOrders = await context.Orders
                .Include(o => o.Payments)
                .Include(o => o.User)
                .Where(o => o.OrderStatus == OrderStatuses.PendingVerification
                         && o.Payments.Any(p => p.PaymentStatus == PaymentStatuses.VerificationPending
                                             && p.PaymentMethod == PaymentMethods.BankTransfer
                                             && p.BankTransferPayment != null
                                             && p.BankTransferPayment.VerificationDeadline != null
                                             && p.BankTransferPayment.VerificationDeadline < now))
                .ToListAsync(cancellationToken);

            foreach (Order order in timedOutOrders)
            {
                order.OrderStatus = OrderStatuses.OnHold;
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.PaymentTimeout,
                    EventDescription = $"Order {order.OrderNumber} (ID {order.OrderId}) moved to OnHold — payment verification deadline exceeded.",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription = $"Order {order.OrderNumber} (ID {order.OrderId}): PendingVerification \u2192 OnHold (payment timeout).",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                timeoutCount++;
                _logger.LogWarning("PaymentTimeoutJob: order {OrderNumber} (ID {OrderId}) placed OnHold.",
                    order.OrderNumber, order.OrderId);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(PaymentTimeoutJob)} cycle completed. {timeoutCount} order(s) moved to OnHold.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);

            // Commit all status changes and log entries before notification work.
            // If notifications fail, orders are still correctly in OnHold.
            await context.SaveChangesAsync(cancellationToken);

            // -- Queue PaymentHeld notification per customer (flowchart J3B) --
            // Executed after SaveChangesAsync so a notification failure never
            // rolls back already-committed status transitions.
            foreach (Order order in timedOutOrders)
            {
                if (order.User?.Email is not string customerEmail)
                {
                    _logger.LogWarning(
                        "PaymentTimeoutJob: order {OrderId} has no customer email — skipping notification.",
                        order.OrderId);
                    continue;
                }

                try
                {
                    await notifications.QueueAsync(
                        channel:           NotifChannels.Email,
                        notifType:         NotifTypes.PaymentHeld,
                        recipient:         customerEmail,
                        subject:           $"Action Required \u2014 Order {order.OrderNumber} On Hold",
                        body:              $"Your order {order.OrderNumber} has been placed on hold because " +
                                           $"payment verification was not completed within 24 hours. " +
                                           $"Please contact us to resolve this.",
                        userId:            order.UserId,
                        orderId:           order.OrderId,
                        cancellationToken: cancellationToken);
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    _logger.LogError(ex,
                        "PaymentTimeoutJob: failed to queue PaymentHeld notification for order {OrderId}.",
                        order.OrderId);
                }
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PaymentTimeoutJob cycle failed.");
            try
            {
                await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                await ec.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(PaymentTimeoutJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await ec.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for PaymentTimeoutJob."); }
        }
    }
}
