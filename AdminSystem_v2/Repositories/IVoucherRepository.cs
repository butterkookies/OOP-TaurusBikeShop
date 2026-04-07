using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IVoucherRepository
    {
        Task<IEnumerable<VoucherListItem>> GetAllAsync();

        Task<int> InsertAsync(
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

        Task UpdateAsync(
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

        Task SetActiveAsync(int voucherId, bool isActive);

        Task<bool> IsCodeUniqueAsync(string code, int excludeVoucherId = 0);

        /// <summary>Search registered online customers by name or email.</summary>
        Task<IEnumerable<VoucherUserRow>> SearchUsersAsync(string search);

        /// <summary>
        /// Assigns the voucher to each userId (skips if already assigned).
        /// Returns the number of new assignments made.
        /// </summary>
        Task<int> AssignAsync(int voucherId, IEnumerable<int> userIds, DateTime? expiresAt);

        /// <summary>Queues one Notification row per (userId, channel) pair.</summary>
        Task QueueNotificationsAsync(
            IEnumerable<VoucherUserRow> users,
            string   voucherCode,
            string   discountDisplay,
            string   description,
            bool     sendInApp,
            bool     sendEmail);
    }
}
