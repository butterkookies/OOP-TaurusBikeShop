using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;
using Dapper;

namespace AdminSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly InventoryRepository _inventoryRepo;
        private readonly ProductRepository   _productRepo;

        public ReportService(
            InventoryRepository inventoryRepo,
            ProductRepository   productRepo)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo   = productRepo;
        }

        public decimal GetTotalSalesToday()
        {
            using (System.Data.SqlClient.SqlConnection conn =
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
            using (System.Data.SqlClient.SqlConnection conn =
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
            using (System.Data.SqlClient.SqlConnection conn =
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
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.ExecuteScalar<int>(
                    "SELECT COUNT(*) FROM [Order] WHERE OrderStatus = @Status",
                    new { Status = status });
            }
        }

        public IEnumerable<Order> GetTopOrdersByValue(int top)
        {
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.Query<Order>(
                    @"SELECT TOP (@Top) o.*,
                             u.FirstName + ' ' + u.LastName AS CustomerName
                      FROM [Order] o
                      INNER JOIN [User] u ON o.UserId = u.UserId
                      WHERE o.OrderStatus = @OrderStatus
                      ORDER BY
                        (SELECT ISNULL(SUM(Amount),0)
                         FROM Payment
                         WHERE OrderId = o.OrderId
                           AND PaymentStatus = @PaymentStatus) DESC",
                    new { Top = top,
                          OrderStatus   = OrderStatuses.Delivered,
                          PaymentStatus = PaymentStatuses.Completed });
            }
        }

        public IEnumerable<Product> GetLowStockProducts()
        {
            IEnumerable<InventoryLog> lowVariants =
                _inventoryRepo.GetLowStockVariants();
            System.Collections.Generic.List<Product> result =
                new System.Collections.Generic.List<Product>();
            System.Collections.Generic.HashSet<int> seen =
                new System.Collections.Generic.HashSet<int>();

            foreach (InventoryLog log in lowVariants)
            {
                if (seen.Contains(log.ProductId)) continue;
                seen.Add(log.ProductId);
                Product p = _productRepo.GetById(log.ProductId);
                if (p != null) result.Add(p);
            }
            return result;
        }

        public IEnumerable<InventoryLog> GetInventoryMovements(
            DateTime from, DateTime to)
        {
            using (System.Data.SqlClient.SqlConnection conn =
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
