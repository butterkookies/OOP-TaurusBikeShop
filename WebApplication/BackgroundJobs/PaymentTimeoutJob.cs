using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;

namespace WebApplication.BackgroundJobs
{
    public class PaymentTimeoutJob : BackgroundService
    {
        static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

        readonly IServiceScopeFactory _scope;
        readonly ILogger<PaymentTimeoutJob> _logger;

        public PaymentTimeoutJob(IServiceScopeFactory scope, ILogger<PaymentTimeoutJob> logger)
        {
            _scope  = scope;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try { await RunAsync(ct); }
                catch (Exception ex) when (!ct.IsCancellationRequested)
                {
                    _logger.LogError(ex, "PaymentTimeoutJob encountered an error.");
                }
                await Task.Delay(Interval, ct);
            }
        }

        async Task RunAsync(CancellationToken ct)
        {
            await using var scope = _scope.CreateAsyncScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var cutoff = DateTime.UtcNow.AddHours(-48);

            // Find orders in PendingVerification where the most recent payment is older than 48 hours
            var timedOutOrders = await ctx.Orders
                .Include(o => o.Payments)
                .Where(o => o.OrderStatus == "PendingVerification"
                         && o.Payments.Any()
                         && o.Payments.Max(p => p.CreatedAt) < cutoff)
                .ToListAsync(ct);

            foreach (var order in timedOutOrders)
            {
                _logger.LogWarning(
                    "Order {OrderNumber} payment verification has timed out (last payment over 48 hours ago).",
                    order.OrderNumber);

                order.OrderStatus = "PaymentTimeout";

                // Mark the last payment as Expired
                var lastPayment = order.Payments.OrderByDescending(p => p.CreatedAt).First();
                lastPayment.PaymentStatus = "Expired";
            }

            if (timedOutOrders.Count > 0)
                await ctx.SaveChangesAsync(ct);
        }
    }
}
