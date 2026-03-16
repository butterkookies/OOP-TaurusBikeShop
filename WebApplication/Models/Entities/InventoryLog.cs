// WebApplication/Models/Entities/InventoryLog.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Immutable audit trail of every stock movement in the system.
/// Every change to <c>ProductVariant.StockQuantity</c> — regardless of the source
/// (order, POS sale, manual adjustment, purchase order receipt, damage) —
/// must produce exactly one InventoryLog row written in the same transaction.
/// <para>
/// <b>Immutability contract:</b> Rows in this table must NEVER be updated or
/// deleted. They are an append-only audit log. Any correction to a prior entry
/// is made by writing a new offsetting entry.
/// </para>
/// <para>
/// <b>Lock/Unlock pattern for online orders:</b>
/// <list type="bullet">
///   <item>Order placed → Lock entry (<see cref="ChangeQuantity"/> is negative,
///     stock is reserved)</item>
///   <item>Order delivered → Unlock entry + Sale entry (stock fully consumed)</item>
///   <item>Order cancelled → Unlock entry (stock restored)</item>
/// </list>
/// All three writes happen within the same DB transaction as the Order status change.
/// </para>
/// <para>
/// <b>Change types:</b> Use <see cref="InventoryChangeTypes"/> constants
/// instead of magic strings for <see cref="ChangeType"/>.
/// </para>
/// </summary>
public sealed class InventoryLog
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int InventoryLogId { get; set; }

    /// <summary>FK to the product whose stock changed.</summary>
    public int ProductId { get; set; }

    /// <summary>
    /// FK to the specific variant whose stock changed.
    /// NULL for base product-level adjustments not tied to a specific variant.
    /// </summary>
    public int? ProductVariantId { get; set; }

    /// <summary>
    /// The quantity change applied to <c>ProductVariant.StockQuantity</c>.
    /// Positive (+) = stock added (Purchase, Unlock, Return, Adjustment up).
    /// Negative (-) = stock removed (Sale, Lock, Damage, Loss, Adjustment down).
    /// </summary>
    public int ChangeQuantity { get; set; }

    /// <summary>
    /// The type of stock movement. Constrained by CK_InvLog_ChangeType to 8 values.
    /// Use <see cref="InventoryChangeTypes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string ChangeType { get; set; } = string.Empty;

    /// <summary>
    /// FK to the order that triggered this stock movement.
    /// Set for Sale, Lock, and Unlock entries.
    /// NULL for Purchase, Adjustment, Damage, Loss entries.
    /// </summary>
    public int? OrderId { get; set; }

    /// <summary>
    /// FK to the purchase order that triggered this stock movement.
    /// Set for Purchase entries only.
    /// NULL for all other change types.
    /// </summary>
    public int? PurchaseOrderId { get; set; }

    /// <summary>
    /// FK to the user who triggered this stock movement.
    /// NULL for system-generated entries (background jobs, automatic order processing).
    /// </summary>
    public int? ChangedByUserId { get; set; }

    /// <summary>
    /// Optional notes describing the reason for this stock movement.
    /// Required for Adjustment, Damage, and Loss entries — should explain why.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    /// <summary>UTC timestamp when this log entry was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The product whose stock changed.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The specific variant whose stock changed.
    /// NULL for product-level entries.
    /// </summary>
    public ProductVariant? Variant { get; set; }

    /// <summary>
    /// The order that triggered this movement.
    /// NULL for non-order-related entries.
    /// </summary>
    public Order? Order { get; set; }

    /// <summary>
    /// The purchase order that triggered this movement.
    /// NULL for non-PO entries.
    /// </summary>
    public PurchaseOrder? PurchaseOrder { get; set; }

    /// <summary>
    /// The user who triggered this movement.
    /// NULL for automated/system entries.
    /// </summary>
    public User? ChangedBy { get; set; }
}

/// <summary>
/// Compile-time constants for all valid inventory change type values.
/// Mirrors the CK_InvLog_ChangeType CHECK constraint in the database (8 values).
/// Use these instead of magic strings in all services and background jobs
/// that write InventoryLog rows.
/// </summary>
public static class InventoryChangeTypes
{
    /// <summary>
    /// Stock received from a supplier purchase order.
    /// ChangeQuantity is positive. PurchaseOrderId must be set.
    /// </summary>
    public const string Purchase = "Purchase";

    /// <summary>
    /// Stock consumed by a confirmed sale.
    /// ChangeQuantity is negative. OrderId must be set.
    /// Written at order delivery confirmation alongside an Unlock entry.
    /// </summary>
    public const string Sale = "Sale";

    /// <summary>
    /// Stock returned from a customer.
    /// ChangeQuantity is positive.
    /// </summary>
    public const string Return = "Return";

    /// <summary>
    /// Manual stock correction by an admin.
    /// Can be positive or negative. Notes field should explain the reason.
    /// </summary>
    public const string Adjustment = "Adjustment";

    /// <summary>
    /// Stock removed due to damage.
    /// ChangeQuantity is negative. Notes field should describe the damage.
    /// </summary>
    public const string Damage = "Damage";

    /// <summary>
    /// Stock removed due to unexplained loss or theft.
    /// ChangeQuantity is negative. Notes field should describe the circumstances.
    /// </summary>
    public const string Loss = "Loss";

    /// <summary>
    /// Stock reserved (soft-locked) when an online order is placed.
    /// ChangeQuantity is negative. OrderId must be set.
    /// Reversed by an Unlock entry if the order is cancelled.
    /// </summary>
    public const string Lock = "Lock";

    /// <summary>
    /// Reserved stock released when an order is cancelled or delivered.
    /// ChangeQuantity is positive. OrderId must be set.
    /// For delivered orders, an Unlock entry is paired with a Sale entry.
    /// </summary>
    public const string Unlock = "Unlock";
}