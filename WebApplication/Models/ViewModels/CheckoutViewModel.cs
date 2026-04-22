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
    /// Fulfilment method chosen by the customer.
    /// Values: "Delivery", "Pickup".
    /// The specific courier (Lalamove vs LBC) is auto-assigned server-side
    /// based on the delivery address province.
    /// </summary>
    [Required(ErrorMessage = "Please select a delivery method.")]
    [Display(Name = "Delivery Method")]
    public string DeliveryMethod { get; set; } = string.Empty;

    /// <summary>
    /// Always ₱0 in the online payment — shipping is paid to the courier directly.
    /// </summary>
    public decimal ShippingFee => 0m;

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

    /// <summary>
    /// Active vouchers explicitly assigned to this user.
    /// Used to populate the quick-select combobox.
    /// </summary>
    public IReadOnlyList<AssignedVoucherViewModel> AssignedVouchers { get; set; } = [];

    // -------------------------------------------------------------------------
    // Order summary (read-only, populated by controller)
    // -------------------------------------------------------------------------

    /// <summary>Cart items for the read-only order summary sidebar.</summary>
    public IReadOnlyList<CartItemViewModel> CartItems { get; set; } = [];

    /// <summary>Cart subtotal before discount. Shipping is excluded (paid to courier).</summary>
    public decimal SubTotal { get; set; }

    /// <summary>Formatted subtotal string.</summary>
    public string FormattedSubTotal => $"₱{SubTotal:N2}";

    /// <summary>
    /// Shipping display label — "Free" for Pickup, "Paid to courier" for Delivery.
    /// </summary>
    public string FormattedShippingFee =>
        DeliveryMethod == "Pickup" ? "Free" : "Paid to courier";

    /// <summary>
    /// Grand total for online payment: SubTotal − DiscountAmount only.
    /// Shipping is excluded — paid directly to the courier.
    /// </summary>
    public decimal GrandTotal => SubTotal - DiscountAmount;

    /// <summary>Formatted grand total string.</summary>
    public string FormattedGrandTotal => $"₱{GrandTotal:N2}";

}