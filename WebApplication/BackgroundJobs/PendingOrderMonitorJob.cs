// WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that monitors orders stuck in <c>Pending</c> status.
/// Runs every 5 minutes. Logs any orders that have been in Pending status
/// for an abnormally long time so that admins can investigate.
/// <para>
/// Each cycle writes <see cref="SystemLogEvents.BackgroundJobStart"/>,
/// <see cref="SystemLogEvents.BackgroundJobComplete"/>, or
/// <see cref="SystemLogEvents.BackgroundJobError"/> to <c>SystemLog</c>.
/// </para>
/// <para>
/// <b>IServiceScopeFactory pattern:</b> <see cref="AppDbContext"/> is scoped;
/// a new scope is created on every cycle.
/// </para>
/// </summary>
public sealed class PendingOrderMonitorJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval    = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan StaleOrderWindow = TimeSpan.FromHours(2);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingOrderMonitorJob> _logger;

    /// <inheritdoc/>
    public PendingOrderMonitorJob(
        IServiceScopeFactory scopeFactory,
        ILogger<PendingOrderMonitorJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PendingOrderMonitorJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }

        _logger.LogInformation("PendingOrderMonitorJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.SystemLogs.AddAsync(new SystemLog
        {
            EventType        = SystemLogEvents.BackgroundJobStart,
            EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle started.",
            CreatedAt        = DateTime.UtcNow
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            DateTime staleThreshold = DateTime.UtcNow - StaleOrderWindow;

            // Find orders that have been Pending for longer than the stale window
            List<int> staleOrderIds = await context.Orders
                .AsNoTracking()
                .Where(o => o.OrderStatus == OrderStatuses.Pending
                         && o.OrderDate < staleThreshold)
                .Select(o => o.OrderId)
                .ToListAsync(cancellationToken);

            if (staleOrderIds.Count > 0)
            {
                _logger.LogWarning(
                    "PendingOrderMonitorJob: {Count} order(s) have been Pending for > {Hours}h: {Ids}",
                    staleOrderIds.Count,
                    StaleOrderWindow.TotalHours,
                    string.Join(", ", staleOrderIds));
            }
            else
            {
                _logger.LogDebug("PendingOrderMonitorJob: no stale pending orders found.");
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription =
                    $"{nameof(PendingOrderMonitorJob)} cycle completed. " +
                    $"{staleOrderIds.Count} stale pending order(s) detected.",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PendingOrderMonitorJob cycle failed.");

            try
            {
                await using AsyncServiceScope errorScope = _scopeFactory.CreateAsyncScope();
                AppDbContext errorContext = errorScope.ServiceProvider.GetRequiredService<AppDbContext>();

                await errorContext.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(PendingOrderMonitorJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await errorContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to write BackgroundJobError log for PendingOrderMonitorJob.");
            }
        }
    }
}
