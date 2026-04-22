// WebApplication/Models/ViewModels/OrderViewModel.cs

using WebApplication.Models.Entities;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for order confirmation, order detail, and order history rows.
/// Populated by <c>OrderService</c>.
/// </summary>
public sealed class OrderViewModel
{
    public int    OrderId       { get; set; }
    public string OrderNumber   { get; set; } = string.Empty;
    public string OrderStatus   { get; set; } = string.Empty;
    public DateTime OrderDate   { get; set; }
    public int    ItemCount     { get; set; }
    public string DeliveryMethod{ get; set; } = string.Empty;
    public decimal ShippingFee  { get; set; }
    public decimal DiscountAmount{ get; set; }

    // Detail-only fields (null on list rows)
    public IReadOnlyList<OrderItemViewModel> Items { get; set; } = [];
    public decimal  SubTotal        { get; set; }
    public Address? ShippingAddress { get; set; }
    public PickupOrder? PickupOrder { get; set; }
    public IReadOnlyList<Payment>   Payments   { get; set; } = [];
    public IReadOnlyList<Delivery>  Deliveries { get; set; } = [];
    public bool IsCancellable          { get; set; }

    /// <summary>
    /// True when the customer can confirm receipt of this order.
    /// Only set for delivery orders (not pickup, not walk-in) in
    /// <c>OutForDelivery</c> status. Drives the "Confirm Delivery" button on the
    /// order detail page. Flowchart: Part 6 — D26.
    /// </summary>
    public bool IsDeliveryConfirmable  { get; set; }

    // Computed
    public decimal GrandTotal =>
        SubTotal - DiscountAmount + ShippingFee;
    public string FormattedGrandTotal => $"₱{GrandTotal:N2}";
    public string FormattedSubTotal   => $"₱{SubTotal:N2}";
    public string FormattedShippingFee=> $"₱{ShippingFee:N2}";
    public string FormattedOrderDate  => OrderDate.ToString("MMMM d, yyyy h:mm tt");

    // ── 24-hour cancellation countdown ─────────────────────────────────
    /// <summary>The deadline after which this pending order will be auto-cancelled.</summary>
    public DateTime CancellationDeadline => OrderDate.AddHours(24);

    /// <summary>True when the order is Pending and the countdown should be displayed.</summary>
    public bool ShowCancellationTimer => OrderStatus == "Pending";

    /// <summary>ISO 8601 deadline string for the JavaScript countdown timer.</summary>
    public string CancellationDeadlineIso => CancellationDeadline.ToString("o");
}

/// <summary>Single line item within an order view model.</summary>
public sealed class OrderItemViewModel
{
    public int     OrderItemId { get; set; }
    public int     ProductId   { get; set; }
    public string  ProductName { get; set; } = string.Empty;
    public string? VariantName { get; set; }
    public string? ImageUrl    { get; set; }
    public int     Quantity    { get; set; }
    public decimal UnitPrice   { get; set; }
    public decimal LineTotal   => Quantity * UnitPrice;
    public string  FormattedLineTotal => $"₱{LineTotal:N2}";
    public string  FormattedUnitPrice => $"₱{UnitPrice:N2}";
}