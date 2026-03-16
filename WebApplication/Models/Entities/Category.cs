// WebApplication/Models/Entities/Category.cs

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a product category with optional parent-child hierarchy.
/// Top-level categories have <see cref="ParentCategoryId"/> = NULL.
/// Child categories point to a parent via <see cref="ParentCategoryId"/>.
/// <para>
/// Valid category codes (enforced by UQ_Category_Code):
/// UNIT, FRAME, FORK, HUB, UPGKIT, STEM, HBAR, SADDLE, GRIP, PEDAL, RIM, TIRE, CHAIN.
/// </para>
/// <para>
/// The self-referencing FK (<see cref="ParentCategoryId"/> → <see cref="CategoryId"/>)
/// is configured in AppDbContext via Fluent API with no cascade delete to prevent
/// accidental tree destruction.
/// </para>
/// </summary>
public sealed class Category
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int CategoryId { get; set; }

    /// <summary>
    /// Short unique code identifying the category (e.g. UNIT, FRAME, FORK).
    /// Enforced unique via UQ_Category_Code in the database.
    /// Use <see cref="CategoryCodes"/> constants instead of magic strings.
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>Display name shown in the product catalog and filter sidebar.</summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>Optional description of what products belong in this category.</summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// FK to the parent category. NULL for top-level categories.
    /// Configured in AppDbContext as a self-referencing relationship with
    /// no cascade delete.
    /// </summary>
    public int? ParentCategoryId { get; set; }

    /// <summary>
    /// Whether this category is active and visible in the product catalog.
    /// Inactive categories are excluded from filter dropdowns and product listings.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Sort order for display in the UI. Lower values appear first.
    /// Managed by AdminSystem.
    /// </summary>
    public int DisplayOrder { get; set; } = 0;

    // -------------------------------------------------------------------------
    // Navigation properties — self-referencing hierarchy configured via
    // Fluent API in AppDbContext.OnModelCreating.
    // -------------------------------------------------------------------------

    /// <summary>
    /// The parent category of this category.
    /// NULL for top-level categories.
    /// </summary>
    public Category? ParentCategory { get; set; }

    /// <summary>
    /// Direct child categories nested under this category.
    /// Empty collection for leaf-level categories.
    /// </summary>
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();

    /// <summary>All products assigned to this category.</summary>
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

/// <summary>
/// Compile-time constants for all valid category code values.
/// Mirrors the UQ_Category_Code unique constraint values in the database.
/// Use these instead of magic strings when filtering or comparing categories.
/// </summary>
public static class CategoryCodes
{
    /// <summary>Complete bicycle unit.</summary>
    public const string Unit = "UNIT";

    /// <summary>Bicycle frame.</summary>
    public const string Frame = "FRAME";

    /// <summary>Fork component.</summary>
    public const string Fork = "FORK";

    /// <summary>Hub component.</summary>
    public const string Hub = "HUB";

    /// <summary>Upgrade kit.</summary>
    public const string UpgradeKit = "UPGKIT";

    /// <summary>Stem component.</summary>
    public const string Stem = "STEM";

    /// <summary>Handlebar.</summary>
    public const string Handlebar = "HBAR";

    /// <summary>Saddle / seat.</summary>
    public const string Saddle = "SADDLE";

    /// <summary>Grip component.</summary>
    public const string Grip = "GRIP";

    /// <summary>Pedal component.</summary>
    public const string Pedal = "PEDAL";

    /// <summary>Rim component.</summary>
    public const string Rim = "RIM";

    /// <summary>Tire.</summary>
    public const string Tire = "TIRE";

    /// <summary>Chain component.</summary>
    public const string Chain = "CHAIN";
}