using System.Collections.Generic;
using AdminSystem.Models;
using Dapper;
using System.Data.SqlClient;

namespace AdminSystem.Repositories
{
    public class InventoryRepository : Repository<InventoryLog>, IRepository<InventoryLog>
    {
        public InventoryLog GetById(int id)
            => QueryFirstOrDefault(
                @"SELECT il.*, p.Name AS ProductName, pv.VariantName
                  FROM InventoryLog il
                  INNER JOIN Product p ON il.ProductId=p.ProductId
                  LEFT JOIN ProductVariant pv ON il.ProductVariantId=pv.ProductVariantId
                  WHERE il.LogId=@Id", new { Id = id });

        public IEnumerable<InventoryLog> GetAll()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<InventoryLog>(
                    @"SELECT TOP 500 il.*, p.Name AS ProductName, pv.VariantName
                      FROM InventoryLog il
                      INNER JOIN Product p ON il.ProductId=p.ProductId
                      LEFT JOIN ProductVariant pv ON il.ProductVariantId=pv.ProductVariantId
                      ORDER BY il.CreatedAt DESC");
        }

        public IEnumerable<InventoryLog> GetLowStockVariants()
        {
            using (SqlConnection conn = GetConnection())
                return conn.Query<InventoryLog>(
                    @"SELECT pv.ProductVariantId AS LogId, pv.ProductId,
                             pv.VariantName, p.Name AS ProductName,
                             pv.StockQuantity AS ChangeQuantity,
                             pv.ReorderThreshold
                      FROM ProductVariant pv
                      INNER JOIN Product p ON pv.ProductId=p.ProductId
                      WHERE pv.IsActive=1 AND p.IsActive=1
                        AND pv.StockQuantity <= pv.ReorderThreshold
                      ORDER BY pv.StockQuantity ASC");
        }

        public int Insert(InventoryLog entity)
        {
            throw new System.NotSupportedException(
                "InventoryLog is append-only. Use AdjustStock or ReceiveStock.");
        }

        public void Update(InventoryLog entity)
        {
            throw new System.NotSupportedException(
                "InventoryLog rows are immutable.");
        }

        public void Delete(int id)
        {
            throw new System.NotSupportedException(
                "InventoryLog rows are immutable.");
        }
    }
}
