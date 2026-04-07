using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _repo;

        public VoucherService(IVoucherRepository repo) => _repo = repo;

        public Task<IEnumerable<VoucherListItem>> GetAllVouchersAsync()
            => _repo.GetAllAsync();

        public async Task<(bool ok, string error)> CreateVoucherAsync(
            string   code,
            string   description,
            string   discountType,
            decimal  discountValue,
            decimal? minimumOrderAmount,
            int?     maxUses,
            int?     maxUsesPerUser,
            DateTime startDate,
            DateTime? endDate,
            bool     isActive)
        {
            var (valid, validationError) = Validate(
                code, discountType, discountValue, startDate, endDate, 0);
            if (!valid) return (false, validationError);

            if (!await _repo.IsCodeUniqueAsync(code))
                return (false, $"Voucher code \"{code.Trim().ToUpperInvariant()}\" is already in use.");

            await _repo.InsertAsync(code, description, discountType, discountValue,
                minimumOrderAmount, maxUses, maxUsesPerUser, startDate, endDate, isActive);

            return (true, string.Empty);
        }

        public async Task<(bool ok, string error)> UpdateVoucherAsync(
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
            bool     isActive)
        {
            var (valid, validationError) = Validate(
                code, discountType, discountValue, startDate, endDate, voucherId);
            if (!valid) return (false, validationError);

            if (!await _repo.IsCodeUniqueAsync(code, voucherId))
                return (false, $"Voucher code \"{code.Trim().ToUpperInvariant()}\" is already in use.");

            await _repo.UpdateAsync(voucherId, code, description, discountType, discountValue,
                minimumOrderAmount, maxUses, maxUsesPerUser, startDate, endDate, isActive);

            return (true, string.Empty);
        }

        public Task DeactivateVoucherAsync(int voucherId) => _repo.SetActiveAsync(voucherId, false);
        public Task ActivateVoucherAsync(int voucherId)   => _repo.SetActiveAsync(voucherId, true);

        public Task<IEnumerable<VoucherUserRow>> SearchUsersAsync(string search)
            => _repo.SearchUsersAsync(search);

        public async Task<(int assigned, string message)> AssignAndNotifyAsync(
            int                         voucherId,
            string                      voucherCode,
            string                      discountDisplay,
            string                      description,
            IEnumerable<VoucherUserRow> users,
            DateTime?                   expiresAt,
            bool                        sendInApp,
            bool                        sendEmail)
        {
            var userList = users.ToList();
            if (userList.Count == 0) return (0, "No customers selected.");

            int assigned = await _repo.AssignAsync(voucherId, userList.Select(u => u.UserId), expiresAt);

            await _repo.QueueNotificationsAsync(
                userList, voucherCode, discountDisplay, description, sendInApp, sendEmail);

            string channelNote = (sendInApp, sendEmail) switch
            {
                (true, true)   => "Website notification + email queued.",
                (true, false)  => "Website notification queued.",
                (false, true)  => "Email queued.",
                _              => "No notifications sent."
            };

            return (assigned, $"Voucher assigned to {userList.Count} customer(s). {channelNote}");
        }

        // ── Validation ────────────────────────────────────────────────────────

        private static (bool ok, string error) Validate(
            string   code,
            string   discountType,
            decimal  discountValue,
            DateTime startDate,
            DateTime? endDate,
            int      excludeId)
        {
            if (string.IsNullOrWhiteSpace(code))
                return (false, "Voucher code is required.");

            if (code.Trim().Length > 50)
                return (false, "Voucher code cannot exceed 50 characters.");

            if (discountType != "Percentage" && discountType != "Fixed")
                return (false, "Discount type must be Percentage or Fixed.");

            if (discountValue <= 0)
                return (false, "Discount value must be greater than zero.");

            if (discountType == "Percentage" && discountValue > 100)
                return (false, "Percentage discount cannot exceed 100%.");

            if (endDate.HasValue && endDate.Value.Date < startDate.Date)
                return (false, "End date must be on or after start date.");

            return (true, string.Empty);
        }
    }
}
