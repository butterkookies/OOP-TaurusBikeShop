// WebApplication/Models/Entities/Product.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Core product master record for the Taurus Bike Shop catalog.
/// Represents a single sellable product — a complete bike unit, frame, component, or part.
/// <para>
/// <b>Stock rule (v7.0+):</b> There is NO StockQuantity column on this table.
/// All stock lives exclusively in <see cref="ProductVariant.StockQuantity"/>.
/// Every product must have at least one <see cref="ProductVariant"/> row.
/// Products with no real variants use <c>VariantName = "Default"</c>.
/// </para>
/// <para>
/// <b>Price audit:</b> The DB trigger <c>TR_Product_PriceAudit</c> fires AFTER UPDATE
/// and automatically inserts a <see cref="PriceHistory"/> row whenever
/// <see cref="Price"/> changes. Application code must never write to PriceHistory directly.
/// </para>
/// <para>
/// <b>Computed fields:</b> TotalPrice per variant is computed as
/// <c>Product.Price + ProductVariant.AdditionalPrice</c> in <c>vw_ProductVariantDetails</c>.
/// It is never stored on either table.
/// </para>
/// </summary>
public sealed class Product
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int ProductId { get; set; }

    /// <summary>FK to the category this product belongs to.</summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// FK to the brand of this product. NULL for unbranded products.
    /// </summary>
    public int? BrandId { get; set; }

    /// <summary>
    /// Optional product-level SKU. Unique when non-NULL, enforced via
    /// UX_Product_SKU filtered index in the database.
    /// </summary>
    [MaxLength(50)]
    public string? SKU { get; set; }

    /// <summary>Full product display name shown in the catalog and detail page.</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Short description used in product listing cards.
    /// Kept under 300 characters for consistent card layout.
    /// </summary>
    [MaxLength(300)]
    public string? ShortDescription { get; set; }

    /// <summary>Full product description shown on the detail page. Supports long-form text.</summary>
    public string? Description { get; set; }

    /// <summary>
    /// Base retail price in the currency specified by <see cref="Currency"/>.
    /// Must be &gt;= 0. Variant-level total price = Price + AdditionalPrice,
    /// computed in <c>vw_ProductVariantDetails</c>.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// ISO 4217 currency code for this product's price.
    /// Constrained by CK_Product_Currency: PHP, USD, or EUR.
    /// Use <see cref="CurrencyCodes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(3)]
    public string Currency { get; set; } = CurrencyCodes.PHP;

    /// <summary>Primary material (e.g. Aluminum, Carbon, Steel). Optional.</summary>
    [MaxLength(100)]
    public string? Material { get; set; }

    /// <summary>Product colour description. Optional.</summary>
    [MaxLength(100)]
    public string? Color { get; set; }

    /// <summary>Wheel size specification (e.g. 29", 27.5", 700c). Optional.</summary>
    [MaxLength(20)]
    public string? WheelSize { get; set; }

    /// <summary>Speed compatibility (e.g. 12-speed, 11-speed). Optional.</summary>
    [MaxLength(50)]
    public string? SpeedCompatibility { get; set; }

    /// <summary>
    /// Whether this product is Boost-compatible.
    /// NULL means the specification is not applicable for this product type.
    /// </summary>
    public bool? BoostCompatible { get; set; }

    /// <summary>
    /// Whether this product supports tubeless setup.
    /// NULL means not applicable.
    /// </summary>
    public bool? TubelessReady { get; set; }

    /// <summary>Axle standard (e.g. Boost 148, QR, Thru-Axle 15mm). Optional.</summary>
    [MaxLength(50)]
    public string? AxleStandard { get; set; }

    /// <summary>Suspension travel specification (e.g. 140mm, 120mm). Optional.</summary>
    [MaxLength(50)]
    public string? SuspensionTravel { get; set; }

    /// <summary>Brake type (e.g. Hydraulic Disc, Mechanical Disc, Rim). Optional.</summary>
    [MaxLength(100)]
    public string? BrakeType { get; set; }

    /// <summary>
    /// Free-text overflow field for additional technical specifications
    /// that don't fit the structured columns above.
    /// </summary>
    [MaxLength(1000)]
    public string? AdditionalSpecs { get; set; }

    /// <summary>
    /// Whether this product is active and visible in the catalog.
    /// Inactive products are excluded from all public-facing queries.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Whether this product should appear in the homepage featured section.
    /// Managed by AdminSystem.
    /// </summary>
    public bool IsFeatured { get; set; } = false;

    /// <summary>UTC timestamp when this product row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// UTC timestamp of the last update to this product record.
    /// Set by application code on every update — not a DB-managed column.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The category this product belongs to.</summary>
    public Category Category { get; set; } = null!;

    /// <summary>
    /// The brand of this product. NULL for unbranded products.
    /// </summary>
    public Brand? Brand { get; set; }

    /// <summary>
    /// All stock and price variants for this product.
    /// Every product must have at least one row here.
    /// Products with no real variants use VariantName = "Default".
    /// </summary>
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();

    /// <summary>
    /// All GCS image metadata rows for this product.
    /// At most one image per product may have IsPrimary = true,
    /// enforced by UX_ProductImage_Primary filtered index.
    /// </summary>
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

    /// <summary>
    /// Immutable price change history. Populated automatically by the
    /// TR_Product_PriceAudit database trigger — never written by application code.
    /// </summary>
    public ICollection<PriceHistory> PriceHistory { get; set; } = new List<PriceHistory>();
}

/// <summary>
/// Compile-time constants for all valid product currency code values.
/// Mirrors the CK_Product_Currency CHECK constraint in the database.
/// </summary>
public static class CurrencyCodes
{
    /// <summary>Philippine Peso — default currency for all products.</summary>
    public const string PHP = "PHP";

    /// <summary>US Dollar.</summary>
    public const string USD = "USD";

    /// <summary>Euro.</summary>
    public const string EUR = "EUR";
}