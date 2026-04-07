using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IPOSRepository
    {
        /// <summary>Fast product+variant lookup for the POS search box.</summary>
        Task<IEnumerable<POSProductItem>> SearchProductsAsync(string search);

        /// <summary>Customer lookup for the customer selector.</summary>
        Task<IEnumerable<POSCustomer>> SearchCustomersAsync(string search);

        /// <summary>Returns the walk-in placeholder user ID (IsWalkIn = 1).</summary>
        Task<int> GetWalkInUserIdAsync();

        /// <summary>
        /// Atomic POS checkout: creates Order + OrderItems, deducts stock,
        /// logs InventoryLog entries, and records Payment — all in one transaction.
        /// Throws InvalidOperationException when stock is insufficient.
        /// </summary>
        Task<POSOrderResult> CreatePOSSaleAsync(
            int    userId,
            int    cashierId,
            string customerName,
            List<POSCartItem> items,
            string  paymentMethod,
            decimal cashReceived,
            decimal discountAmount);
    }
}
