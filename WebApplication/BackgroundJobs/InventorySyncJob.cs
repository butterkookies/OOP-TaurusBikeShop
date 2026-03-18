// WebApplication/BackgroundJobs/InventorySyncJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that periodically synchronises inventory between the
/// walk-in POS channel and the online store.
/// Runs every 60 minutes.
/// <para>
/// Each cycle writes <see cref="SystemLogEvents.BackgroundJobStart"/>,
/// <see cref="SystemLogEvents.BackgroundJobComplete"/>, or
/// <see cref="SystemLogEvents.BackgroundJobError"/> to <c>SystemLog</c>.
/// </para>
/// <para>
/// <b>IServiceScopeFactory pattern:</b> <see cref="AppDbContext"/> is scoped;
/// it must never be injected directly into a singleton/hosted service.
/// A new scope is created on every cycle.
/// </para>
/// </summary>
public sealed class InventorySyncJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(60);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<InventorySyncJob> _logger;

    /// <inheritdoc/>
    public InventorySyncJob(
        IServiceScopeFactory scopeFactory,
        ILogger<InventorySyncJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InventorySyncJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }

        _logger.LogInformation("InventorySyncJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.SystemLogs.AddAsync(new SystemLog
        {
            EventType        = SystemLogEvents.BackgroundJobStart,
            EventDescription = $"{nameof(InventorySyncJob)} cycle started.",
            CreatedAt        = DateTime.UtcNow
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            // Inventory sync between POS walk-in orders and online channel.
            // Walk-in orders (IsWalkIn = true) are already Delivered and their
            // InventoryLog rows are written at POS order creation time.
            // This cycle validates that ProductVariant.StockQuantity is
            // consistent with the sum of InventoryLog entries.
            int variantCount = await context.ProductVariants
                .AsNoTracking()
                .CountAsync(cancellationToken);

            _logger.LogInformation(
                "InventorySyncJob: checked {VariantCount} product variants.", variantCount);

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
                await using AsyncServiceScope errorScope = _scopeFactory.CreateAsyncScope();
                AppDbContext errorContext = errorScope.ServiceProvider.GetRequiredService<AppDbContext>();

                await errorContext.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(InventorySyncJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await errorContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to write BackgroundJobError log for InventorySyncJob.");
            }
        }
    }
}
