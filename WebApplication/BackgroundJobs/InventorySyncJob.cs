// WebApplication/BackgroundJobs/InventorySyncJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Polls the product-variant table every 10 seconds to keep a live inventory
/// count available to the rest of the application.
/// Flowchart: Part 11 — "Every 10 Seconds".
/// <para>
/// SystemLog summary entries are throttled to once every 5 minutes to prevent
/// log flooding (10-second intervals would otherwise produce ~8,640 rows/day
/// of BackgroundJobStart/Complete noise).
/// </para>
/// </summary>
public sealed class InventorySyncJob : BackgroundService
{
    // Flowchart Part 11: sync every 60 seconds (was 10s — reduced to cut
    // DbContext allocation churn that contributed to process memory exhaustion).
    private static readonly TimeSpan CycleInterval      = TimeSpan.FromSeconds(60);

    // Write a SystemLog summary entry at most once per this interval.
    private static readonly TimeSpan SummaryLogInterval = TimeSpan.FromMinutes(5);

    private readonly IServiceScopeFactory      _scopeFactory;
    private readonly ILogger<InventorySyncJob> _logger;

    // BackgroundService is registered as singleton — this field persists
    // across cycles and controls the SystemLog write throttle.
    private DateTime _nextSummaryLogAt = DateTime.MinValue;

    // Exponential backoff: tracks consecutive failures to avoid log flooding
    // when the database is unavailable.
    private int _consecutiveFailures;
    private static readonly TimeSpan MaxBackoff = TimeSpan.FromMinutes(2);

    public InventorySyncJob(
        IServiceScopeFactory      scopeFactory,
        ILogger<InventorySyncJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("InventorySyncJob started.");

        // Staggered startup: delay before first cycle to prevent all background
        // services from hitting the DB simultaneously at boot.
        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken).ConfigureAwait(false);

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

            // Exponential backoff: delay increases with consecutive failures,
            // capped at MaxBackoff to avoid flooding logs when DB is down.
            TimeSpan delay = _consecutiveFailures > 0
                ? TimeSpan.FromSeconds(Math.Min(MaxBackoff.TotalSeconds,
                    CycleInterval.TotalSeconds * Math.Pow(2, _consecutiveFailures - 1)))
                : CycleInterval;
            await Task.Delay(delay, stoppingToken).ConfigureAwait(false);
        }
        _logger.LogInformation("InventorySyncJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            int variantCount = await context.ProductVariants
                .AsNoTracking()
                .CountAsync(cancellationToken);

            // Debug-level log on every cycle — only visible when debug logging
            // is explicitly enabled; does not write to SystemLog.
            _logger.LogDebug("InventorySyncJob: checked {VariantCount} product variants.",
                variantCount);

            // Throttle SystemLog writes — skip writing a DB row on every 10-second
            // cycle; only persist a summary entry once per SummaryLogInterval.
            bool writeSummaryLog = DateTime.UtcNow >= _nextSummaryLogAt;
            if (writeSummaryLog)
            {
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.InventorySync,
                    EventDescription = $"Inventory sync completed. {variantCount} variants checked.",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobComplete,
                    EventDescription = $"{nameof(InventorySyncJob)} periodic summary logged.",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                _nextSummaryLogAt = DateTime.UtcNow.Add(SummaryLogInterval);
            }
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "InventorySyncJob cycle failed.");

            // Skip DB error-logging when the failure is itself a DB connectivity
            // problem — writing to the DB would fail again and double the noise.
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
                        EventDescription = $"{nameof(InventorySyncJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);
                    await ec.SaveChangesAsync(cancellationToken);
                }
                catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for InventorySyncJob."); }
            }
        }
    }
}
