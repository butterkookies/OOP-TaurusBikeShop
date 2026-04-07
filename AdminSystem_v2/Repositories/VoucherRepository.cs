using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class VoucherRepository : Repository<VoucherListItem>, IVoucherRepository
    {
        // ── List ──────────────────────────────────────────────────────────────

        public async Task<IEnumerable<VoucherListItem>> GetAllAsync()
            => await QueryAsync<VoucherListItem>(
                @"SELECT
                    v.VoucherId,
                    v.Code,
                    ISNULL(v.Description, '')   AS Description,
                    v.DiscountType,
                    v.DiscountValue,
                    v.MinimumOrderAmount,
                    v.MaxUses,
                    v.MaxUsesPerUser,
                    v.StartDate,
                    v.EndDate,
                    v.IsActive,
                    v.CreatedAt,
                    ISNULL(vu.TimesUsed,    0)  AS TimesUsed,
                    ISNULL(uv.AssignedCount, 0) AS AssignedCount
                  FROM Voucher v
                  LEFT JOIN (
                      SELECT VoucherId, COUNT(*) AS TimesUsed
                      FROM   VoucherUsage
                      GROUP  BY VoucherId
                  ) vu ON vu.VoucherId = v.VoucherId
                  LEFT JOIN (
                      SELECT VoucherId, COUNT(*) AS AssignedCount
                      FROM   UserVoucher
                      GROUP  BY VoucherId
                  ) uv ON uv.VoucherId = v.VoucherId
                  ORDER BY v.CreatedAt DESC");

        // ── Insert ────────────────────────────────────────────────────────────

        public async Task<int> InsertAsync(
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
            await using var conn = GetConnection();
            return await conn.ExecuteScalarAsync<int>(
                @"INSERT INTO Voucher
                      (Code, Description, DiscountType, DiscountValue,
                       MinimumOrderAmount, MaxUses, MaxUsesPerUser,
                       StartDate, EndDate, IsActive)
                  VALUES
                      (@Code, @Description, @DiscountType, @DiscountValue,
                       @MinimumOrderAmount, @MaxUses, @MaxUsesPerUser,
                       @StartDate, @EndDate, @IsActive);
                  SELECT CAST(SCOPE_IDENTITY() AS INT);",
                new
                {
                    Code               = code.Trim().ToUpperInvariant(),
                    Description        = description.Trim(),
                    DiscountType       = discountType,
                    DiscountValue      = discountValue,
                    MinimumOrderAmount = minimumOrderAmount,
                    MaxUses            = maxUses,
                    MaxUsesPerUser     = maxUsesPerUser,
                    StartDate          = startDate,
                    EndDate            = endDate,
                    IsActive           = isActive
                });
        }

        // ── Update ────────────────────────────────────────────────────────────

        public async Task UpdateAsync(
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
            await using var conn = GetConnection();
            await conn.ExecuteAsync(
                @"UPDATE Voucher SET
                    Code               = @Code,
                    Description        = @Description,
                    DiscountType       = @DiscountType,
                    DiscountValue      = @DiscountValue,
                    MinimumOrderAmount = @MinimumOrderAmount,
                    MaxUses            = @MaxUses,
                    MaxUsesPerUser     = @MaxUsesPerUser,
                    StartDate          = @StartDate,
                    EndDate            = @EndDate,
                    IsActive           = @IsActive
                  WHERE VoucherId = @VoucherId",
                new
                {
                    VoucherId          = voucherId,
                    Code               = code.Trim().ToUpperInvariant(),
                    Description        = description.Trim(),
                    DiscountType       = discountType,
                    DiscountValue      = discountValue,
                    MinimumOrderAmount = minimumOrderAmount,
                    MaxUses            = maxUses,
                    MaxUsesPerUser     = maxUsesPerUser,
                    StartDate          = startDate,
                    EndDate            = endDate,
                    IsActive           = isActive
                });
        }

        // ── Toggle active ─────────────────────────────────────────────────────

        public async Task SetActiveAsync(int voucherId, bool isActive)
        {
            await using var conn = GetConnection();
            await conn.ExecuteAsync(
                "UPDATE Voucher SET IsActive = @IsActive WHERE VoucherId = @VoucherId",
                new { VoucherId = voucherId, IsActive = isActive });
        }

        // ── Code uniqueness check ─────────────────────────────────────────────

        public async Task<bool> IsCodeUniqueAsync(string code, int excludeVoucherId = 0)
        {
            await using var conn = GetConnection();
            int count = await conn.ExecuteScalarAsync<int>(
                @"SELECT COUNT(1) FROM Voucher
                  WHERE Code = @Code AND VoucherId <> @Exclude",
                new { Code = code.Trim().ToUpperInvariant(), Exclude = excludeVoucherId });
            return count == 0;
        }

        // ── User search ───────────────────────────────────────────────────────

        public async Task<IEnumerable<VoucherUserRow>> SearchUsersAsync(string search)
            => await QueryAsync<VoucherUserRow>(
                @"SELECT TOP 25
                    UserId,
                    FirstName + ' ' + LastName  AS Name,
                    ISNULL(Email, '')            AS Email,
                    IsWalkIn
                  FROM [User]
                  WHERE IsActive = 1
                    AND IsWalkIn = 0
                    AND (
                        FirstName + ' ' + LastName LIKE @Search
                        OR Email                   LIKE @Search
                    )
                  ORDER BY FirstName, LastName",
                new { Search = $"%{search}%" });

        // ── Assign ────────────────────────────────────────────────────────────

        public async Task<int> AssignAsync(
            int              voucherId,
            IEnumerable<int> userIds,
            DateTime?        expiresAt)
        {
            await using var conn = GetConnection();
            int assigned = 0;
            foreach (int uid in userIds)
            {
                int rows = await conn.ExecuteAsync(
                    @"INSERT INTO UserVoucher (UserId, VoucherId, AssignedAt, ExpiresAt)
                      SELECT @UserId, @VoucherId, GETUTCDATE(), @ExpiresAt
                      WHERE  NOT EXISTS (
                          SELECT 1 FROM UserVoucher
                          WHERE UserId = @UserId AND VoucherId = @VoucherId
                      )",
                    new { UserId = uid, VoucherId = voucherId, ExpiresAt = expiresAt });
                assigned += rows;
            }
            return assigned;
        }

        // ── Queue notifications ───────────────────────────────────────────────

        public async Task QueueNotificationsAsync(
            IEnumerable<VoucherUserRow> users,
            string voucherCode,
            string discountDisplay,
            string description,
            bool   sendInApp,
            bool   sendEmail)
        {
            if (!sendInApp && !sendEmail) return;

            string subject = $"You have a new voucher — {voucherCode}";
            string desc    = string.IsNullOrWhiteSpace(description) ? "" : $"\n\n{description}";

            await using var conn = GetConnection();

            foreach (var user in users)
            {
                string body =
                    $"Hi {user.Name},\n\n" +
                    $"You have been assigned a voucher from Taurus Bike Shop!\n\n" +
                    $"Code:     {voucherCode}\n" +
                    $"Discount: {discountDisplay}" +
                    $"{desc}\n\n" +
                    $"Use the code on your next online order or present it at our store.";

                if (sendInApp)
                {
                    await conn.ExecuteAsync(
                        @"INSERT INTO Notification
                              (UserId, Channel, NotifType, Recipient, Subject, Body)
                          VALUES
                              (@UserId, 'InApp', 'VoucherAssigned', @Recipient, @Subject, @Body)",
                        new
                        {
                            UserId    = user.UserId,
                            Recipient = user.Email,
                            Subject   = subject,
                            Body      = body
                        });
                }

                if (sendEmail && !string.IsNullOrWhiteSpace(user.Email))
                {
                    await conn.ExecuteAsync(
                        @"INSERT INTO Notification
                              (UserId, Channel, NotifType, Recipient, Subject, Body)
                          VALUES
                              (@UserId, 'Email', 'VoucherAssigned', @Recipient, @Subject, @Body)",
                        new
                        {
                            UserId    = user.UserId,
                            Recipient = user.Email,
                            Subject   = subject,
                            Body      = body
                        });
                }
            }
        }
    }
}
