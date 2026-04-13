// WebApplication/Models/ViewModels/CartViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for the full shopping cart page and the order summary sidebar.
/// Populated by <c>CartService.GetCartViewModelAsync</c>.
/// </summary>
public sealed class CartViewModel
{
    /// <summary>The cart's database ID. Used in AJAX calls.</summary>
    public int CartId { get; set; }

    /// <summary>All line items currently in the cart.</summary>
    public IReadOnlyList<CartItemViewModel> Items { get; set; } = [];

    /// <summary>
    /// Sum of all line totals before any discount or shipping.
    /// Computed as SUM(Quantity × UnitPrice) across all items.
    /// </summary>
    public decimal SubTotal { get; set; }

    /// <summary>Number of distinct line items in the cart.</summary>
    public int ItemCount => Items.Count;

    /// <summary>Total units across all line items (sum of quantities).</summary>
    public int TotalQuantity => Items.Sum(i => i.Quantity);

    /// <summary>
    /// Voucher code applied to this cart during the checkout flow.
    /// Null when no voucher has been applied.
    /// </summary>
    public string? AppliedVoucherCode { get; set; }

    /// <summary>
    /// Discount amount from the applied voucher.
    /// Zero when no voucher is applied.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Shipping fee selected during checkout.
    /// Zero until delivery method is chosen — not set on the Cart page itself.
    /// </summary>
    public decimal ShippingFee { get; set; }

    /// <summary>
    /// Grand total: SubTotal - DiscountAmount + ShippingFee.
    /// Matches the formula used in <c>vw_OrderSummary</c>.
    /// </summary>
    public decimal GrandTotal => SubTotal - DiscountAmount + ShippingFee;

    /// <summary>Formatted SubTotal string (e.g. ₱12,500.00).</summary>
    public string FormattedSubTotal => $"₱{SubTotal:N2}";

    /// <summary>Formatted GrandTotal string.</summary>
    public string FormattedGrandTotal => $"₱{GrandTotal:N2}";

    /// <summary>Convenience: true when the cart has no items.</summary>
    public bool IsEmpty => Items.Count == 0;
}

/// <summary>
/// Represents a single line item within the cart view model.
/// </summary>
public sealed class CartItemViewModel
{
    /// <summary>The cart item's database ID. Used in update/remove AJAX calls.</summary>
    public int CartItemId { get; set; }

    /// <summary>The product's database ID.</summary>
    public int ProductId { get; set; }

    /// <summary>The variant's database ID. Null for products with no real variants.</summary>
    public int? VariantId { get; set; }

    /// <summary>Product display name.</summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// Variant display name (e.g. "Large", "Red").
    /// Null or "Default" for products with no real variants.
    /// </summary>
    public string? VariantName { get; set; }

    /// <summary>Primary image URL. Null when no image is available.</summary>
    public string? ImageUrl { get; set; }

    /// <summary>Quantity of this item in the cart.</summary>
    public int Quantity { get; set; }

    /// <summary>Available stock for this item's variant. Used to cap the quantity stepper.</summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// Price per unit at the time this item was added to the cart.
    /// This is a snapshot — does not change if the product price changes.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Line total: Quantity × UnitPrice.
    /// Computed client-side as well for AJAX quantity updates.
    /// </summary>
    public decimal LineTotal => Quantity * UnitPrice;

    /// <summary>Formatted unit price (e.g. ₱1,500.00).</summary>
    public string FormattedUnitPrice => $"₱{UnitPrice:N2}";

    /// <summary>Formatted line total.</summary>
    public string FormattedLineTotal => $"₱{LineTotal:N2}";
}