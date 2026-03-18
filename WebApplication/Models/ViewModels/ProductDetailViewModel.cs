// WebApplication/Models/ViewModels/ProductDetailViewModel.cs

using WebApplication.Models.Entities;

namespace WebApplication.Models.ViewModels;

/// <summary>
/// Rich view model for the product detail page.
/// Populated by <c>ProductService.GetByIdAsync</c>.
/// </summary>
public sealed class ProductDetailViewModel
{
    // -------------------------------------------------------------------------
    // Core product data
    // -------------------------------------------------------------------------

    /// <summary>The product's primary key.</summary>
    public int ProductId { get; set; }

    /// <summary>Full product display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Full product description for the detail page.</summary>
    public string? Description { get; set; }

    /// <summary>Short description used in meta tags and breadcrumb subtitles.</summary>
    public string? ShortDescription { get; set; }

    /// <summary>Base retail price.</summary>
    public decimal Price { get; set; }

    /// <summary>Formatted base price (e.g. ₱12,500.00).</summary>
    public string FormattedPrice => $"₱{Price:N2}";

    /// <summary>Brand name, or null for unbranded products.</summary>
    public string? Brand { get; set; }

    /// <summary>Category display name.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Category code for breadcrumb links.</summary>
    public string CategoryCode { get; set; } = string.Empty;

    // -------------------------------------------------------------------------
    // Specifications
    // -------------------------------------------------------------------------

    public string? Material          { get; set; }
    public string? Color             { get; set; }
    public string? WheelSize         { get; set; }
    public string? SpeedCompatibility{ get; set; }
    public bool?   BoostCompatible   { get; set; }
    public bool?   TubelessReady     { get; set; }
    public string? AxleStandard      { get; set; }
    public string? SuspensionTravel  { get; set; }
    public string? BrakeType         { get; set; }
    public string? AdditionalSpecs   { get; set; }

    // -------------------------------------------------------------------------
    // Variants, images, reviews
    // -------------------------------------------------------------------------

    /// <summary>All active variants — drives the variant selector on the detail page.</summary>
    public IReadOnlyList<ProductVariant> Variants { get; set; } = [];

    /// <summary>
    /// All product images ordered: primary first, then by DisplayOrder.
    /// First element is the main gallery image.
    /// </summary>
    public IReadOnlyList<ProductImage> Images { get; set; } = [];

    /// <summary>First page of reviews for initial render. More loaded via AJAX.</summary>
    public IReadOnlyList<ProductReviewViewModel> Reviews { get; set; } = [];

    /// <summary>Average rating across all reviews, rounded to 1 decimal.</summary>
    public decimal AverageRating { get; set; }

    /// <summary>Total review count for pagination.</summary>
    public int ReviewCount { get; set; }

    // -------------------------------------------------------------------------
    // State flags
    // -------------------------------------------------------------------------

    /// <summary>
    /// The variant ID selected for add-to-cart.
    /// Defaults to the first active variant's ID.
    /// Updated by JavaScript when the customer selects a different variant.
    /// </summary>
    public int? SelectedVariantId { get; set; }

    /// <summary>
    /// <c>true</c> when the authenticated user has this product in their wishlist.
    /// </summary>
    public bool IsInWishlist { get; set; }

    /// <summary>
    /// <c>true</c> when the authenticated user has a Delivered order
    /// containing this product — they are eligible to submit a review.
    /// </summary>
    public bool CanReview { get; set; }

    /// <summary>
    /// Primary image URL shortcut for &lt;meta og:image&gt; and thumbnails.
    /// </summary>
    public string? PrimaryImageUrl =>
        Images.FirstOrDefault(i => i.IsPrimary)?.ImageUrl
        ?? Images.FirstOrDefault()?.ImageUrl;

    /// <summary>
    /// Total stock across all active variants.
    /// </summary>
    public int TotalStock => Variants.Sum(v => v.StockQuantity);

    /// <summary>Convenience: out of stock when total stock is zero.</summary>
    public bool IsOutOfStock => TotalStock <= 0;
}