using System.Collections.Generic;
using System.Threading.Tasks;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IInventoryService
    {
        IEnumerable<InventoryLog>  GetRecentLogs();
        IEnumerable<InventoryLog>  GetLogsByProduct(int productId);
        IEnumerable<LowStockVariant>  GetLowStockVariants();
        Task<List<LowStockVariant>>   GetLowStockVariantsAsync();
        void AdjustStock(int variantId, int quantity,
            string changeType, string notes = null);
        void ReceiveStock(int purchaseOrderId,
            System.Collections.Generic.IEnumerable<(int VariantId, int ReceivedQty)> items);
    }
}
