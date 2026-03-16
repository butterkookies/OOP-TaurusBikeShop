// WebApplication/Models/Entities/PurchaseOrder.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents an admin-created restocking order sent to a supplier.
/// Purchase orders are created and managed exclusively through the AdminSystem.
/// The WebApplication entity exists only to support EF Core relationship
/// mapping — there are no PurchaseOrder controllers or views in the WebApplication.
/// <para>
/// <b>Computed total:</b> <c>TotalAmount = SUM(Quantity × UnitPrice)</c> across all
/// line items is computed in <c>vw_PurchaseOrderDetail</c> and must never be stored
/// as a column or property.
/// </para>
/// <para>
/// <b>Status flow:</b> Pending → Received | Cancelled.
/// Use <see cref="PurchaseOrderStatuses"/> constants instead of magic strings.
/// </para>
/// <para>
/// <b>Stock receiving:</b> When the AdminSystem marks a PO as Received,
/// <c>SupplierRepository.ReceiveStockAsync</c> updates
/// <c>ProductVariant.StockQuantity</c> and writes
/// <c>InventoryLog</c> Purchase entries for each received item —
/// all within a single DB transaction.
/// </para>
/// </summary>
public sealed class PurchaseOrder
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int PurchaseOrderId { get; set; }

    /// <summary>FK to the supplier this order was sent to.</summary>
    public int SupplierId { get; set; }

    /// <summary>UTC timestamp when the purchase order was created.</summary>
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Expected delivery date agreed with the supplier.
    /// NULL when no delivery date has been confirmed.
    /// </summary>
    public DateTime? ExpectedDeliveryDate { get; set; }

    /// <summary>
    /// Current status of the purchase order.
    /// Constrained by CK_PurchaseOrder_Status: Pending, Received, Cancelled.
    /// Use <see cref="PurchaseOrderStatuses"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Status { get; set; } = PurchaseOrderStatuses.Pending;

    /// <summary>
    /// FK to the admin user who created this purchase order.
    /// NULL for system-generated purchase orders.
    /// </summary>
    public int? CreatedByUserId { get; set; }

    /// <summary>UTC timestamp when this purchase order row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The supplier this purchase order was sent to.</summary>
    public Supplier Supplier { get; set; } = null!;

    /// <summary>
    /// The admin who created this purchase order.
    /// NULL for system-generated orders.
    /// </summary>
    public User? CreatedBy { get; set; }

    /// <summary>
    /// All line items in this purchase order.
    /// UnitPrice is locked at PO creation time for each item.
    /// Cascade-deleted when the purchase order is deleted.
    /// </summary>
    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();

    /// <summary>
    /// Inventory log entries written when this PO's stock is received.
    /// ChangeType = Purchase per item, written by
    /// <c>SupplierRepository.ReceiveStockAsync</c>.
    /// </summary>
    public ICollection<InventoryLog> InventoryLogs { get; set; } = new List<InventoryLog>();
}

/// <summary>
/// Compile-time constants for all valid purchase order status values.
/// Mirrors the CK_PurchaseOrder_Status CHECK constraint in the database.
/// </summary>
public static class PurchaseOrderStatuses
{
    /// <summary>PO has been created and sent to the supplier — awaiting delivery.</summary>
    public const string Pending = "Pending";

    /// <summary>Stock has been received and inventory has been updated.</summary>
    public const string Received = "Received";

    /// <summary>PO was cancelled before stock was received.</summary>
    public const string Cancelled = "Cancelled";
}