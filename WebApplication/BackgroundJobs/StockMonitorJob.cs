using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;

namespace WebApplication.BackgroundJobs
{
    public class StockMonitorJob : BackgroundService
    {
        static readonly TimeSpan Interval = TimeSpan.FromMinutes(15);

        readonly IServiceScopeFactory _scope;
        readonly ILogger<StockMonitorJob> _logger;

        public StockMonitorJob(IServiceScopeFactory scope, ILogger<StockMonitorJob> logger)
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
                    _logger.LogError(ex, "StockMonitorJob encountered an error.");
                }
                await Task.Delay(Interval, ct);
            }
        }

        async Task RunAsync(CancellationToken ct)
        {
            await using var scope = _scope.CreateAsyncScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var lowStockProducts = await ctx.Products
                .Where(p => p.IsActive && p.StockQuantity <= 5)
                .ToListAsync(ct);

            foreach (var product in lowStockProducts)
            {
                _logger.LogWarning(
                    "Low stock alert: {ProductName} has {StockQuantity} units remaining.",
                    product.Name,
                    product.StockQuantity);
            }
        }
    }
}
