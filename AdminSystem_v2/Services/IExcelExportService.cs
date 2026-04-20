using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public class SalesReportExport
    {
        public string SourceLabel    { get; set; } = string.Empty;
        public string PeriodLabel    { get; set; } = string.Empty;
        public DateTime FromDate     { get; set; }
        public DateTime ToDate       { get; set; }
        public SalesSummary? Summary { get; set; }
        public IEnumerable<DailySales> Breakdown   { get; set; } = Array.Empty<DailySales>();
        public IEnumerable<TopProduct> TopProducts { get; set; } = Array.Empty<TopProduct>();
    }

    public interface IExcelExportService
    {
        void ExportSalesReport(string filePath, SalesReportExport data);
    }
}
