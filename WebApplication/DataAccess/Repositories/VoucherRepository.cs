// WebApplication/DataAccess/Repositories/VoucherRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Voucher"/>, <see cref="UserVoucher"/>,
/// and <see cref="VoucherUsage"/> entities.
/// Supports the 5-step validation chain in <c>VoucherService.ValidateAsync</c>
/// and usage recording at order confirmation.
/// </summary>
public sealed class VoucherRepository : Repository<Voucher>
{
    /// <inheritdoc/>
    public VoucherRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns a voucher by its code string.
    /// Case-sensitive match — voucher codes are stored and compared as entered.
    /// Returns <c>null</c> when no voucher with that code exists.
    /// </summary>
    /// <param name="code">The voucher code to look up (e.g. TAURUS10).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching <see cref="Voucher"/>, or <c>null</c>.</returns>
    public async Task<Voucher?> GetByCodeAsync(
        string code,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        return await Context.Vouchers
            .AsNoTracking()
            .FirstOrDefaultAsync(
                v => v.Code.ToUpper() == code.ToUpper(), cancellationToken);
    }

    /// <summary>
    /// Returns the total number of times a voucher has been redeemed
    /// across all users. Used to enforce <c>Voucher.MaxUses</c>.
    /// </summary>
    /// <param name="voucherId">The voucher ID to count redemptions for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Total global redemption count.</returns>
    public async Task<int> GetGlobalUsageCountAsync(
        int voucherId,
        CancellationToken cancellationToken = default)
    {
        return await Context.VoucherUsages
            .CountAsync(vu => vu.VoucherId == voucherId, cancellationToken);
    }

    /// <summary>
    /// Returns the number of times a specific user has redeemed a voucher.
    /// Used to enforce <c>Voucher.MaxUsesPerUser</c>.
    /// </summary>
    /// <param name="voucherId">The voucher ID to count.</param>
    /// <param name="userId">The user ID to count for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Per-user redemption count.</returns>
    public async Task<int> GetUserUsageCountAsync(
        int voucherId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.VoucherUsages
            .CountAsync(
                vu => vu.VoucherId == voucherId && vu.UserId == userId,
                cancellationToken);
    }

    /// <summary>
    /// Records a successful voucher redemption by inserting a
    /// <see cref="VoucherUsage"/> row. This row is the source of truth
    /// for usage count enforcement in future validation checks.
    /// </summary>
    /// <param name="voucherId">The voucher that was redeemed.</param>
    /// <param name="userId">The user who redeemed it.</param>
    /// <param name="orderId">The order the voucher was applied to.</param>
    /// <param name="discountAmount">The actual discount amount applied.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task WriteUsageAsync(
        int voucherId,
        int userId,
        int orderId,
        decimal discountAmount,
        CancellationToken cancellationToken = default)
    {
        VoucherUsage usage = new()
        {
            VoucherId = voucherId,
            UserId = userId,
            OrderId = orderId,
            DiscountAmount = discountAmount,
            UsedAt = DateTime.UtcNow
        };

        await Context.VoucherUsages.AddAsync(usage, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Fetches all active vouchers assigned to the given user.
    /// Filters out inactive vouchers and those outside their valid window.
    /// Ordered by nearest expiration date first.
    /// </summary>
    /// <param name="userId">The user ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task<IReadOnlyList<UserVoucher>> GetActiveAssignedVouchersAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        DateTime now = DateTime.Now; // Use Local Time to match Admin system's DatePicker
        return await Context.UserVouchers
            .Include(uv => uv.Voucher)
            .AsNoTracking()
            .Where(uv => uv.UserId == userId 
                      && uv.Voucher.IsActive 
                      && uv.Voucher.StartDate <= now
                      && (uv.Voucher.EndDate == null || uv.Voucher.EndDate > now)
                      && (uv.ExpiresAt == null || uv.ExpiresAt > now))
            .OrderBy(uv => uv.ExpiresAt ?? uv.Voucher.EndDate)
            .ToListAsync(cancellationToken);
    }
}