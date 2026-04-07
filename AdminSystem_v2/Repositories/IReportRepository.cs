using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IReportRepository
    {
        /// <summary>
        /// Aggregated sales figures. isWalkIn: null = combined, false = online, true = walk-in.
        /// </summary>
        Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to, bool? isWalkIn);

        /// <summary>
        /// One row per calendar day in the window, ordered oldest → newest.
        /// </summary>
        Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to, bool? isWalkIn);

        /// <summary>
        /// Top N products by revenue in the window.
        /// </summary>
        Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top, bool? isWalkIn);

        /// <summary>
        /// Chart data grouped by Day / Week / Month / Year, ordered oldest → newest.
        /// groupBy must be one of: "Day", "Week", "Month", "Year".
        /// </summary>
        Task<IEnumerable<DailySales>> GetChartDataAsync(DateTime from, DateTime to, string groupBy, bool? isWalkIn);

        /// <summary>One row per active product variant, ordered by urgency.</summary>
        Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync();
    }
}
