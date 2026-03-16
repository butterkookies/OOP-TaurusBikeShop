// WebApplication/Models/Entities/PurchaseOrderItem.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a single product line item within a supplier purchase order.
/// All values are locked at purchase order creation time and are immutable thereafter.
/// <para>
/// <b>Computed subtotal:</b> <c>Subtotal = Quantity × UnitPrice</c> is computed
/// in <c>vw_PurchaseOrderDetail</c> and must never be stored as a column or property.
/// </para>
/// <para>
/// <b>UnitPrice lock:</b> <see cref="UnitPrice"/> records the wholesale price
/// agreed with the supplier at the time the PO was created. Changes to the
/// product's retail price after PO creation do not affect this value.
/// </para>
/// <para>
/// <b>Stock receiving:</b> When the AdminSystem receives stock for this line item,
/// <c>SupplierRepository.ReceiveStockAsync</c> increments
/// <c>ProductVariant.StockQuantity</c> by the received quantity and writes
/// an <c>InventoryLog</c> row with <c>ChangeType = Purchase</c>.
/// </para>
/// </summary>
public sealed class PurchaseOrderItem
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int PurchaseOrderItemId { get; set; }

    /// <summary>
    /// FK to the parent purchase order.
    /// Configured with CASCADE DELETE — removing a PO removes all its line items.
    /// </summary>
    public int PurchaseOrderId { get; set; }

    /// <summary>FK to the product being ordered from the supplier.</summary>
    public int ProductId { get; set; }

    /// <summary>
    /// FK to the specific variant being ordered.
    /// NULL when the PO is for a product without a specific variant selection
    /// (e.g. ordering unvariated stock at the product level).
    /// </summary>
    public int? ProductVariantId { get; set; }

    /// <summary>
    /// Number of units ordered. Must be &gt; 0 —
    /// enforced by CK_POItem_Quantity.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Wholesale price per unit agreed with the supplier at PO creation time.
    /// Must be &gt;= 0 — enforced by CK_POItem_UnitPrice.
    /// Never recalculated after PO creation.
    /// </summary>
    public decimal UnitPrice { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent purchase order this line item belongs to.</summary>
    public PurchaseOrder PurchaseOrder { get; set; } = null!;

    /// <summary>The product being ordered.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The specific variant being ordered.
    /// NULL when no variant was specified.
    /// </summary>
    public ProductVariant? Variant { get; set; }
}