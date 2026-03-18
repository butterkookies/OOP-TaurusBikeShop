// WebApplication/Models/ViewModels/ProductViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// Lightweight product model for listing cards in the catalog grid.
/// Populated by <c>ProductService.GetFilteredAsync</c> and
/// <c>ProductService.GetFeaturedAsync</c>.
/// </summary>
public sealed class ProductViewModel
{
    /// <summary>The product's primary key.</summary>
    public int ProductId { get; set; }

    /// <summary>Full product display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Short description shown beneath the product name on listing cards.</summary>
    public string? ShortDescription { get; set; }

    /// <summary>Base retail price before any variant additional price.</summary>
    public decimal Price { get; set; }

    /// <summary>Formatted price string for display (e.g. ₱12,500.00).</summary>
    public string FormattedPrice => $"₱{Price:N2}";

    /// <summary>Brand name, or null for unbranded products.</summary>
    public string? Brand { get; set; }

    /// <summary>Category display name.</summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>Category code used for filtering (e.g. UNIT, FRAME).</summary>
    public string CategoryCode { get; set; } = string.Empty;

    /// <summary>
    /// Public CDN URL of the product's primary image.
    /// Null when no image has been uploaded — views should show a placeholder.
    /// </summary>
    public string? PrimaryImageUrl { get; set; }

    /// <summary>
    /// Total available stock aggregated across all active variants.
    /// Zero means the product is out of stock.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>
    /// <c>true</c> when the authenticated user has this product in their wishlist.
    /// Always <c>false</c> for unauthenticated users.
    /// </summary>
    public bool IsInWishlist { get; set; }

    /// <summary><c>true</c> when this product is flagged for homepage featuring.</summary>
    public bool IsFeatured { get; set; }

    /// <summary>Convenience property — true when StockQuantity is zero.</summary>
    public bool IsOutOfStock => StockQuantity <= 0;

    /// <summary>Convenience property — true when stock is low but not zero.</summary>
    public bool IsLowStock => StockQuantity > 0 && StockQuantity <= 5;
}