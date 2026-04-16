// WebApplication/Models/Entities/StorePaymentAccount.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// A store-owned payment account (GCash number / BPI account) that customers
/// should send money to. Rows are managed by admin via AdminSystem_v2 so
/// account numbers can rotate without a redeploy.
/// <para>
/// At most one row per <see cref="PaymentMethod"/> may have
/// <c>IsActive = true</c> (enforced by a filtered unique index).
/// </para>
/// </summary>
public sealed class StorePaymentAccount
{
    public int StorePaymentAccountId { get; set; }

    /// <summary>"GCash" or "BankTransfer". Constrained by CK_StorePaymentAccount_Method.</summary>
    [Required]
    [MaxLength(50)]
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>Account holder / registered name shown to the customer.</summary>
    [Required]
    [MaxLength(150)]
    public string AccountName { get; set; } = string.Empty;

    /// <summary>GCash mobile number or BPI account number (free-form string).</summary>
    [Required]
    [MaxLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    /// <summary>Bank name — only populated for BankTransfer rows.</summary>
    [MaxLength(100)]
    public string? BankName { get; set; }

    /// <summary>Cloudinary URL of a QR code image (optional, typically for GCash).</summary>
    [MaxLength(1000)]
    public string? QrImageUrl { get; set; }

    /// <summary>Short customer-facing instructions shown under the account.</summary>
    [MaxLength(500)]
    public string? Instructions { get; set; }

    public bool IsActive { get; set; } = true;

    public int DisplayOrder { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
