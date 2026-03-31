using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;
using Dapper;

namespace AdminSystem.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryRepository _inventoryRepo;
        private readonly ProductRepository   _productRepo;

        public InventoryService(
            InventoryRepository inventoryRepo,
            ProductRepository   productRepo)
        {
            _inventoryRepo = inventoryRepo;
            _productRepo   = productRepo;
        }

        public IEnumerable<InventoryLog> GetRecentLogs(int top = 500)
            => _inventoryRepo.GetAll();

        public IEnumerable<InventoryLog> GetLogsByProduct(int productId)
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
                      WHERE il.ProductId = @ProductId
                      ORDER BY il.CreatedAt DESC",
                    new { ProductId = productId });
            }
        }

        public IEnumerable<InventoryLog> GetLowStockVariants()
            => _inventoryRepo.GetLowStockVariants();

        public Task<List<InventoryLog>> GetLowStockVariantsAsync()
            => Task.Run(() => _inventoryRepo.GetLowStockVariants().ToList());

        public void AdjustStock(int variantId, int quantity,
            string changeType, string notes = null)
        {
            if (quantity == 0)
                throw new ArgumentException(
                    "Adjustment quantity cannot be zero.");

            // Validate change type
            switch (changeType)
            {
                case InventoryChangeTypes.Adjustment:
                case InventoryChangeTypes.Damage:
                case InventoryChangeTypes.Loss:
                case InventoryChangeTypes.Return:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid change type for manual adjustment: " + changeType);
            }

            int userId = App.CurrentUser != null ? App.CurrentUser.UserId : 0;
            _productRepo.AdjustStock(variantId, quantity, changeType, userId, notes);
        }

        public void ReceiveStock(int purchaseOrderId,
            IEnumerable<(int VariantId, int ReceivedQty)> items)
        {
            if (purchaseOrderId <= 0)
                throw new ArgumentException("Invalid purchase order ID.");

            int userId = App.CurrentUser != null ? App.CurrentUser.UserId : 0;

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            using (System.Data.IDbTransaction tx = conn.BeginTransaction())
            {
                try
                {
                    foreach ((int variantId, int receivedQty) in items)
                    {
                        if (receivedQty <= 0) continue;

                        // Update stock quantity — stock lives in ProductVariant.StockQuantity only
                        conn.Execute(
                            @"UPDATE ProductVariant
                              SET StockQuantity = StockQuantity + @Qty,
                                  UpdatedAt     = GETUTCDATE()
                              WHERE ProductVariantId = @VariantId",
                            new { Qty = receivedQty, VariantId = variantId }, tx);

                        int productId = conn.ExecuteScalar<int>(
                            "SELECT ProductId FROM ProductVariant WHERE ProductVariantId = @Id",
                            new { Id = variantId }, tx);

                        // InventoryLog is append-only — never update or delete rows
                        conn.Execute(
                            @"INSERT INTO InventoryLog
                                (ProductId, ProductVariantId, ChangeQuantity,
                                 ChangeType, ChangedByUserId, Notes, CreatedAt)
                              VALUES
                                (@ProductId, @VariantId, @Qty,
                                 @ChangeType, @UserId, @Notes, GETUTCDATE())",
                            new { ProductId  = productId,
                                  VariantId  = variantId,
                                  Qty        = receivedQty,
                                  ChangeType = InventoryChangeTypes.Purchase,
                                  UserId     = userId,
                                  Notes      = "Received via PO #" + purchaseOrderId },
                            tx);
                    }

                    // Mark purchase order items as received
                    conn.Execute(
                        @"UPDATE PurchaseOrderItem
                          SET ReceivedQuantity = OrderedQuantity
                          WHERE PurchaseOrderId = @PoId",
                        new { PoId = purchaseOrderId }, tx);

                    conn.Execute(
                        @"UPDATE PurchaseOrder
                          SET Status    = @Status,
                              UpdatedAt = GETUTCDATE()
                          WHERE PurchaseOrderId = @PoId",
                        new { Status = PurchaseOrderStatuses.Received, PoId = purchaseOrderId }, tx);

                    tx.Commit();
                }
                catch { tx.Rollback(); throw; }
            }
        }
    }
}
