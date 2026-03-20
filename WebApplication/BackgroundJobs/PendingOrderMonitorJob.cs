using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;

namespace WebApplication.BackgroundJobs
{
    public class PendingOrderMonitorJob : BackgroundService
    {
        static readonly TimeSpan Interval = TimeSpan.FromMinutes(5);

        readonly IServiceScopeFactory _scope;
        readonly ILogger<PendingOrderMonitorJob> _logger;

        public PendingOrderMonitorJob(IServiceScopeFactory scope, ILogger<PendingOrderMonitorJob> logger)
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
                    _logger.LogError(ex, "PendingOrderMonitorJob encountered an error.");
                }
                await Task.Delay(Interval, ct);
            }
        }

        async Task RunAsync(CancellationToken ct)
        {
            await using var scope = _scope.CreateAsyncScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var cutoff = DateTime.UtcNow.AddHours(-24);

            var staleOrders = await ctx.Orders
                .Where(o => o.OrderStatus == "Pending" && o.CreatedAt < cutoff)
                .ToListAsync(ct);

            foreach (var order in staleOrders)
            {
                _logger.LogWarning("Order {OrderNumber} has been pending for over 24 hours.", order.OrderNumber);
                order.OrderStatus = "OnHold";
            }

            if (staleOrders.Count > 0)
                await ctx.SaveChangesAsync(ct);
        }
    }
}
