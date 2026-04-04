using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IReportRepository
    {
        /// <summary>Returns aggregated sales figures for the given date window.</summary>
        Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to);

        /// <summary>Returns one row per calendar day in the window, ordered newest first.</summary>
        Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to);

        /// <summary>Returns the top N products by revenue in the window.</summary>
        Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top = 10);

        /// <summary>Returns one row per active product variant, ordered by stock status urgency.</summary>
        Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync();
    }
}
