using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IStorePaymentAccountRepository
    {
        Task<IEnumerable<StorePaymentAccount>> GetAllAsync();

        Task<int> InsertAsync(
            string  paymentMethod,
            string  accountName,
            string  accountNumber,
            string? bankName,
            string? qrImageUrl,
            string? instructions,
            bool    isActive,
            int     displayOrder);

        Task UpdateAsync(
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

        /// <summary>
        /// Sets <c>IsActive = false</c> on any currently-active row with the
        /// same <paramref name="paymentMethod"/>, except the row identified by
        /// <paramref name="excludeId"/>. Used to preserve the single-active-per-method
        /// invariant before activating another row.
        /// </summary>
        Task DeactivateOtherActiveAsync(string paymentMethod, int excludeId);
    }
}
