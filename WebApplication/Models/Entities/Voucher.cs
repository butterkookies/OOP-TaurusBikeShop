// WebApplication/Models/Entities/Voucher.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a discount voucher definition — either percentage-based or fixed-amount.
/// Vouchers are created and managed in the AdminSystem. The WebApplication validates
/// and applies them during checkout via <c>VoucherService</c>.
/// <para>
/// Validation chain (enforced in <c>VoucherService.ValidateAsync</c> in strict order):
/// <list type="number">
///   <item>Voucher exists and <see cref="IsActive"/> is <c>true</c></item>
///   <item><see cref="StartDate"/> &lt;= now &lt;= <see cref="EndDate"/> (if set)</item>
///   <item>Cart subtotal &gt;= <see cref="MinimumOrderAmount"/> (if set)</item>
///   <item>Global usage count &lt; <see cref="MaxUses"/> (if set)</item>
///   <item>Per-user usage count &lt; <see cref="MaxUsesPerUser"/> (if set)</item>
/// </list>
/// </para>
/// </summary>
public sealed class Voucher
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int VoucherId { get; set; }

    /// <summary>
    /// Unique voucher code entered by the customer at checkout (e.g. TAURUS10, FREESHIP).
    /// Enforced unique via a unique index on the database.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    /// <summary>Optional human-readable description of the voucher's purpose.</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Whether the discount is a percentage of the subtotal or a fixed peso amount.
    /// Use <see cref="DiscountTypes"/> constants instead of magic strings.
    /// Constrained by CK_Voucher_DiscountType: Percentage or Fixed.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string DiscountType { get; set; } = string.Empty;

    /// <summary>
    /// The discount value. Interpretation depends on <see cref="DiscountType"/>:
    /// <list type="bullet">
    ///   <item>Percentage: value is 0–100 (e.g. 10 = 10% off)</item>
    ///   <item>Fixed: value is a peso amount (e.g. 500 = ₱500 off)</item>
    /// </list>
    /// Must be greater than 0 — enforced by CK_Voucher_DiscountVal.
    /// </summary>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Minimum cart subtotal required to apply this voucher.
    /// NULL means no minimum. Must be &gt;= 0 when set.
    /// </summary>
    public decimal? MinimumOrderAmount { get; set; }

    /// <summary>
    /// Maximum number of times this voucher can be redeemed globally across all users.
    /// NULL means unlimited. Must be &gt; 0 when set.
    /// </summary>
    public int? MaxUses { get; set; }

    /// <summary>
    /// Maximum number of times a single user can redeem this voucher.
    /// NULL means unlimited per user. Must be &gt; 0 when set.
    /// </summary>
    public int? MaxUsesPerUser { get; set; }

    /// <summary>UTC date and time from which this voucher becomes valid.</summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// UTC date and time after which this voucher expires.
    /// NULL means the voucher never expires.
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Whether this voucher is currently active and available for use.
    /// Inactive vouchers fail the first validation check regardless of dates.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>UTC timestamp when this voucher row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// User-specific voucher assignments. Controls which users have access
    /// to restricted vouchers and any per-user expiry overrides.
    /// </summary>
    public ICollection<UserVoucher> UserVouchers { get; set; } = new List<UserVoucher>();

    /// <summary>
    /// Redemption log entries. One row per successful voucher use per order.
    /// Used to enforce <see cref="MaxUses"/> and <see cref="MaxUsesPerUser"/> counts.
    /// </summary>
    public ICollection<VoucherUsage> Usages { get; set; } = new List<VoucherUsage>();
}

/// <summary>
/// Compile-time constants for all valid voucher discount type values.
/// Mirrors the CK_Voucher_DiscountType CHECK constraint in the database.
/// </summary>
public static class DiscountTypes
{
    /// <summary>Discount is calculated as a percentage of the cart subtotal.</summary>
    public const string Percentage = "Percentage";

    /// <summary>Discount is a fixed peso amount deducted from the cart subtotal.</summary>
    public const string Fixed = "Fixed";
}