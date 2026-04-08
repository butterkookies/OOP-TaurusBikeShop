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
        /// Returns active, eligible vouchers for the given user:
        /// either global (no UserVoucher rows) or specifically assigned to them.
        /// Global and per-user usage caps are checked.
        /// </summary>
        Task<IEnumerable<POSVoucherSuggestion>> GetVoucherSuggestionsAsync(int userId);

        /// <summary>
        /// Validates a voucher code for a POS transaction.
        /// Checks: exists + active, date window, min order, global cap, per-user cap.
        /// </summary>
        Task<POSVoucherResult> ValidateVoucherAsync(string code, int userId, decimal subtotal);

        /// <summary>
        /// Atomic POS checkout: creates Order + OrderItems, deducts stock,
        /// logs InventoryLog entries, records Payment, and records VoucherUsage
        /// when a voucher is applied — all in one transaction.
        /// Throws InvalidOperationException when stock is insufficient.
        /// </summary>
        Task<POSOrderResult> CreatePOSSaleAsync(
            int    userId,
            int    cashierId,
            string customerName,
            List<POSCartItem> items,
            string  paymentMethod,
            decimal cashReceived,
            decimal discountAmount,
            string? voucherCode);
    }
}
