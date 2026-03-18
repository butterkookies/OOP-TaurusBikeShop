// WebApplication/BackgroundJobs/PendingOrderMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

public sealed class PendingOrderMonitorJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval    = TimeSpan.FromMinutes(5);
    private static readonly TimeSpan StaleOrderWindow = TimeSpan.FromHours(2);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PendingOrderMonitorJob> _logger;

    public PendingOrderMonitorJob(IServiceScopeFactory scopeFactory, ILogger<PendingOrderMonitorJob> logger)
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
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
            DateTime staleThreshold = DateTime.UtcNow - StaleOrderWindow;
            List<int> staleOrderIds = await context.Orders
                .AsNoTracking()
                .Where(o => o.OrderStatus == OrderStatuses.Pending && o.OrderDate < staleThreshold)
                .Select(o => o.OrderId)
                .ToListAsync(cancellationToken);

            if (staleOrderIds.Count > 0)
                _logger.LogWarning("PendingOrderMonitorJob: {Count} order(s) Pending > {Hours}h: {Ids}",
                    staleOrderIds.Count, StaleOrderWindow.TotalHours, string.Join(", ", staleOrderIds));
            else
                _logger.LogDebug("PendingOrderMonitorJob: no stale pending orders found.");

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(PendingOrderMonitorJob)} cycle completed. {staleOrderIds.Count} stale pending order(s) detected.",
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
