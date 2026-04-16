// WebApplication/BusinessLogic/Services/VoucherService.cs

using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IVoucherService"/> — 6-step voucher validation chain
/// and usage recording at order creation.
/// <para>
/// Validation order follows flowchart Part 15 (V6 → V7 → V8 → V9 → V10):
/// <list type="number">
///   <item><description>Code format (local, no DB)</description></item>
///   <item><description>Exists and IsActive</description></item>
///   <item><description>Within StartDate / EndDate window</description></item>
///   <item><description>Subtotal meets MinimumOrderAmount</description></item>
///   <item><description>Global usage cap (MaxUses)</description></item>
///   <item><description>Per-user usage cap (MaxUsesPerUser)</description></item>
/// </list>
/// Steps 1–4 are cheap (no extra DB round-trips); steps 5–6 are DB queries
/// and are intentionally deferred until the cart value is confirmed valid.
/// </para>
/// </summary>
public sealed class VoucherService : IVoucherService
{
    private readonly VoucherRepository _voucherRepo;

    /// <inheritdoc/>
    public VoucherService(VoucherRepository voucherRepo)
    {
        _voucherRepo = voucherRepo ?? throw new ArgumentNullException(nameof(voucherRepo));
    }

    /// <inheritdoc/>
    public async Task<VoucherValidationResult> ValidateAsync(
        string code,
        int userId,
        decimal orderSubTotal,
        CancellationToken cancellationToken = default)
    {
        // ── Step 1: Code format ────────────────────────────────────────────
        if (!ValidationHelper.IsValidVoucherCode(code))
            return Fail("Invalid voucher code format.");

        // ── Step 2: Exists and is active ───────────────────────────────────
        Voucher? voucher = await _voucherRepo.GetByCodeAsync(code, cancellationToken);
        if (voucher is null || !voucher.IsActive)
            return Fail("This voucher code does not exist or has been deactivated.");

        // ── Step 3: Date window ────────────────────────────────────────────
        DateTime now = DateTime.Now; // Use Local Time to match Admin system's DatePicker
        if (now < voucher.StartDate)
            return Fail("This voucher is not yet active.");
        if (voucher.EndDate.HasValue && now > voucher.EndDate.Value)
            return Fail("This voucher has expired.");

        // ── Step 4: Minimum order amount ───────────────────────────────────
        // Checked before the usage-cap queries (steps 5-6) so we skip two
        // unnecessary database round-trips when the cart subtotal is too low.
        // Flowchart reference: Part 15 — V8 (before V9 / V10).
        if (voucher.MinimumOrderAmount.HasValue && orderSubTotal < voucher.MinimumOrderAmount.Value)
            return Fail(
                $"This voucher requires a minimum order of \u20b1{voucher.MinimumOrderAmount.Value:N2}.");

        // ── Step 5: Global usage cap ───────────────────────────────────────
        if (voucher.MaxUses.HasValue)
        {
            int globalCount =
                await _voucherRepo.GetGlobalUsageCountAsync(voucher.VoucherId, cancellationToken);
            if (globalCount >= voucher.MaxUses.Value)
                return Fail("This voucher has reached its maximum number of uses.");
        }

        // ── Step 6: Per-user usage cap ─────────────────────────────────────
        if (voucher.MaxUsesPerUser.HasValue)
        {
            int userCount = await _voucherRepo.GetUserUsageCountAsync(
                voucher.VoucherId, userId, cancellationToken);
            if (userCount >= voucher.MaxUsesPerUser.Value)
                return Fail("You have already used this voucher the maximum number of times.");
        }

        // ── Calculate discount ─────────────────────────────────────────────
        decimal discountAmount = voucher.DiscountType == DiscountTypes.Percentage
            ? Math.Round(orderSubTotal * (voucher.DiscountValue / 100m), 2)
            : voucher.DiscountValue;

        // Cap discount to subtotal — never go negative
        discountAmount = Math.Min(discountAmount, orderSubTotal);

        decimal newTotal = orderSubTotal - discountAmount;

        string formattedDiscount = voucher.DiscountType == DiscountTypes.Percentage
            ? $"{voucher.DiscountValue:0.##}% off"
            : $"\u20b1{discountAmount:N2} off";

        string description = voucher.DiscountType == DiscountTypes.Percentage
            ? $"{voucher.DiscountValue:0.##}% off your order"
            : $"\u20b1{voucher.DiscountValue:N2} off your order";

        return new VoucherValidationResult
        {
            IsValid           = true,
            DiscountAmount    = discountAmount,
            FormattedDiscount = formattedDiscount,
            NewTotal          = newTotal,
            FormattedNewTotal = $"\u20b1{newTotal:N2}",
            VoucherCode       = voucher.Code,
            Description       = description
        };
    }

    /// <inheritdoc/>
    public async Task RedeemAsync(
        string code,
        int userId,
        int orderId,
        decimal discountAmount,
        CancellationToken cancellationToken = default)
    {
        Voucher? voucher = await _voucherRepo.GetByCodeAsync(code, cancellationToken);
        if (voucher is null)
            throw new InvalidOperationException(
                $"Cannot redeem voucher '{code}': voucher not found.");

        await _voucherRepo.WriteUsageAsync(
            voucher.VoucherId, userId, orderId, discountAmount, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<UserVoucher>> GetActiveAssignedVouchersAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await _voucherRepo.GetActiveAssignedVouchersAsync(userId, cancellationToken);
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private static VoucherValidationResult Fail(string error) =>
        new() { IsValid = false, Error = error };
}
