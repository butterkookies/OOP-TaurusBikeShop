// WebApplication/BackgroundJobs/DeliveryStatusPollJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that polls courier APIs every 5 minutes to update delivery statuses.
/// Covers Lalamove and LBC deliveries that are in a non-terminal status
/// (Pending, PickedUp, or InTransit).
/// <para>
/// <b>Courier API integration is not yet implemented.</b>
/// <see cref="PollCourierStatusAsync"/> is a placeholder that returns <c>null</c>.
/// When real courier API clients are available, replace the body of that method
/// without changing the job's structure or SystemLog behaviour.
/// </para>
/// <para>
/// Each cycle writes <see cref="SystemLogEvents.BackgroundJobStart"/>,
/// <see cref="SystemLogEvents.BackgroundJobComplete"/>, or
/// <see cref="SystemLogEvents.BackgroundJobError"/> to <c>SystemLog</c>.
/// Active deliveries polled are recorded with <see cref="SystemLogEvents.DeliveryStatusPoll"/>.
/// </para>
/// <para>
/// <b>IServiceScopeFactory pattern:</b> <see cref="AppDbContext"/> is scoped;
/// a new scope is created on every cycle.
/// </para>
/// </summary>
public sealed class DeliveryStatusPollJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Delivery statuses that are not yet terminal and therefore require polling.
    /// </summary>
    private static readonly IReadOnlySet<string> ActiveStatuses = new HashSet<string>
    {
        DeliveryStatuses.Pending,
        DeliveryStatuses.PickedUp,
        DeliveryStatuses.InTransit
    };

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<DeliveryStatusPollJob> _logger;

    /// <inheritdoc/>
    public DeliveryStatusPollJob(
        IServiceScopeFactory scopeFactory,
        ILogger<DeliveryStatusPollJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("DeliveryStatusPollJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }

        _logger.LogInformation("DeliveryStatusPollJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.SystemLogs.AddAsync(new SystemLog
        {
            EventType        = SystemLogEvents.BackgroundJobStart,
            EventDescription = $"{nameof(DeliveryStatusPollJob)} cycle started.",
            CreatedAt        = DateTime.UtcNow
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            // Find all deliveries in a non-terminal status
            List<Delivery> activeDeliveries = await context.Deliveries
                .Where(d => ActiveStatuses.Contains(d.DeliveryStatus))
                .ToListAsync(cancellationToken);

            int pollCount = 0;

            foreach (Delivery delivery in activeDeliveries)
            {
                // Poll the courier API for the latest status.
                // PollCourierStatusAsync is a placeholder — returns null until
                // the real courier API clients are integrated.
                string? newStatus = await PollCourierStatusAsync(delivery, cancellationToken);

                if (newStatus is not null && newStatus != delivery.DeliveryStatus)
                {
                    // Status update returned — apply it.
                    // This path is not reachable until PollCourierStatusAsync
                    // is implemented; kept here as a scaffold for future integration.
                    _logger.LogInformation(
                        "DeliveryStatusPollJob: delivery {DeliveryId} status " +
                        "{OldStatus} → {NewStatus}.",
                        delivery.DeliveryId, delivery.DeliveryStatus, newStatus);

                    delivery.DeliveryStatus = newStatus;

                    await context.SystemLogs.AddAsync(new SystemLog
                    {
                        EventType        = SystemLogEvents.DeliveryStatusPoll,
                        EventDescription =
                            $"Delivery {delivery.DeliveryId} (order {delivery.OrderId}): " +
                            $"status updated to {newStatus}.",
                        CreatedAt = DateTime.UtcNow
                    }, cancellationToken);
                }

                pollCount++;
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription =
                    $"{nameof(DeliveryStatusPollJob)} cycle completed. " +
                    $"{pollCount} active delivery/deliveries polled.",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "DeliveryStatusPollJob cycle failed.");

            try
            {
                await using AsyncServiceScope errorScope = _scopeFactory.CreateAsyncScope();
                AppDbContext errorContext = errorScope.ServiceProvider.GetRequiredService<AppDbContext>();

                await errorContext.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(DeliveryStatusPollJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await errorContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to write BackgroundJobError log for DeliveryStatusPollJob.");
            }
        }
    }

    /// <summary>
    /// Polls the appropriate courier API for the current delivery status.
    /// </summary>
    /// <param name="delivery">The delivery to poll.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The new <c>DeliveryStatus</c> string (a <see cref="DeliveryStatuses"/> constant)
    /// when the courier API returns a different status than the one currently recorded,
    /// or <c>null</c> when there is no change or the API is not yet integrated.
    /// </returns>
    /// <remarks>
    /// <b>Placeholder — always returns <c>null</c>.</b>
    /// No courier API clients are integrated yet. Do not add fake status updates here.
    /// When real Lalamove or LBC API clients are available, implement the call here
    /// and map their status codes to the corresponding <see cref="DeliveryStatuses"/> constant.
    /// </remarks>
    private static Task<string?> PollCourierStatusAsync(
        Delivery delivery,
        CancellationToken cancellationToken)
    {
        // Placeholder: real courier API integration not yet implemented.
        // Returns null — no status update is applied.
        return Task.FromResult<string?>(null);
    }
}
