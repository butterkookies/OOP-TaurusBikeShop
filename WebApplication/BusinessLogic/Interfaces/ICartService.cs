// WebApplication/BusinessLogic/Interfaces/ICartService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for shopping cart operations.
/// Supports both authenticated user carts and anonymous guest session carts.
/// Flowchart: Part 3 — Cart &amp; Checkout.
/// </summary>
public interface ICartService
{
    /// <summary>
    /// Returns the cart view model for the given user or guest session.
    /// Creates a new empty cart if none exists.
    /// </summary>
    /// <param name="userId">Authenticated user ID. Null for guests.</param>
    /// <param name="guestSessionId">Guest session ID. Null for authenticated users.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<CartViewModel> GetCartAsync(
        int? userId,
        int? guestSessionId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a product/variant to the cart, or increments the quantity if
    /// the same variant is already present.
    /// Validates stock availability before adding.
    /// </summary>
    /// <param name="userId">Authenticated user ID. Null for guests.</param>
    /// <param name="guestSessionId">Guest session ID. Null for authenticated users.</param>
    /// <param name="productId">The product to add.</param>
    /// <param name="variantId">The specific variant. Null uses the Default variant.</param>
    /// <param name="qty">Quantity to add. Must be &gt;= 1.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> with the updated cart item count on success.
    /// </returns>
    Task<ServiceResult> AddItemAsync(
        int? userId,
        int? guestSessionId,
        int productId,
        int? variantId,
        int qty,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the quantity of a specific cart item.
    /// Validates stock before increasing quantity.
    /// </summary>
    /// <param name="cartItemId">The cart item to update.</param>
    /// <param name="newQty">The desired new quantity. Must be &gt;= 1.</param>
    /// <param name="ownerUserId">
    /// The user ID or null — used to verify the cart item belongs to the caller.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> containing the updated line total and subtotal.
    /// </returns>
    Task<ServiceResult> UpdateQuantityAsync(
        int cartItemId,
        int newQty,
        int? ownerUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a specific item from the cart.
    /// </summary>
    /// <param name="cartItemId">The cart item to remove.</param>
    /// <param name="ownerUserId">Used to verify ownership.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ServiceResult> RemoveItemAsync(
        int cartItemId,
        int? ownerUserId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Marks the cart as checked out (IsCheckedOut = true).
    /// Called by <c>OrderService.CreateOrderAsync</c> at order confirmation.
    /// </summary>
    /// <param name="cartId">The cart to check out.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task ClearCartAsync(
        int cartId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Merges a guest cart into a registered user's cart when the guest logs in.
    /// Items from the guest cart are moved to the user cart.
    /// The guest cart is then marked as checked out.
    /// </summary>
    /// <param name="guestSessionId">The guest session whose cart to merge.</param>
    /// <param name="userId">The user to merge the items into.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task MergeGuestCartAsync(
        int guestSessionId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the total item count (sum of quantities) for the navbar badge.
    /// Returns 0 when no active cart exists.
    /// </summary>
    /// <param name="userId">Authenticated user ID. Null for guests.</param>
    /// <param name="guestSessionId">Guest session ID. Null for authenticated users.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<int> GetCartCountAsync(
        int? userId,
        int? guestSessionId,
        CancellationToken cancellationToken = default);
}