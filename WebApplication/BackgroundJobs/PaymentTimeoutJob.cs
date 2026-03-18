// WebApplication/BackgroundJobs/PaymentTimeoutJob.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BackgroundJobs;

public sealed class PaymentTimeoutJob : BackgroundService
{
    private static readonly TimeSpan CycleInterval        = TimeSpan.FromMinutes(10);
    private static readonly TimeSpan VerificationDeadline = TimeSpan.FromHours(24);
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PaymentTimeoutJob> _logger;

    public PaymentTimeoutJob(IServiceScopeFactory scopeFactory, ILogger<PaymentTimeoutJob> logger)
    {
        _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PaymentTimeoutJob started.");
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
        _logger.LogInformation("PaymentTimeoutJob stopping.");
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
                EventDescription = $"{nameof(PaymentTimeoutJob)} cycle started.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogWarning(ex, "{Job} could not write start log — DB may be unavailable.",
                nameof(PaymentTimeoutJob));
        }

        try
        {
            DateTime deadline    = DateTime.UtcNow - VerificationDeadline;
            int      timeoutCount = 0;

            List<Order> timedOutOrders = await context.Orders
                .Include(o => o.Payments)
                .Where(o => o.OrderStatus == OrderStatuses.PendingVerification
                         && o.Payments.Any(p => p.PaymentStatus == PaymentStatuses.VerificationPending
                                             && p.CreatedAt < deadline))
                .ToListAsync(cancellationToken);

            foreach (Order order in timedOutOrders)
            {
                order.OrderStatus = OrderStatuses.OnHold;
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.PaymentTimeout,
                    EventDescription = $"Order {order.OrderNumber} (ID {order.OrderId}) moved to OnHold — payment verification deadline exceeded.",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await context.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription = $"Order {order.OrderNumber} (ID {order.OrderId}): PendingVerification → OnHold (payment timeout).",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                timeoutCount++;
                _logger.LogWarning("PaymentTimeoutJob: order {OrderNumber} (ID {OrderId}) placed OnHold.",
                    order.OrderNumber, order.OrderId);
            }

            await context.SystemLogs.AddAsync(new SystemLog
            {
                EventType        = SystemLogEvents.BackgroundJobComplete,
                EventDescription = $"{nameof(PaymentTimeoutJob)} cycle completed. {timeoutCount} order(s) moved to OnHold.",
                CreatedAt        = DateTime.UtcNow
            }, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex) when (ex is not OperationCanceledException)
        {
            _logger.LogError(ex, "PaymentTimeoutJob cycle failed.");
            try
            {
                await using AsyncServiceScope err = _scopeFactory.CreateAsyncScope();
                AppDbContext ec = err.ServiceProvider.GetRequiredService<AppDbContext>();
                await ec.SystemLogs.AddAsync(new SystemLog
                {
                    EventType        = SystemLogEvents.BackgroundJobError,
                    EventDescription = $"{nameof(PaymentTimeoutJob)} error: {ex.Message[..Math.Min(ex.Message.Length, 200)]}",
                    CreatedAt        = DateTime.UtcNow
                }, cancellationToken);
                await ec.SaveChangesAsync(cancellationToken);
            }
            catch (Exception logEx) { _logger.LogError(logEx, "Failed to write error log for PaymentTimeoutJob."); }
        }
    }
}
