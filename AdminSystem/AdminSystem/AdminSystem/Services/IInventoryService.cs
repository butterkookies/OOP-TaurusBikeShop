using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IInventoryService
    {
        IEnumerable<InventoryLog> GetRecentLogs(int top = 500);
        IEnumerable<InventoryLog> GetLogsByProduct(int productId);
        IEnumerable<InventoryLog> GetLowStockVariants();
        void AdjustStock(int variantId, int quantity,
            string changeType, string notes = null);
        void ReceiveStock(int purchaseOrderId,
            System.Collections.Generic.IEnumerable<(int VariantId, int ReceivedQty)> items);
    }
}
