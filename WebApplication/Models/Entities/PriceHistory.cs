// WebApplication/Models/Entities/PriceHistory.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Immutable price change audit log for products.
/// Rows are written exclusively by the database trigger
/// <c>TR_Product_PriceAudit</c> which fires AFTER UPDATE on the Product table
/// whenever <c>Product.Price</c> changes.
/// <para>
/// <b>Application code must never insert, update, or delete rows in this table.</b>
/// Any attempt to do so directly bypasses the audit mechanism and violates the
/// immutability contract. Reads are permitted for admin reporting only.
/// </para>
/// <para>
/// When the trigger fires, <see cref="ChangedByUserId"/> is NULL because the
/// trigger has no session context. If an admin manually triggers a price update
/// through the AdminSystem, the AdminSystem should write its own audit note to
/// <c>SystemLog</c> with the <c>ProductUpdate</c> event type.
/// </para>
/// </summary>
public sealed class PriceHistory
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int PriceHistoryId { get; set; }

    /// <summary>FK to the product whose price changed.</summary>
    public int ProductId { get; set; }

    /// <summary>The product's price immediately before this change.</summary>
    public decimal OldPrice { get; set; }

    /// <summary>The product's price immediately after this change.</summary>
    public decimal NewPrice { get; set; }

    /// <summary>UTC timestamp when this price change was recorded by the trigger.</summary>
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// FK to the user who triggered the price change.
    /// Always NULL for trigger-generated rows — the DB trigger has no user context.
    /// </summary>
    public int? ChangedByUserId { get; set; }

    /// <summary>
    /// Notes about the price change. For trigger-generated rows this is always
    /// "Automatic price change audit". For manually created rows (if ever needed)
    /// this should describe the reason.
    /// </summary>
    [MaxLength(500)]
    public string? Notes { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The product whose price changed.</summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// The user who triggered the change.
    /// NULL for all trigger-generated rows.
    /// </summary>
    public User? ChangedBy { get; set; }
}