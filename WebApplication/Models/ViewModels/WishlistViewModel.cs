// WebApplication/Models/ViewModels/WishlistViewModel.cs

namespace WebApplication.Models.ViewModels;

/// <summary>
/// View model for the customer wishlist page.
/// Populated by <c>WishlistService.GetWishlistAsync</c>.
/// </summary>
public sealed class WishlistViewModel
{
    /// <summary>All products currently saved in the user's wishlist.</summary>
    public IReadOnlyList<WishlistItemViewModel> Items { get; set; } = [];

    /// <summary>Convenience: true when the wishlist has no items.</summary>
    public bool IsEmpty => Items.Count == 0;

    /// <summary>Total number of wishlisted products.</summary>
    public int ItemCount => Items.Count;
}

/// <summary>
/// Represents a single product saved in the wishlist.
/// </summary>
public sealed class WishlistItemViewModel
{
    /// <summary>Wishlist row ID — used in remove AJAX calls.</summary>
    public int WishlistId { get; set; }

    /// <summary>The product's primary key.</summary>
    public int ProductId { get; set; }

    /// <summary>Product display name.</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Short description shown beneath the name.</summary>
    public string? ShortDescription { get; set; }

    /// <summary>Current base retail price.</summary>
    public decimal Price { get; set; }

    /// <summary>Formatted price string (e.g. ₱12,500.00).</summary>
    public string FormattedPrice => $"₱{Price:N2}";

    /// <summary>Brand name, or null for unbranded products.</summary>
    public string? Brand { get; set; }

    /// <summary>Primary image CDN URL. Null when no image has been uploaded.</summary>
    public string? PrimaryImageUrl { get; set; }

    /// <summary>
    /// Total available stock across all active variants.
    /// Zero means the product is currently out of stock.
    /// </summary>
    public int StockQuantity { get; set; }

    /// <summary>Convenience: true when stock is zero.</summary>
    public bool IsOutOfStock => StockQuantity <= 0;

    /// <summary>UTC timestamp when the product was added to the wishlist.</summary>
    public DateTime AddedAt { get; set; }
}