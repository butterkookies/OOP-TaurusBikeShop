// WebApplication/Models/Entities/Payment.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Base payment record — method-agnostic. One row per payment attempt per order.
/// Method-specific data lives in subtype tables sharing this row's PK:
/// <list type="bullet">
///   <item><see cref="GCashPayment"/> — when <see cref="PaymentMethod"/> = GCash</item>
///   <item><see cref="BankTransferPayment"/> — when <see cref="PaymentMethod"/> = BankTransfer</item>
///   <item>No subtype row — when <see cref="PaymentMethod"/> = Cash (POS only)</item>
/// </list>
/// <para>
/// <b>Payment methods:</b> GCash, BankTransfer, and Cash (POS only).
/// COD and card payments are NOT offered by Taurus Bike Shop.
/// Use <see cref="PaymentMethods"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Payment status flow:</b>
/// <list type="bullet">
///   <item>GCash: Pending → Completed | Failed</item>
///   <item>BankTransfer: Pending → VerificationPending → Completed | VerificationRejected | Failed</item>
///   <item>Cash (POS): directly → Completed</item>
/// </list>
/// Use <see cref="PaymentStatuses"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>No refunds:</b> Taurus does not offer refunds. No code path should ever
/// set a PaymentStatus value of 'Refunded' — that value does not exist in the
/// database CHECK constraint.
/// </para>
/// </summary>
public sealed class Payment
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int PaymentId { get; set; }

    /// <summary>FK to the order this payment is for.</summary>
    public int OrderId { get; set; }

    /// <summary>
    /// Payment method used. Constrained by CK_Payment_Method: GCash, BankTransfer, Cash.
    /// COD and card are NOT valid values.
    /// Use <see cref="PaymentMethods"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// Whether this is an upfront payment or a confirmation payment.
    /// Constrained by CK_Payment_Stage: Upfront or Confirmation.
    /// Use <see cref="PaymentStages"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string PaymentStage { get; set; } = PaymentStages.Upfront;

    /// <summary>
    /// Current status of this payment.
    /// Constrained by CK_Payment_Status: Pending, VerificationPending,
    /// VerificationRejected, Completed, Failed.
    /// Use <see cref="PaymentStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string PaymentStatus { get; set; } = PaymentStatuses.Pending;

    /// <summary>
    /// Payment amount in the order's currency. Must be &gt;= 0 —
    /// enforced by CK_Payment_Amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// UTC timestamp when payment was confirmed or completed.
    /// NULL until the payment reaches a terminal status (Completed or Failed).
    /// </summary>
    public DateTime? PaymentDate { get; set; }

    /// <summary>UTC timestamp when this payment row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The order this payment is associated with.</summary>
    public Order Order { get; set; } = null!;

    /// <summary>
    /// GCash-specific payment data. NULL when PaymentMethod != GCash.
    /// 1:1 relationship — PaymentId is both PK and FK on GCashPayment.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public GCashPayment? GCashPayment { get; set; }

    /// <summary>
    /// Bank transfer-specific payment data. NULL when PaymentMethod != BankTransfer.
    /// 1:1 relationship — PaymentId is both PK and FK on BankTransferPayment.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public BankTransferPayment? BankTransferPayment { get; set; }
}

/// <summary>
/// Compile-time constants for all valid payment method values.
/// Mirrors the CK_Payment_Method CHECK constraint in the database.
/// COD and card are NOT included — they are not offered by Taurus Bike Shop.
/// </summary>
public static class PaymentMethods
{
    /// <summary>GCash mobile wallet payment — available for online orders.</summary>
    public const string GCash = "GCash";

    /// <summary>Bank transfer payment — requires admin proof verification.</summary>
    public const string BankTransfer = "BankTransfer";

    /// <summary>Cash payment — POS walk-in transactions only.</summary>
    public const string Cash = "Cash";
}

/// <summary>
/// Compile-time constants for all valid payment stage values.
/// Mirrors the CK_Payment_Stage CHECK constraint in the database.
/// </summary>
public static class PaymentStages
{
    /// <summary>Full upfront payment — standard for all online orders.</summary>
    public const string Upfront = "Upfront";

    /// <summary>Confirmation payment — used for staged or deposit-based payments.</summary>
    public const string Confirmation = "Confirmation";
}

/// <summary>
/// Compile-time constants for all valid payment status values.
/// Mirrors the CK_Payment_Status CHECK constraint in the database.
/// Note: 'Refunded' is NOT a valid status — Taurus does not offer refunds.
/// </summary>
public static class PaymentStatuses
{
    /// <summary>Payment has been initiated but not yet processed or verified.</summary>
    public const string Pending = "Pending";

    /// <summary>
    /// Bank transfer proof has been uploaded and is awaiting admin verification.
    /// </summary>
    public const string VerificationPending = "VerificationPending";

    /// <summary>Admin has reviewed and rejected the bank transfer proof.</summary>
    public const string VerificationRejected = "VerificationRejected";

    /// <summary>Payment has been successfully processed and confirmed.</summary>
    public const string Completed = "Completed";

    /// <summary>Payment processing failed — e.g. GCash gateway error.</summary>
    public const string Failed = "Failed";
}