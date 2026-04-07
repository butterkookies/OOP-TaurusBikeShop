using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IReportService
    {
        Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to, bool? isWalkIn);
        Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to, bool? isWalkIn);
        Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top, bool? isWalkIn);
        Task<IEnumerable<DailySales>> GetChartDataAsync(DateTime from, DateTime to, string groupBy, bool? isWalkIn);
        Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync();
    }
}
