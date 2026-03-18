// WebApplication/Models/ViewModels/VoucherViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// Result returned to the client after a voucher validation attempt.
/// Used as the JSON payload from <c>VoucherController.Validate</c>.
/// </summary>
public sealed class VoucherValidationResult
{
    /// <summary><c>true</c> when the voucher is valid and can be applied.</summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// Calculated discount amount in PHP.
    /// Zero when <see cref="IsValid"/> is <c>false</c>.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>Formatted discount string (e.g. ₱500.00 or 10%).</summary>
    public string FormattedDiscount { get; set; } = string.Empty;

    /// <summary>
    /// Order total after applying the discount.
    /// Zero when <see cref="IsValid"/> is <c>false</c>.
    /// </summary>
    public decimal NewTotal { get; set; }

    /// <summary>Formatted new total string (e.g. ₱4,500.00).</summary>
    public string FormattedNewTotal { get; set; } = string.Empty;

    /// <summary>
    /// The voucher code that was validated.
    /// Returned so the client can display it in the applied-voucher banner.
    /// </summary>
    public string? VoucherCode { get; set; }

    /// <summary>
    /// Human-readable description of the discount (e.g. "10% off your order").
    /// Shown in the checkout summary.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Validation failure reason. Null when <see cref="IsValid"/> is <c>true</c>.
    /// </summary>
    public string? Error { get; set; }
}