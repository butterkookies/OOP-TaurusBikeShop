// WebApplication/Models/Entities/OrderItem.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a single product line item within a customer order.
/// All values are locked at order creation time and are immutable thereafter.
/// <para>
/// <b>Computed subtotal:</b> <c>Subtotal = Quantity × UnitPrice</c> is computed
/// in <c>vw_OrderItemDetail</c> and must never be stored as a column or property.
/// Calculate it inline in ViewModels where needed.
/// </para>
/// <para>
/// <b>Price lock:</b> <see cref="UnitPrice"/> is copied from the cart item's
/// <c>PriceAtAdd</c> value at the moment the order is created. Subsequent changes
/// to the product price do not affect existing order items.
/// </para>
/// </summary>
public sealed class OrderItem
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int OrderItemId { get; set; }

    /// <summary>
    /// FK to the parent order. Configured with CASCADE DELETE — removing an order
    /// removes all its line items.
    /// </summary>
    public int OrderId { get; set; }

    /// <summary>FK to the product ordered.</summary>
    public int ProductId { get; set; }

    /// <summary>
    /// FK to the specific variant ordered.
    /// Should always be set — NULL only for legacy or walk-in POS rows
    /// where a variant was not explicitly selected.
    /// </summary>
    public int? ProductVariantId { get; set; }

    /// <summary>
    /// Number of units ordered. Must be &gt; 0 —
    /// enforced by CK_OrderItem_Quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Selling price per unit at the time the order was placed.
    /// Equals <c>Product.Price + ProductVariant.AdditionalPrice</c> at order time.
    /// Must be &gt;= 0 — enforced by CK_OrderItem_UnitPrice.
    /// Never recalculated after order creation.
    /// </summary>
    public decimal UnitPrice { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The order this line item belongs to.</summary>
    public Order Order { get; set; } = null!;

    /// <summary>The product that was ordered.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The specific variant that was ordered.
    /// NULL only for legacy rows — new order items always reference a variant.
    /// </summary>
    public ProductVariant? Variant { get; set; }
}