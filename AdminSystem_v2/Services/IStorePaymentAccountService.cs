using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IStorePaymentAccountService
    {
        Task<IEnumerable<StorePaymentAccount>> GetAllAsync();

        Task<(bool ok, string error)> CreateAsync(
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder);

        Task<(bool ok, string error)> UpdateAsync(
            int     storePaymentAccountId,
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder);

        Task DeleteAsync(int storePaymentAccountId);
    }
}
