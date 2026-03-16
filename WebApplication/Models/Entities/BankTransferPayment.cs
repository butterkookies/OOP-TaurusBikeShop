// WebApplication/Models/Entities/BankTransferPayment.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Bank transfer-specific payment data. Exists only when the parent
/// <see cref="Payment.PaymentMethod"/> = <c>BankTransfer</c>.
/// <para>
/// <b>Shared PK pattern:</b> <see cref="PaymentId"/> is simultaneously the primary
/// key of this table AND a foreign key to <c>Payment.PaymentId</c>, enforcing
/// a strict 1:1 relationship. Configured in AppDbContext via
/// HasOne/WithOne/HasForeignKey.
/// </para>
/// <para>
/// <b>Verification flow:</b>
/// <list type="number">
///   <item>Customer uploads proof screenshot → <see cref="ProofUrl"/>,
///     <see cref="ProofStorageBucket"/>, <see cref="ProofStoragePath"/> are set.
///     Payment status → VerificationPending.</item>
///   <item><see cref="VerificationDeadline"/> is set to <c>now + 24 hours</c>
///     by <c>PaymentService</c>.</item>
///   <item>Admin reviews and approves or rejects:
///     <see cref="VerifiedByUserId"/>, <see cref="VerifiedAt"/>, and optionally
///     <see cref="VerificationNotes"/> are set.</item>
///   <item>If deadline is exceeded without admin action,
///     <c>PaymentTimeoutJob</c> sets the order to OnHold.</item>
/// </list>
/// </para>
/// </summary>
public sealed class BankTransferPayment
{
    /// <summary>
    /// Primary key AND foreign key to <c>Payment.PaymentId</c>.
    /// Not auto-increment — value is set equal to the parent Payment's PK.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public int PaymentId { get; set; }

    /// <summary>
    /// Customer's bank reference number from their transaction receipt.
    /// Optional — not all banks display a reference number.
    /// </summary>
    [MaxLength(255)]
    public string? BpiReferenceNumber { get; set; }

    /// <summary>
    /// GCS public URL of the uploaded payment proof screenshot.
    /// NULL until the customer uploads proof.
    /// Used by the AdminSystem to display the proof image during verification.
    /// </summary>
    [MaxLength(1000)]
    public string? ProofUrl { get; set; }

    /// <summary>GCS bucket where the proof screenshot is stored.</summary>
    [MaxLength(200)]
    public string? ProofStorageBucket { get; set; }

    /// <summary>GCS object path for the proof screenshot within the bucket.</summary>
    [MaxLength(1000)]
    public string? ProofStoragePath { get; set; }

    /// <summary>
    /// FK to the admin user who approved or rejected this payment.
    /// NULL until an admin takes a verification action.
    /// </summary>
    public int? VerifiedByUserId { get; set; }

    /// <summary>
    /// Admin notes explaining the verification decision.
    /// Required when rejecting — should describe why the proof was rejected
    /// so the customer can resubmit correctly.
    /// Optional for approvals.
    /// </summary>
    [MaxLength(500)]
    public string? VerificationNotes { get; set; }

    /// <summary>
    /// UTC timestamp when the admin approved or rejected this payment.
    /// NULL until a verification decision is made.
    /// </summary>
    public DateTime? VerifiedAt { get; set; }

    /// <summary>
    /// UTC timestamp by which an admin must verify this payment.
    /// Set to <c>CreatedAt + 24 hours</c> by <c>PaymentService</c> when
    /// proof is uploaded.
    /// Polled by <c>PaymentTimeoutJob</c> every 5 minutes — if exceeded
    /// and still VerificationPending, the order is auto-held.
    /// NULL until proof is uploaded.
    /// </summary>
    public DateTime? VerificationDeadline { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent payment record this bank transfer data belongs to.</summary>
    public Payment Payment { get; set; } = null!;

    /// <summary>
    /// The admin who verified (approved or rejected) this payment.
    /// NULL until verification is complete.
    /// </summary>
    public User? VerifiedBy { get; set; }
}