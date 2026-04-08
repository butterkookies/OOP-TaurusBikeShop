using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IPOSService
    {
        Task<IEnumerable<POSProductItem>> SearchProductsAsync(string search);
        Task<IEnumerable<POSCustomer>>    SearchCustomersAsync(string search);
        Task<int>                         GetWalkInUserIdAsync();
        Task<IEnumerable<POSVoucherSuggestion>> GetVoucherSuggestionsAsync(int userId);
        Task<POSVoucherResult>               ValidateVoucherAsync(string code, int userId, decimal subtotal);

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
