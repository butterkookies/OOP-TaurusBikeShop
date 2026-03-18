// WebApplication/DataAccess/Repositories/WishlistRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Wishlist"/> entities.
/// Supports wishlist display, idempotent add, and stock monitor job queries.
/// </summary>
public sealed class WishlistRepository : Repository<Wishlist>
{
    /// <inheritdoc/>
    public WishlistRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns all wishlist entries for a user with product and primary image loaded.
    /// Used to render the Wishlist page.
    /// </summary>
    /// <param name="userId">The user whose wishlist to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Wishlist entries with product and primary image included.</returns>
    public async Task<IReadOnlyList<Wishlist>> GetByUserWithProductAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Wishlists
            .AsNoTracking()
            .Include(w => w.Product)
                .ThenInclude(p => p.Images.Where(i => i.IsPrimary))
            .Include(w => w.Product)
                .ThenInclude(p => p.Variants.Where(v => v.IsActive))
            .Where(w => w.UserId == userId)
            .OrderByDescending(w => w.AddedAt)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Adds a product to a user's wishlist idempotently.
    /// If the entry already exists (unique index violation), the exception is
    /// silently caught and the operation succeeds without error.
    /// </summary>
    /// <param name="userId">The user adding the product.</param>
    /// <param name="productId">The product to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task AddIdempotentAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        bool alreadyExists = await Context.Wishlists
            .AnyAsync(
                w => w.UserId == userId && w.ProductId == productId,
                cancellationToken);

        if (alreadyExists)
            return;

        Wishlist entry = new()
        {
            UserId = userId,
            ProductId = productId,
            AddedAt = DateTime.UtcNow
        };

        try
        {
            await Context.Wishlists.AddAsync(entry, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            // Swallow unique constraint violations from a concurrent add.
            // The item is already in the wishlist — the outcome is correct.
        }
    }

    /// <summary>
    /// Returns the product IDs saved in a user's wishlist.
    /// Used by <c>ProductController</c> to set the <c>IsInWishlist</c> flag
    /// on product listing cards without loading full product data.
    /// </summary>
    /// <param name="userId">The user whose wishlist product IDs to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A set of product IDs in the user's wishlist.</returns>
    public async Task<IReadOnlyList<int>> GetProductIdsForUserAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Wishlists
            .AsNoTracking()
            .Where(w => w.UserId == userId)
            .Select(w => w.ProductId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the IDs of all users who have wishlisted a specific product.
    /// Used by <c>StockMonitorJob</c> to find customers to notify when
    /// a product's stock is restored above its reorder threshold.
    /// </summary>
    /// <param name="productId">The product that came back in stock.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>User IDs of all customers who wishlisted this product.</returns>
    public async Task<IReadOnlyList<int>> GetUserIdsForProductAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Wishlists
            .AsNoTracking()
            .Where(w => w.ProductId == productId)
            .Select(w => w.UserId)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the count of products in a user's wishlist.
    /// Used to populate the wishlist badge count in the navbar.
    /// </summary>
    /// <param name="userId">The user ID to count for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of products in the user's wishlist.</returns>
    public async Task<int> GetWishlistCountAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Wishlists
            .CountAsync(w => w.UserId == userId, cancellationToken);
    }
}