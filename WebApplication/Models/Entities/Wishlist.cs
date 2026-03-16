// WebApplication/Models/Entities/Wishlist.cs

namespace WebApplication.Models.Entities;

/// <summary>
/// Represents a product saved to a customer's wishlist.
/// Typically used when a product is out of stock — the customer saves it and
/// receives a WishlistRestock notification when stock is restored.
/// <para>
/// <b>Uniqueness rule:</b> A user can save a product to their wishlist only once.
/// Enforced by UX_Wishlist_UserProduct unique index on (UserId, ProductId).
/// <c>WishlistService.AddAsync</c> handles this idempotently — duplicate add
/// attempts are silently ignored.
/// </para>
/// <para>
/// <b>Stock monitor integration:</b> <c>StockMonitorJob</c> reads wishlist rows
/// via <c>WishlistRepository.GetUserIdsForProductAsync</c> to find which customers
/// should receive a WishlistRestock notification when a variant's stock is restored
/// above its ReorderThreshold.
/// </para>
/// </summary>
public sealed class Wishlist
{
    /// <summary>Primary key — auto-increment identity.</summary>
    public int WishlistId { get; set; }

    /// <summary>
    /// FK to the user who saved this product.
    /// Configured with CASCADE DELETE — removing a user removes all their
    /// wishlist entries.
    /// </summary>
    public int UserId { get; set; }

    /// <summary>FK to the product that was saved.</summary>
    public int ProductId { get; set; }

    /// <summary>UTC timestamp when this product was added to the wishlist.</summary>
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;

    // -------------------------------------------------------------------------
    // Navigation properties
    // -------------------------------------------------------------------------

    /// <summary>The user who saved this product.</summary>
    public User User { get; set; } = null!;

    /// <summary>The product that was saved to the wishlist.</summary>
    public Product Product { get; set; } = null!;
}