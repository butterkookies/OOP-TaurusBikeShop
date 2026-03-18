// WebApplication/BusinessLogic/Services/VoucherService.cs

using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IVoucherService"/> — 5-step voucher validation chain
/// and usage recording at order creation.
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
        DateTime now = DateTime.UtcNow;
        if (voucher.StartDate.HasValue && now < voucher.StartDate.Value)
            return Fail("This voucher is not yet active.");
        if (voucher.EndDate.HasValue && now > voucher.EndDate.Value)
            return Fail("This voucher has expired.");

        // ── Step 4: Global usage cap ───────────────────────────────────────
        if (voucher.MaxUses.HasValue)
        {
            int globalCount =
                await _voucherRepo.GetGlobalUsageCountAsync(voucher.VoucherId, cancellationToken);
            if (globalCount >= voucher.MaxUses.Value)
                return Fail("This voucher has reached its maximum number of uses.");
        }

        // ── Step 5: Per-user usage cap ─────────────────────────────────────
        if (voucher.MaxUsesPerUser.HasValue)
        {
            int userCount = await _voucherRepo.GetUserUsageCountAsync(
                voucher.VoucherId, userId, cancellationToken);
            if (userCount >= voucher.MaxUsesPerUser.Value)
                return Fail("You have already used this voucher the maximum number of times.");
        }

        // ── Minimum order value check ──────────────────────────────────────
        if (voucher.MinimumOrderValue.HasValue && orderSubTotal < voucher.MinimumOrderValue.Value)
            return Fail(
                $"This voucher requires a minimum order of ₱{voucher.MinimumOrderValue.Value:N2}.");

        // ── Calculate discount ─────────────────────────────────────────────
        decimal discountAmount = voucher.DiscountType == DiscountTypes.Percentage
            ? Math.Round(orderSubTotal * (voucher.DiscountValue / 100m), 2)
            : voucher.DiscountValue;

        // Cap discount to subtotal — never go negative
        discountAmount = Math.Min(discountAmount, orderSubTotal);

        decimal newTotal = orderSubTotal - discountAmount;

        string formattedDiscount = voucher.DiscountType == DiscountTypes.Percentage
            ? $"{voucher.DiscountValue:0.##}% off"
            : $"₱{discountAmount:N2} off";

        string description = voucher.DiscountType == DiscountTypes.Percentage
            ? $"{voucher.DiscountValue:0.##}% off your order"
            : $"₱{voucher.DiscountValue:N2} off your order";

        return new VoucherValidationResult
        {
            IsValid           = true,
            DiscountAmount    = discountAmount,
            FormattedDiscount = formattedDiscount,
            NewTotal          = newTotal,
            FormattedNewTotal = $"₱{newTotal:N2}",
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

    // =========================================================================
    // Private helpers
    // =========================================================================

    private static VoucherValidationResult Fail(string error) =>
        new() { IsValid = false, Error = error };
}