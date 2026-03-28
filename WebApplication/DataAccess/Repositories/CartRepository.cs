// WebApplication/DataAccess/Repositories/CartRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Cart"/> and <see cref="CartItem"/> entities.
/// Supports both authenticated user carts and anonymous guest session carts.
/// </summary>
public sealed class CartRepository : Repository<Cart>
{
    /// <inheritdoc/>
    public CartRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns the active (not checked out) cart for a registered user
    /// or a guest session, with all items and their product/variant details loaded.
    /// Exactly one of <paramref name="userId"/> or <paramref name="guestSessionId"/>
    /// must be provided.
    /// </summary>
    /// <param name="userId">The registered user's ID. NULL for guest carts.</param>
    /// <param name="guestSessionId">The guest session ID. NULL for user carts.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The active cart with items included, or <c>null</c> if no active cart exists.
    /// </returns>
    public async Task<Cart?> GetActiveCartAsync(
        int? userId,
        int? guestSessionId,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Cart> query = Context.Carts
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Product)
                    .ThenInclude(p => p!.Images)
            .Include(c => c.Items)
                .ThenInclude(ci => ci.Variant)
            .Where(c => !c.IsCheckedOut);

        if (userId.HasValue)
            query = query.Where(c => c.UserId == userId.Value);
        else if (guestSessionId.HasValue)
            query = query.Where(c => c.GuestSessionId == guestSessionId.Value);
        else
            return null;

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Returns the total number of items (sum of quantities) in the specified cart.
    /// Used to update the cart badge count in the navbar without loading full cart details.
    /// </summary>
    /// <param name="cartId">The cart ID to count items for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The sum of all <c>CartItem.Quantity</c> values in the cart, or 0 if empty.
    /// </returns>
    public async Task<int> GetCartItemCountAsync(
        int cartId,
        CancellationToken cancellationToken = default)
    {
        return await Context.CartItems
            .Where(ci => ci.CartId == cartId)
            .SumAsync(ci => ci.Quantity, cancellationToken);
    }

    /// <summary>
    /// Returns a single cart item by its ID, with product and variant loaded.
    /// Used by update-quantity and remove-item operations.
    /// </summary>
    /// <param name="cartItemId">The cart item ID to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The cart item with product/variant included, or <c>null</c>.</returns>
    public async Task<CartItem?> GetCartItemAsync(
        int cartItemId,
        CancellationToken cancellationToken = default)
    {
        return await Context.CartItems
            .Include(ci => ci.Product)
            .Include(ci => ci.Variant)
            .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId, cancellationToken);
    }
}