using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo) => _repo = repo;

        public Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to, bool? isWalkIn)
            => _repo.GetSalesSummaryAsync(from, to, isWalkIn);

        public Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to, bool? isWalkIn)
            => _repo.GetDailySalesAsync(from, to, isWalkIn);

        public Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top, bool? isWalkIn)
            => _repo.GetTopProductsAsync(from, to, top, isWalkIn);

        public Task<IEnumerable<DailySales>> GetChartDataAsync(DateTime from, DateTime to, string groupBy, bool? isWalkIn)
            => _repo.GetChartDataAsync(from, to, groupBy, isWalkIn);

        public Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync()
            => _repo.GetInventoryReportAsync();
    }
}
