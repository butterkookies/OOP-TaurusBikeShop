using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        public async Task<IEnumerable<LowStockVariant>> GetLowStockVariantsAsync()
        {
            await using var conn = DatabaseHelper.GetConnection();
            return await conn.QueryAsync<LowStockVariant>(
                @"SELECT pv.ProductVariantId,
                         pv.ProductId,
                         p.Name  AS ProductName,
                         pv.VariantName,
                         pv.StockQuantity,
                         pv.ReorderThreshold
                  FROM   ProductVariant pv
                  INNER JOIN Product p ON pv.ProductId = p.ProductId
                  WHERE  pv.IsActive = 1
                    AND  p.IsActive  = 1
                    AND  pv.StockQuantity <= pv.ReorderThreshold
                  ORDER BY pv.StockQuantity ASC");
        }
    }
}
