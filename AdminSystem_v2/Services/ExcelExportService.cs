using ClosedXML.Excel;

namespace AdminSystem_v2.Services
{
    public class ExcelExportService : IExcelExportService
    {
        private const string Currency = "₱#,##0.00";
        private static readonly XLColor HeaderFill = XLColor.FromHtml("#1F2937");
        private static readonly XLColor HeaderText = XLColor.FromHtml("#F9FAFB");
        private static readonly XLColor AccentFill = XLColor.FromHtml("#DC2626");

        public void ExportSalesReport(string filePath, SalesReportExport data)
        {
            using var wb = new XLWorkbook();

            BuildSummarySheet(wb, data);
            BuildBreakdownSheet(wb, data);
            BuildTopProductsSheet(wb, data);

            wb.SaveAs(filePath);
        }

        private static void BuildSummarySheet(XLWorkbook wb, SalesReportExport d)
        {
            var ws = wb.Worksheets.Add("Summary");

            ws.Cell("A1").Value = "Sales Report";
            ws.Range("A1:B1").Merge();
            ws.Cell("A1").Style.Font.Bold = true;
            ws.Cell("A1").Style.Font.FontSize = 16;
            ws.Cell("A1").Style.Fill.BackgroundColor = AccentFill;
            ws.Cell("A1").Style.Font.FontColor = HeaderText;
            ws.Cell("A1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Row(1).Height = 24;

            var meta = new (string Label, string Value)[]
            {
                ("Source",       d.SourceLabel),
                ("Period",       d.PeriodLabel),
                ("Date Range",   $"{d.FromDate:MMM d, yyyy} — {d.ToDate:MMM d, yyyy}"),
                ("Generated",    DateTime.Now.ToString("MMM d, yyyy h:mm tt")),
            };
            for (int i = 0; i < meta.Length; i++)
            {
                int r = 3 + i;
                ws.Cell(r, 1).Value = meta[i].Label;
                ws.Cell(r, 2).Value = meta[i].Value;
                ws.Cell(r, 1).Style.Font.Bold = true;
                ws.Cell(r, 1).Style.Font.FontColor = XLColor.FromHtml("#6B7280");
            }

            int start = 3 + meta.Length + 1;
            ws.Cell(start, 1).Value = "Metric";
            ws.Cell(start, 2).Value = "Value";
            var header = ws.Range(start, 1, start, 2);
            header.Style.Fill.BackgroundColor = HeaderFill;
            header.Style.Font.FontColor       = HeaderText;
            header.Style.Font.Bold            = true;

            var s = d.Summary;
            var rows = new (string Label, object? Value, string? Format)[]
            {
                ("Total Orders",     s?.TotalOrders    ?? 0,   null),
                ("Total Revenue",    s?.TotalRevenue   ?? 0m,  Currency),
                ("Avg Order Value",  s?.AvgOrderValue  ?? 0m,  Currency),
                ("Gross Revenue",    s?.GrossRevenue   ?? 0m,  Currency),
                ("Total Discounts",  s?.TotalDiscounts ?? 0m,  Currency),
                ("Total Shipping",   s?.TotalShipping  ?? 0m,  Currency),
            };
            for (int i = 0; i < rows.Length; i++)
            {
                int r = start + 1 + i;
                ws.Cell(r, 1).Value = rows[i].Label;
                ws.Cell(r, 2).Value = XLCellValue.FromObject(rows[i].Value);
                if (rows[i].Format != null)
                    ws.Cell(r, 2).Style.NumberFormat.Format = rows[i].Format;
            }

            ws.Columns(1, 2).AdjustToContents();
            ws.Column(1).Width = Math.Max(ws.Column(1).Width, 20);
            ws.Column(2).Width = Math.Max(ws.Column(2).Width, 28);
        }

        private static void BuildBreakdownSheet(XLWorkbook wb, SalesReportExport d)
        {
            var ws = wb.Worksheets.Add("Breakdown");

            ws.Cell(1, 1).Value = "Period";
            ws.Cell(1, 2).Value = "Orders";
            ws.Cell(1, 3).Value = "Revenue";
            StyleHeader(ws.Range(1, 1, 1, 3));

            int row = 2;
            foreach (var item in d.Breakdown)
            {
                ws.Cell(row, 1).Value = item.SaleDateDisplay;
                ws.Cell(row, 2).Value = item.OrderCount;
                ws.Cell(row, 3).Value = item.Revenue;
                ws.Cell(row, 3).Style.NumberFormat.Format = Currency;
                row++;
            }

            if (row > 2)
            {
                ws.Cell(row, 1).Value = "Total";
                ws.Cell(row, 2).FormulaA1 = $"SUM(B2:B{row - 1})";
                ws.Cell(row, 3).FormulaA1 = $"SUM(C2:C{row - 1})";
                ws.Cell(row, 3).Style.NumberFormat.Format = Currency;
                ws.Range(row, 1, row, 3).Style.Font.Bold = true;
                ws.Range(row, 1, row, 3).Style.Border.TopBorder = XLBorderStyleValues.Thin;
            }

            ws.Columns(1, 3).AdjustToContents();
            ws.SheetView.FreezeRows(1);
        }

        private static void BuildTopProductsSheet(XLWorkbook wb, SalesReportExport d)
        {
            var ws = wb.Worksheets.Add("Top Products");

            ws.Cell(1, 1).Value = "Rank";
            ws.Cell(1, 2).Value = "Product";
            ws.Cell(1, 3).Value = "Units Sold";
            ws.Cell(1, 4).Value = "Revenue";
            StyleHeader(ws.Range(1, 1, 1, 4));

            int row = 2;
            int rank = 1;
            foreach (var p in d.TopProducts)
            {
                ws.Cell(row, 1).Value = rank++;
                ws.Cell(row, 2).Value = p.ProductName;
                ws.Cell(row, 3).Value = p.UnitsSold;
                ws.Cell(row, 4).Value = p.Revenue;
                ws.Cell(row, 4).Style.NumberFormat.Format = Currency;
                row++;
            }

            ws.Columns(1, 4).AdjustToContents();
            ws.Column(2).Width = Math.Max(ws.Column(2).Width, 32);
            ws.SheetView.FreezeRows(1);
        }

        private static void StyleHeader(IXLRange range)
        {
            range.Style.Fill.BackgroundColor = HeaderFill;
            range.Style.Font.FontColor       = HeaderText;
            range.Style.Font.Bold            = true;
            range.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
        }
    }
}
