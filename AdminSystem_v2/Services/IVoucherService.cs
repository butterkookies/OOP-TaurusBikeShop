using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IVoucherService
    {
        Task<IEnumerable<VoucherListItem>> GetAllVouchersAsync();

        Task<(bool ok, string error)> CreateVoucherAsync(
            string   code,
            string   description,
            string   discountType,
            decimal  discountValue,
            decimal? minimumOrderAmount,
            int?     maxUses,
            int?     maxUsesPerUser,
            DateTime startDate,
            DateTime? endDate,
            bool     isActive);

        Task<(bool ok, string error)> UpdateVoucherAsync(
            int      voucherId,
            string   code,
            string   description,
            string   discountType,
            decimal  discountValue,
            decimal? minimumOrderAmount,
            int?     maxUses,
            int?     maxUsesPerUser,
            DateTime startDate,
            DateTime? endDate,
            bool     isActive);

        Task DeactivateVoucherAsync(int voucherId);
        Task ActivateVoucherAsync(int voucherId);

        Task<IEnumerable<VoucherUserRow>> SearchUsersAsync(string search);

        Task<(int assigned, string message)> AssignAndNotifyAsync(
            int                         voucherId,
            string                      voucherCode,
            string                      discountDisplay,
            string                      description,
            IEnumerable<VoucherUserRow> users,
            DateTime?                   expiresAt,
            bool                        sendInApp,
            bool                        sendEmail);
    }
}
