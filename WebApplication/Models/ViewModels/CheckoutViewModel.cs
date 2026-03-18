// WebApplication/Models/ViewModels/CheckoutViewModel.cs

using System.ComponentModel.DataAnnotations;
using WebApplication.Models.Entities;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for the checkout page — shipping, payment method,
/// voucher entry, and the read-only order summary sidebar.
/// Bound to the checkout form POST.
/// </summary>
public sealed class CheckoutViewModel
{
    // -------------------------------------------------------------------------
    // Shipping
    // -------------------------------------------------------------------------

    /// <summary>
    /// The address ID selected for delivery.
    /// Required for non-walk-in orders.
    /// </summary>
    [Display(Name = "Delivery Address")]
    public int? SelectedAddressId { get; set; }

    /// <summary>
    /// All saved (non-snapshot) addresses for the user —
    /// rendered as the address selector radio group.
    /// </summary>
    public IReadOnlyList<Address> SavedAddresses { get; set; } = [];

    /// <summary>
    /// Delivery method chosen by the customer.
    /// Values: "Lalamove", "LBC", "Pickup".
    /// </summary>
    [Required(ErrorMessage = "Please select a delivery method.")]
    [Display(Name = "Delivery Method")]
    public string DeliveryMethod { get; set; } = string.Empty;

    /// <summary>
    /// Estimated shipping fee for the selected delivery method.
    /// Displayed read-only — not editable by customer.
    /// Set by <c>CheckoutController</c> based on <see cref="DeliveryMethod"/>.
    /// </summary>
    public decimal ShippingFee { get; set; }

    // -------------------------------------------------------------------------
    // Payment
    // -------------------------------------------------------------------------

    /// <summary>
    /// Payment method chosen by the customer.
    /// Values: GCash, BankTransfer — no Cash, no Card, no COD.
    /// </summary>
    [Required(ErrorMessage = "Please select a payment method.")]
    [Display(Name = "Payment Method")]
    public string PaymentMethod { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Voucher
    // -------------------------------------------------------------------------

    /// <summary>
    /// Voucher code applied to this order.
    /// Populated from session after <c>VoucherController.Validate</c> succeeds.
    /// Null when no voucher is applied.
    /// </summary>
    public string? VoucherCode { get; set; }

    /// <summary>
    /// Discount amount from the applied voucher.
    /// Zero when no voucher is applied.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    // -------------------------------------------------------------------------
    // Order summary (read-only, populated by controller)
    // -------------------------------------------------------------------------

    /// <summary>Cart items for the read-only order summary sidebar.</summary>
    public IReadOnlyList<CartItemViewModel> CartItems { get; set; } = [];

    /// <summary>Cart subtotal before discount and shipping.</summary>
    public decimal SubTotal { get; set; }

    /// <summary>Formatted subtotal string.</summary>
    public string FormattedSubTotal => $"₱{SubTotal:N2}";

    /// <summary>Formatted shipping fee string.</summary>
    public string FormattedShippingFee =>
        ShippingFee == 0 ? "TBD" : $"₱{ShippingFee:N2}";

    /// <summary>Grand total: SubTotal − DiscountAmount + ShippingFee.</summary>
    public decimal GrandTotal => SubTotal - DiscountAmount + ShippingFee;

    /// <summary>Formatted grand total string.</summary>
    public string FormattedGrandTotal => $"₱{GrandTotal:N2}";

    // -------------------------------------------------------------------------
    // Shipping fee table (constants used by controller and view)
    // -------------------------------------------------------------------------

    /// <summary>Lalamove base shipping fee (PHP).</summary>
    public const decimal LalamoveFee = 150m;

    /// <summary>LBC base shipping fee (PHP).</summary>
    public const decimal LBCFee = 120m;

    /// <summary>In-store pickup — no shipping fee.</summary>
    public const decimal PickupFee = 0m;
}