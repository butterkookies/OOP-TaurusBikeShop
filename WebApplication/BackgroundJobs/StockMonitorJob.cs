// WebApplication/BackgroundJobs/StockMonitorJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that polls product variant stock levels every 15 minutes.
/// When a variant's <c>StockQuantity</c> drops below its <c>ReorderThreshold</c>,
/// a <see cref="SystemLogEvents.LowStockTriggered"/> event is written to
/// <c>SystemLog</c> for admin visibility.
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
public sealed class StockMonitorJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(15);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<StockMonitorJob> _logger;

    /// <inheritdoc/>
    public StockMonitorJob(
        IServiceScopeFactory scopeFactory,
        ILogger<StockMonitorJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("StockMonitorJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }

        _logger.LogInformation("StockMonitorJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.SystemLogs.AddAsync(new SystemLog
        {
            EventType        = SystemLogEvents.BackgroundJobStart,
            EventDescription = $"{nameof(StockMonitorJob)} cycle started.",
            CreatedAt        = DateTime.UtcNow
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            // Find active variants whose stock is below their reorder threshold
            var lowStockVariants = await context.ProductVariants
                .AsNoTracking()
                .Include(v => v.Product)
                .Where(v => v.IsActive && v.StockQuantity < v.ReorderThreshold)
                .Select(v => new
                {
                    v.ProductVariantId,
                    v.VariantName,
                    v.StockQuantity,
                    v.ReorderThreshold,
                    ProductName = v.Product.Name,
                    ProductId   = v.ProductId
                })
                .ToListAsync(cancellationToken);

            foreach (var variant in lowStockVariants)
            {
                _logger.LogWarning(
                    "StockMonitorJob: low stock — variant {VariantId} ({ProductName} / {VariantName}): " +
                    "{Stock} remaining (threshold {Threshold}).",
                    variant.ProductVariantId,
                    variant.ProductName,
                    variant.VariantName,
                    variant.StockQuantity,
                    variant.ReorderThreshold);

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.LowStockTriggered,
                    EventDescription =
                        $"Low stock: {variant.ProductName} / {variant.VariantName} " +
                        $"(VariantId {variant.ProductVariantId}) — " +
                        $"{variant.StockQuantity} unit(s) remaining " +
                        $"(threshold {variant.ReorderThreshold}).",
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription =
                    $"{nameof(StockMonitorJob)} cycle completed. " +
                    $"{lowStockVariants.Count} low-stock variant(s) detected.",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "StockMonitorJob cycle failed.");

            try
            {
                await using AsyncServiceScope errorScope = _scopeFactory.CreateAsyncScope();
                AppDbContext errorContext = errorScope.ServiceProvider.GetRequiredService<AppDbContext>();

                await errorContext.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(StockMonitorJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await errorContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to write BackgroundJobError log for StockMonitorJob.");
            }
        }
    }
}
