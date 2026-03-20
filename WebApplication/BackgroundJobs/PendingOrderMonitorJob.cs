// WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Checks every 5 minutes for orders that have been in Pending status for
/// over 24 hours and queues a PendingOrderAlert notification to all admin
/// and manager users.
/// Flowchart: Part 12 / J2 — "Every 5 Minutes", alert threshold "> 24 Hours",
/// J2B — "Notify Admin".
/// <para>
/// Deduplication: a PendingOrderAlert is only queued once per order per 24-hour
/// window. The check is performed against the Notifications table — if any
/// PendingOrderAlert row for the same OrderId was inserted within the last 24
/// hours the order is skipped for this cycle.
/// </para>
/// </summary>
public sealed class PendingOrderMonitorJob : BackgroundService
{
    // Flowchart Part 12/J2: run every 5 minutes; flag orders pending > 24 hours.
    private static readonly TimeSpan CycleInterval    = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan StaleOrderWindow = TimeSpan.FromHours(24);
    private static readonly TimeSpan AlertCooldown    = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory            _scopeFactory;
    private readonly ILogger<PendingOrderMonitorJob> _logger;

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
        _logger.LogInformation("PendingOrderMonitorJob stopping.");
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
                EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(PendingOrderMonitorJob));
        }

        try
        {
            DateTime now            = DateTime.UtcNow;
            DateTime staleThreshold = now - StaleOrderWindow;
            DateTime cooldownCutoff = now - AlertCooldown;

            // Fetch stale orders — OrderNumber is required for the notification body.
            var staleOrders = await context.Orders
                .AsNoTracking()
                .Where(o => o.OrderStatus == OrderStatuses.Pending
                         && o.OrderDate   < staleThreshold)
                .Select(o => new { o.OrderId, o.OrderNumber })
                .ToListAsync(cancellationToken);

            if (staleOrders.Count == 0)
            {
                _logger.LogDebug("PendingOrderMonitorJob: no stale pending orders found.");
            }
            else
            {
                _logger.LogWarning(
                    "PendingOrderMonitorJob: {Count} order(s) Pending > {Hours}h: {Ids}",
                    staleOrders.Count,
                    StaleOrderWindow.TotalHours,
                    string.Join(", ", staleOrders.Select(o => o.OrderId)));

                // Load admin/manager recipients once per cycle — all must have
                // an email address to receive the alert (flowchart J2B).
                List<User> adminRecipients = await context.Users
                    .AsNoTracking()
                    .Where(u => u.UserRoles.Any(ur =>
                                    ur.Role.RoleName == RoleNames.Admin ||
                                    ur.Role.RoleName == RoleNames.Manager)
                             && u.Email != null)
                    .ToListAsync(cancellationToken);

                if (adminRecipients.Count == 0)
                    _logger.LogWarning(
                        "PendingOrderMonitorJob: no admin/manager users with email found — " +
                        "PendingOrderAlert notifications cannot be sent.");

                foreach (var staleOrder in staleOrders)
                {
                    try
                    {
                        // Dedup: skip this order if a PendingOrderAlert was already
                        // queued within the AlertCooldown window.
                        bool alreadyAlerted = await context.Notifications
                            .AnyAsync(n => n.NotifType == NotifTypes.PendingOrderAlert
                                       && n.OrderId   == staleOrder.OrderId
                                       && n.CreatedAt  > cooldownCutoff,
                                cancellationToken);

                        if (alreadyAlerted)
                        {
                            _logger.LogDebug(
                                "PendingOrderMonitorJob: alert already sent for order {OrderId} — skipping.",
                                staleOrder.OrderId);
                            continue;
                        }

                        // Queue one notification per admin recipient.
                        foreach (User admin in adminRecipients)
                        {
                            try
                            {
                                await notifications.QueueAsync(
                                    channel:           NotifChannels.Email,
                                    notifType:         NotifTypes.PendingOrderAlert,
                                    recipient:         admin.Email!,
                                    subject:           $"Pending Order Alert — {staleOrder.OrderNumber}",
                                    body:              $"Order {staleOrder.OrderNumber} (ID {staleOrder.OrderId}) " +
                                                       $"has been in Pending status for over " +
                                                       $"{(int)StaleOrderWindow.TotalHours} hours and requires attention.",
                                    userId:            admin.UserId,
                                    orderId:           staleOrder.OrderId,
                                    cancellationToken: cancellationToken);
                            }
                            catch (Exception ex) when (ex is not OperationCanceledException)
                            {
                                _logger.LogError(ex,
                                    "PendingOrderMonitorJob: failed to queue alert for admin {AdminId}, order {OrderId}.",
                                    admin.UserId, staleOrder.OrderId);
                            }
                        }
                    }
                    catch (Exception ex) when (ex is not OperationCanceledException)
                    {
                        _logger.LogError(ex,
                            "PendingOrderMonitorJob: error processing stale order {OrderId}.",
                            staleOrder.OrderId);
                    }
                }
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle completed. " +
                                   $"{staleOrders.Count} stale pending order(s) detected.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PendingOrderMonitorJob cycle failed.");
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
