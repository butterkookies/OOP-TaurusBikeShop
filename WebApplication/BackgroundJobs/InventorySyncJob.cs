// WebApplication/BackgroundJobs/InventorySyncJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

public sealed class InventorySyncJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(60);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<InventorySyncJob> _logger;

    public InventorySyncJob(IServiceScopeFactory scopeFactory, ILogger<InventorySyncJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InventorySyncJob started.");
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
        _logger.LogInformation("InventorySyncJob stopping.");
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
                EventDescription = $"{nameof(InventorySyncJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(InventorySyncJob));
        }

        try
        {
            int variantCount = await context.ProductVariants.AsNoTracking().CountAsync(cancellationToken);
            _logger.LogInformation("InventorySyncJob: checked {VariantCount} product variants.", variantCount);

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.InventorySync,
                EventDescription = $"Inventory sync completed. {variantCount} variants checked.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(InventorySyncJob)} cycle completed.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "InventorySyncJob cycle failed.");
            try
            {
                await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                await ec.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(InventorySyncJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await ec.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for InventorySyncJob."); }
        }
    }
}
