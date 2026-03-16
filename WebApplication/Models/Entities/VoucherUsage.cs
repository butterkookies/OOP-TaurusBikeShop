// WebApplication/Models/Entities/VoucherUsage.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Immutable redemption log — records each successful voucher application to an order.
/// One row is written per voucher per order by <c>VoucherService.ApplyAsync</c>
/// at the moment the order is confirmed.
/// <para>
/// <b>Usage counting:</b> This table is the source of truth for voucher usage counts.
/// <c>VoucherRepository.GetGlobalUsageCountAsync</c> and
/// <c>GetUserUsageCountAsync</c> COUNT rows here to enforce
/// <c>Voucher.MaxUses</c> and <c>Voucher.MaxUsesPerUser</c> limits
/// during the validation chain in <c>VoucherService.ValidateAsync</c>.
/// </para>
/// <para>
/// Rows in this table must never be updated or deleted — they are an
/// append-only redemption audit log.
/// </para>
/// </summary>
public sealed class VoucherUsage
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int VoucherUsageId { get; set; }

    /// <summary>FK to the voucher that was redeemed.</summary>
    public int VoucherId { get; set; }

    /// <summary>FK to the user who redeemed the voucher.</summary>
    public int UserId { get; set; }

    /// <summary>FK to the order the voucher was applied to.</summary>
    public int OrderId { get; set; }

    /// <summary>
    /// The actual discount amount that was applied to the order.
    /// For Percentage vouchers: <c>SubTotal × DiscountValue / 100</c>.
    /// For Fixed vouchers: <c>DiscountValue</c>.
    /// Must be &gt;= 0 — enforced by CK_VoucherUsage_Disc.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>UTC timestamp when this voucher was redeemed.</summary>
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The voucher that was redeemed.</summary>
    public Voucher Voucher { get; set; } = null!;

    /// <summary>The user who redeemed the voucher.</summary>
    public User User { get; set; } = null!;

    /// <summary>The order the voucher was applied to.</summary>
    public Order Order { get; set; } = null!;
}