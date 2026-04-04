using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AdminSystem.Helpers;
using AdminSystem.Models;
using Dapper;

namespace AdminSystem.Services
{
    public class ReportService : IReportService
    {

        public decimal GetTotalSalesToday()
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.ExecuteScalar<decimal>(
                    @"SELECT ISNULL(SUM(p.Amount), 0)
                      FROM Payment p
                      WHERE p.PaymentStatus = @Status
                        AND CAST(p.CreatedAt AS DATE) = CAST(GETUTCDATE() AS DATE)",
                    new { Status = PaymentStatuses.Completed });
            }
        }

        public async Task<decimal> GetTotalSalesTodayAsync()
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return await conn.ExecuteScalarAsync<decimal>(
                    @"SELECT ISNULL(SUM(p.Amount), 0)
                      FROM Payment p
                      WHERE p.PaymentStatus = @Status
                        AND CAST(p.CreatedAt AS DATE) = CAST(GETUTCDATE() AS DATE)",
                    new { Status = PaymentStatuses.Completed });
            }
        }

        public decimal GetTotalSalesForPeriod(DateTime from, DateTime to)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.ExecuteScalar<decimal>(
                    @"SELECT ISNULL(SUM(p.Amount), 0)
                      FROM Payment p
                      WHERE p.PaymentStatus = @Status
                        AND p.CreatedAt >= @From
                        AND p.CreatedAt <= @To",
                    new { Status = PaymentStatuses.Completed, From = from, To = to });
            }
        }

        public int GetOrderCountByStatus(string status)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM [Order] WHERE OrderStatus = @Status",
                    new { Status = status });
            }
        }

        public IEnumerable<Order> GetTopOrdersByValue(int top)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                return conn.Query<Order>(
                    @"WITH OrderPayments AS (
                          SELECT OrderId, ISNULL(SUM(Amount), 0) AS TotalPaid
                          FROM Payment
                          WHERE PaymentStatus = @PaymentStatus
                          GROUP BY OrderId
                      )
                      SELECT TOP (@Top) o.*,
                             u.FirstName + ' ' + u.LastName AS CustomerName
                      FROM [Order] o
                      INNER JOIN [User] u ON o.UserId = u.UserId
                      LEFT  JOIN OrderPayments op ON o.OrderId = op.OrderId
                      WHERE o.OrderStatus = @OrderStatus
                      ORDER BY ISNULL(op.TotalPaid, 0) DESC",
                    new { Top = top,
                          OrderStatus   = OrderStatuses.Delivered,
                          PaymentStatus = PaymentStatuses.Completed });
            }
        }

        public IEnumerable<Product> GetLowStockProducts()
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                return conn.Query<Product>(
                    @"SELECT DISTINCT p.ProductId, p.CategoryId, p.BrandId,
                             p.Name, p.Description, p.Price,
                             p.IsActive, p.IsFeatured, p.CreatedAt, p.UpdatedAt,
                             c.Name AS CategoryName, b.BrandName
                      FROM Product p
                      INNER JOIN ProductVariant pv ON pv.ProductId = p.ProductId
                      LEFT  JOIN Category c ON p.CategoryId = c.CategoryId
                      LEFT  JOIN Brand    b ON p.BrandId    = b.BrandId
                      WHERE p.IsActive  = 1
                        AND pv.IsActive = 1
                        AND pv.StockQuantity <= pv.ReorderThreshold
                      ORDER BY p.Name");
            }
        }

        public IEnumerable<InventoryLog> GetInventoryMovements(
            DateTime from, DateTime to)
        {
            using (SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.Query<InventoryLog>(
                    @"SELECT il.*, p.Name AS ProductName, pv.VariantName
                      FROM InventoryLog il
                      INNER JOIN Product p ON il.ProductId = p.ProductId
                      LEFT  JOIN ProductVariant pv
                                 ON il.ProductVariantId = pv.ProductVariantId
                      WHERE il.CreatedAt >= @From
                        AND il.CreatedAt <= @To
                      ORDER BY il.CreatedAt DESC",
                    new { From = from, To = to });
            }
        }
    }
}
