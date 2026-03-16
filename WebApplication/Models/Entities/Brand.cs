// WebApplication/Models/Entities/Brand.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a bicycle or parts manufacturer. Brands are reference data managed
/// exclusively through the AdminSystem. The WebApplication reads brands for product
/// filter dropdowns and detail pages.
/// </summary>
public sealed class Brand
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int BrandId { get; set; }

    /// <summary>
    /// Unique brand display name.
    /// Enforced unique via UQ_Brand_Name in the database.
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string BrandName { get; set; } = string.Empty;

    /// <summary>Country of origin for this brand. Optional.</summary>
    [MaxLength(100)]
    public string? Country { get; set; }

    /// <summary>Brand's official website URL. Optional.</summary>
    [MaxLength(255)]
    public string? Website { get; set; }

    /// <summary>Optional description of the brand.</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// Whether this brand is currently active and visible in the product catalog.
    /// Inactive brands are hidden from filters and product listings.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>UTC timestamp when this brand row was created.</summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>
    /// All products associated with this brand.
    /// A product's BrandId is nullable — unbranded products have no Brand row.
    /// </summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}