using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class POSService : IPOSService
    {
        private readonly IPOSRepository _repo;

        public POSService(IPOSRepository repo) => _repo = repo;

        public Task<IEnumerable<POSProductItem>> SearchProductsAsync(string search)
            => _repo.SearchProductsAsync(search);

        public Task<IEnumerable<POSCustomer>> SearchCustomersAsync(string search)
            => _repo.SearchCustomersAsync(search);

        public Task<int> GetWalkInUserIdAsync()
            => _repo.GetWalkInUserIdAsync();

        public Task<IEnumerable<POSVoucherSuggestion>> GetVoucherSuggestionsAsync(int userId)
            => _repo.GetVoucherSuggestionsAsync(userId);

        public Task<POSVoucherResult> ValidateVoucherAsync(string code, int userId, decimal subtotal)
            => _repo.ValidateVoucherAsync(code, userId, subtotal);

        public Task<POSOrderResult> CreatePOSSaleAsync(
            int    userId,
            int    cashierId,
            int?   posSessionId,
            string customerName,
            List<POSCartItem> items,
            string  paymentMethod,
            decimal cashReceived,
            decimal discountAmount,
            string? voucherCode)
            => _repo.CreatePOSSaleAsync(
                userId, cashierId, posSessionId, customerName,
                items, paymentMethod, cashReceived, discountAmount, voucherCode);

        public Task<POSSession?> GetActiveSessionAsync(int cashierId)
            => _repo.GetActiveSessionAsync(cashierId);

        public Task<int> OpenSessionAsync(int cashierId, string terminalName)
            => _repo.OpenSessionAsync(cashierId, terminalName);

        public Task CloseSessionAsync(int sessionId)
            => _repo.CloseSessionAsync(sessionId);
    }
}
