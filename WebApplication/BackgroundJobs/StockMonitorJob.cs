// WebApplication/BackgroundJobs/StockMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

public sealed class StockMonitorJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(15);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockMonitorJob> _logger;

    public StockMonitorJob(IServiceScopeFactory scopeFactory, ILogger<StockMonitorJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StockMonitorJob started.");
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
        _logger.LogInformation("StockMonitorJob stopping.");
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
                EventDescription = $"{nameof(StockMonitorJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(StockMonitorJob));
        }

        try
        {
            var lowStockVariants = await context.ProductVariants
                .AsNoTracking()
                .Include(v => v.Product)
                .Where(v => v.IsActive && v.StockQuantity < v.ReorderThreshold)
                .Select(v => new
                {
                    v.ProductVariantId, v.VariantName, v.StockQuantity, v.ReorderThreshold,
                    ProductName = v.Product.Name, ProductId = v.ProductId
                })
                .ToListAsync(cancellationToken);

            foreach (var variant in lowStockVariants)
            {
                _logger.LogWarning(
                    "StockMonitorJob: low stock — variant {VariantId} ({ProductName} / {VariantName}): {Stock} remaining (threshold {Threshold}).",
                    variant.ProductVariantId, variant.ProductName, variant.VariantName,
                    variant.StockQuantity, variant.ReorderThreshold);

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.LowStockTriggered,
                    EventDescription = $"Low stock: {variant.ProductName} / {variant.VariantName} (VariantId {variant.ProductVariantId}) — {variant.StockQuantity} unit(s) remaining (threshold {variant.ReorderThreshold}).",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(StockMonitorJob)} cycle completed. {lowStockVariants.Count} low-stock variant(s) detected.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "StockMonitorJob cycle failed.");
            try
            {
                await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                await ec.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(StockMonitorJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await ec.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for StockMonitorJob."); }
        }
    }
}
