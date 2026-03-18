// WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

public sealed class DeliveryStatusPollJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(5);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DeliveryStatusPollJob> _logger;

    public DeliveryStatusPollJob(IServiceScopeFactory scopeFactory, ILogger<DeliveryStatusPollJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeliveryStatusPollJob started.");
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
        _logger.LogInformation("DeliveryStatusPollJob stopping.");
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
                EventDescription = $"{nameof(DeliveryStatusPollJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(DeliveryStatusPollJob));
        }

        try
        {
            List<Delivery> activeDeliveries = await context.Deliveries
                .Where(d => d.DeliveryStatus == DeliveryStatuses.PickedUp
                         || d.DeliveryStatus == DeliveryStatuses.InTransit)
                .ToListAsync(cancellationToken);

            int pollCount = 0;
            foreach (Delivery delivery in activeDeliveries)
            {
                string? newStatus = await PollCourierStatusAsync(delivery, cancellationToken);
                if (newStatus is not null && newStatus != delivery.DeliveryStatus)
                {
                    _logger.LogInformation(
                        "DeliveryStatusPollJob: delivery {DeliveryId} status {OldStatus} → {NewStatus}.",
                        delivery.DeliveryId, delivery.DeliveryStatus, newStatus);
                    delivery.DeliveryStatus = newStatus;
                    await context.SystemLogs.AddAsync(new SystemLog
                    {
                        EventType        = SystemLogEvents.DeliveryStatusPoll,
                        EventDescription = $"Delivery {delivery.DeliveryId} (order {delivery.OrderId}): status updated to {newStatus}.",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);
                }
                pollCount++;
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(DeliveryStatusPollJob)} cycle completed. {pollCount} active delivery/deliveries polled.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "DeliveryStatusPollJob cycle failed.");
            try
            {
                await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                await ec.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(DeliveryStatusPollJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await ec.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for DeliveryStatusPollJob."); }
        }
    }

    /// <summary>
    /// Placeholder — always returns null. No courier API is integrated yet.
    /// </summary>
    private static Task<string?> PollCourierStatusAsync(
        Delivery delivery, CancellationToken cancellationToken)
        => Task.FromResult<string?>(null);
}
