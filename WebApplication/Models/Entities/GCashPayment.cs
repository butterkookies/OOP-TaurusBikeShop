// WebApplication/Models/Entities/GCashPayment.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// GCash-specific payment data. Exists only when the parent
/// <see cref="Payment.PaymentMethod"/> = <c>GCash</c>.
/// <para>
/// <b>Shared PK pattern:</b> <see cref="PaymentId"/> is simultaneously the primary
/// key of this table AND a foreign key to <c>Payment.PaymentId</c>. This enforces
/// a strict 1:1 relationship at the database level. The value is never
/// auto-generated here — it is always copied from the parent Payment row.
/// Configured in AppDbContext via:
/// <code>
/// modelBuilder.Entity&lt;GCashPayment&gt;()
///     .HasOne(g => g.Payment)
///     .WithOne(p => p.GCashPayment)
///     .HasForeignKey&lt;GCashPayment&gt;(g => g.PaymentId);
/// </code>
/// </para>
/// <para>
/// <b>Transaction flow:</b> When the GCash gateway callback confirms payment,
/// <c>PaymentService.InitiateGCashAsync</c> stores the gateway reference in
/// <see cref="GcashTransactionId"/> and updates the parent Payment status
/// to Completed. NULL while the payment is Pending or Failed.
/// </para>
/// </summary>
public sealed class GCashPayment
{
    /// <summary>
    /// Primary key AND foreign key to <c>Payment.PaymentId</c>.
    /// Not auto-increment — value is set equal to the parent Payment's PK.
    /// Configured via HasOne/WithOne/HasForeignKey in AppDbContext.
    /// </summary>
    public int PaymentId { get; set; }

    /// <summary>
    /// GCash gateway transaction reference number returned after successful payment.
    /// NULL while the payment is Pending or Failed — populated only after the
    /// gateway callback confirms the transaction.
    /// </summary>
    [MaxLength(255)]
    public string? GcashTransactionId { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent payment record this GCash data belongs to.</summary>
    public Payment Payment { get; set; } = null!;
}