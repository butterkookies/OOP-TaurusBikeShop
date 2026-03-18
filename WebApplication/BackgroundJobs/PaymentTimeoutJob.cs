// WebApplication/BackgroundJobs/PaymentTimeoutJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

/// <summary>
/// Background job that enforces the 24-hour payment verification deadline.
/// Orders in <c>PendingVerification</c> status whose payment proof was uploaded
/// more than 24 hours ago are moved to <c>OnHold</c>.
/// Runs every 10 minutes.
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
public sealed class PaymentTimeoutJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval       = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan VerificationDeadline = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PaymentTimeoutJob> _logger;

    /// <inheritdoc/>
    public PaymentTimeoutJob(
        IServiceScopeFactory scopeFactory,
        ILogger<PaymentTimeoutJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentTimeoutJob started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            await RunCycleAsync(stoppingToken);
            await Task.Delay(CycleInterval, stoppingToken).ConfigureAwait(false);
        }

        _logger.LogInformation("PaymentTimeoutJob stopping.");
    }

    private async Task RunCycleAsync(CancellationToken cancellationToken)
    {
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();
        AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.SystemLogs.AddAsync(new SystemLog
        {
            EventType        = SystemLogEvents.BackgroundJobStart,
            EventDescription = $"{nameof(PaymentTimeoutJob)} cycle started.",
            CreatedAt        = DateTime.UtcNow
        }, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        try
        {
            DateTime deadline = DateTime.UtcNow - VerificationDeadline;
            int timeoutCount = 0;

            // Find orders in PendingVerification where the proof was uploaded > 24h ago.
            // The proof upload timestamp is recorded in Payment.CreatedAt for the
            // VerificationPending payment row.
            List<Order> timedOutOrders = await context.Orders
                .Include(o => o.Payments)
                .Where(o => o.OrderStatus == OrderStatuses.PendingVerification
                         && o.Payments.Any(p =>
                             p.PaymentStatus == PaymentStatuses.VerificationPending
                          && p.CreatedAt < deadline))
                .ToListAsync(cancellationToken);

            foreach (Order order in timedOutOrders)
            {
                order.OrderStatus = OrderStatuses.OnHold;

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.PaymentTimeout,
                    EventDescription =
                        $"Order {order.OrderNumber} (ID {order.OrderId}) moved to OnHold — " +
                        $"payment verification deadline exceeded.",
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);

                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription =
                        $"Order {order.OrderNumber} (ID {order.OrderId}): " +
                        $"PendingVerification → OnHold (payment timeout).",
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);

                timeoutCount++;
                _logger.LogWarning(
                    "PaymentTimeoutJob: order {OrderNumber} (ID {OrderId}) placed OnHold.",
                    order.OrderNumber, order.OrderId);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription =
                    $"{nameof(PaymentTimeoutJob)} cycle completed. " +
                    $"{timeoutCount} order(s) moved to OnHold.",
                CreatedAt = DateTime.UtcNow
            }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PaymentTimeoutJob cycle failed.");

            try
            {
                await using AsyncServiceScope errorScope = _scopeFactory.CreateAsyncScope();
                AppDbContext errorContext = errorScope.ServiceProvider.GetRequiredService<AppDbContext>();

                await errorContext.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(PaymentTimeoutJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await errorContext.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx)
            {
                _logger.LogError(logEx, "Failed to write BackgroundJobError log for PaymentTimeoutJob.");
            }
        }
    }
}
