using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo) => _repo = repo;

        public Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to)
            => _repo.GetSalesSummaryAsync(from, to);

        public Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to)
            => _repo.GetDailySalesAsync(from, to);

        public Task<IEnumerable<TopProduct>> GetTopProductsAsync(DateTime from, DateTime to, int top = 10)
            => _repo.GetTopProductsAsync(from, to, top);

        public Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync()
            => _repo.GetInventoryReportAsync();
    }
}
