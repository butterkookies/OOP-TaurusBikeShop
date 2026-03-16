// WebApplication/Models/Entities/ProductVariant.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a stock and pricing variant of a product.
/// This is the single source of truth for inventory in the Taurus system (v7.0+).
/// <para>
/// <b>Stock rule:</b> All stock lives exclusively in <see cref="StockQuantity"/>.
/// There is no StockQuantity on the Product table. Every stock change — purchase,
/// sale, adjustment, lock, unlock — must update this column and write a corresponding
/// <c>InventoryLog</c> row.
/// </para>
/// <para>
/// <b>Default variant:</b> Products with no real variants (e.g. a single-size part)
/// must still have exactly one ProductVariant row with <c>VariantName = "Default"</c>
/// and <c>AdditionalPrice = 0</c>.
/// </para>
/// <para>
/// <b>Computed total price:</b> The effective selling price of a variant is
/// <c>Product.Price + AdditionalPrice</c>, computed in <c>vw_ProductVariantDetails</c>.
/// It is never stored here.
/// </para>
/// <para>
/// <b>Low-stock alert (v7.1):</b> <see cref="ReorderThreshold"/> is polled every
/// 15 minutes by <c>StockMonitorJob</c>. When <see cref="StockQuantity"/> drops
/// below this threshold, a LowStockAlert notification is queued for admin and a
/// WishlistRestock notification is sent to matching wishlist customers.
/// </para>
/// </summary>
public sealed class ProductVariant
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int ProductVariantId { get; set; }

    /// <summary>
    /// FK to the parent product. Configured with CASCADE DELETE — removing a product
    /// removes all its variants.
    /// </summary>
    public int ProductId { get; set; }

    /// <summary>
    /// Display name of this variant (e.g. Small, Medium, Large, Default).
    /// Use "Default" for products with no real size or colour variants.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string VariantName { get; set; } = string.Empty;

    /// <summary>
    /// Optional variant-level SKU for barcode scanning in the POS system.
    /// Not unique — multiple variants may share a SKU in edge cases.
    /// </summary>
    [MaxLength(50)]
    public string? SKU { get; set; }

    /// <summary>
    /// Price premium added to the base <c>Product.Price</c> for this variant.
    /// Must be &gt;= 0. Zero for the Default variant and for same-price variants.
    /// TotalPrice = Product.Price + AdditionalPrice (computed, never stored).
    /// </summary>
    public decimal AdditionalPrice { get; set; } = 0m;

    /// <summary>
    /// Current available stock quantity for this variant.
    /// Must be &gt;= 0. This is the only authoritative stock source in the system.
    /// Never update this directly — always go through <c>InventoryService.AdjustStockAsync</c>
    /// which also writes the corresponding <c>InventoryLog</c> row.
    /// </summary>
    public int StockQuantity { get; set; } = 0;

    /// <summary>
    /// Stock level below which a low-stock alert is triggered (v7.1).
    /// When <see cref="StockQuantity"/> &lt; <see cref="ReorderThreshold"/>,
    /// <c>StockMonitorJob</c> queues a LowStockAlert to admin and WishlistRestock
    /// notifications to matching wishlist customers.
    /// Must be &gt;= 0. Default is 5.
    /// </summary>
    public int ReorderThreshold { get; set; } = 5;

    /// <summary>
    /// Whether this variant is active and available for purchase.
    /// Inactive variants are hidden from the product detail page and cannot
    /// be added to cart.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>UTC timestamp when this variant row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last update to this variant record.
    /// Set by application code on every update.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The parent product this variant belongs to.</summary>
    public Product Product { get; set; } = null!;
}