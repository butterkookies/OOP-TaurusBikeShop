using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class ReportRepository : Repository<DailySales>, IReportRepository
    {
        private static readonly string[] ExcludedStatuses = { OrderStatuses.Cancelled, OrderStatuses.Pending };

        // ── Source filter helper ──────────────────────────────────────────────
        // Returns an empty string (no filter), "AND IsWalkIn = 0", or "AND IsWalkIn = 1"
        // isWalkIn is always from our own code — never user input, so string interpolation is safe.
        private static string WalkInClause(bool? isWalkIn) => isWalkIn switch
        {
            false => "AND IsWalkIn = 0",
            true  => "AND IsWalkIn = 1",
            _     => string.Empty
        };

        // ── Sales summary ─────────────────────────────────────────────────────

        public async Task<SalesSummary> GetSalesSummaryAsync(DateTime from, DateTime to, bool? isWalkIn)
        {
            await using var conn = GetConnection();

            var row = await conn.QueryFirstOrDefaultAsync(
                $@"SELECT
                    COUNT(DISTINCT OrderId)       AS TotalOrders,
                    ISNULL(SUM(TotalAmount),  0)  AS TotalRevenue,
                    ISNULL(AVG(TotalAmount),  0)  AS AvgOrderValue,
                    ISNULL(SUM(SubTotal),     0)  AS GrossRevenue,
                    ISNULL(SUM(DiscountAmount),0) AS TotalDiscounts,
                    ISNULL(SUM(ShippingFee),  0)  AS TotalShipping
                  FROM vw_OrderSummary
                  WHERE OrderStatus NOT IN @Excluded
                    {WalkInClause(isWalkIn)}
                    AND OrderDate >= @From
                    AND OrderDate  < @To",
                new { Excluded = ExcludedStatuses, From = from, To = to });

            if (row == null) return new SalesSummary();

            return new SalesSummary
            {
                TotalOrders    = (int)row.TotalOrders,
                TotalRevenue   = (decimal)row.TotalRevenue,
                AvgOrderValue  = (decimal)row.AvgOrderValue,
                GrossRevenue   = (decimal)row.GrossRevenue,
                TotalDiscounts = (decimal)row.TotalDiscounts,
                TotalShipping  = (decimal)row.TotalShipping,
            };
        }

        // ── Daily breakdown ───────────────────────────────────────────────────

        public async Task<IEnumerable<DailySales>> GetDailySalesAsync(DateTime from, DateTime to, bool? isWalkIn)
            => await QueryAsync<DailySales>(
                $@"SELECT
                    CAST(OrderDate AS DATE)         AS SaleDate,
                    COUNT(OrderId)                  AS OrderCount,
                    ISNULL(SUM(TotalAmount), 0)     AS Revenue
                  FROM vw_OrderSummary
                  WHERE OrderStatus NOT IN @Excluded
                    {WalkInClause(isWalkIn)}
                    AND OrderDate >= @From
                    AND OrderDate  < @To
                  GROUP BY CAST(OrderDate AS DATE)
                  ORDER BY SaleDate DESC",
                new { Excluded = ExcludedStatuses, From = from, To = to });

        // ── Top products ──────────────────────────────────────────────────────

        public async Task<IEnumerable<TopProduct>> GetTopProductsAsync(
            DateTime from, DateTime to, int top, bool? isWalkIn)
            => await QueryAsync<TopProduct>(
                $@"SELECT TOP (@Top)
                    oi.ProductName,
                    SUM(oi.Quantity)  AS UnitsSold,
                    SUM(oi.Subtotal)  AS Revenue
                  FROM vw_OrderItemDetail oi
                  INNER JOIN [Order] o ON oi.OrderId = o.OrderId
                  WHERE o.OrderStatus NOT IN @Excluded
                    {(isWalkIn.HasValue ? $"AND o.IsWalkIn = {(isWalkIn.Value ? 1 : 0)}" : "")}
                    AND o.OrderDate >= @From
                    AND o.OrderDate  < @To
                  GROUP BY oi.ProductName
                  ORDER BY Revenue DESC",
                new { Top = top, Excluded = ExcludedStatuses, From = from, To = to });

        // ── Chart data ────────────────────────────────────────────────────────

        public async Task<IEnumerable<DailySales>> GetChartDataAsync(
            DateTime from, DateTime to, string groupBy, bool? isWalkIn)
        {
            var (dateExpr, groupExpr) = groupBy switch
            {
                "Day"   => ("CAST(OrderDate AS DATE)",
                            "CAST(OrderDate AS DATE)"),
                "Week"  => ("DATEADD(WEEK, DATEDIFF(WEEK, 0, OrderDate), 0)",
                            "DATEDIFF(WEEK, 0, OrderDate)"),
                "Month" => ("DATEFROMPARTS(YEAR(OrderDate), MONTH(OrderDate), 1)",
                            "YEAR(OrderDate), MONTH(OrderDate)"),
                "Year"  => ("DATEFROMPARTS(YEAR(OrderDate), 1, 1)",
                            "YEAR(OrderDate)"),
                _ => throw new ArgumentException($"Unknown groupBy: {groupBy}")
            };

            return await QueryAsync<DailySales>(
                $@"SELECT
                    {dateExpr}                     AS SaleDate,
                    COUNT(OrderId)                 AS OrderCount,
                    ISNULL(SUM(TotalAmount), 0)    AS Revenue
                  FROM vw_OrderSummary
                  WHERE OrderStatus NOT IN @Excluded
                    {WalkInClause(isWalkIn)}
                    AND OrderDate >= @From
                    AND OrderDate  < @To
                  GROUP BY {groupExpr}
                  ORDER BY SaleDate ASC",
                new { Excluded = ExcludedStatuses, From = from, To = to });
        }

        // ── Inventory report ──────────────────────────────────────────────────

        public async Task<IEnumerable<InventoryReportItem>> GetInventoryReportAsync()
            => await QueryAsync<InventoryReportItem>(
                @"SELECT
                    p.ProductId,
                    p.[Name]           AS ProductName,
                    c.[Name]           AS CategoryName,
                    b.BrandName,
                    pv.VariantName,
                    pv.SKU,
                    pv.StockQuantity,
                    pv.ReorderThreshold,
                    p.Price,
                    (pv.StockQuantity * p.Price) AS StockValue,
                    CASE
                        WHEN pv.StockQuantity  = 0                       THEN 'OutOfStock'
                        WHEN pv.StockQuantity <= pv.ReorderThreshold      THEN 'LowStock'
                        ELSE 'InStock'
                    END AS StockStatus
                  FROM ProductVariant pv
                  INNER JOIN Product   p ON pv.ProductId  = p.ProductId
                  INNER JOIN Category  c ON p.CategoryId  = c.CategoryId
                  LEFT  JOIN Brand     b ON p.BrandId     = b.BrandId
                  WHERE p.IsActive = 1 AND pv.IsActive = 1
                  ORDER BY
                    CASE
                        WHEN pv.StockQuantity  = 0                       THEN 0
                        WHEN pv.StockQuantity <= pv.ReorderThreshold      THEN 1
                        ELSE 2
                    END,
                    pv.StockQuantity ASC");
    }
}
