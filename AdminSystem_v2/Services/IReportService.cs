using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IReportService
    {
        Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to);
        Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to);
        Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top = 10);
        Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync();
    }
}
