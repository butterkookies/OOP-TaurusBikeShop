// WebApplication/BusinessLogic/Interfaces/IWishlistService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for wishlist operations.
/// Wishlist is an authenticated-only feature — all methods require a valid userId.
/// </summary>
public interface IWishlistService
{
    /// <summary>
    /// Returns the full wishlist view model for the authenticated user.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<WishlistViewModel> GetWishlistAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a product to the user's wishlist.
    /// Idempotent — silently succeeds if the product is already wishlisted.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="productId">The product to add.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> with <c>isInWishlist = true</c> on success.
    /// </returns>
    Task<ServiceResult> AddAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Removes a product from the user's wishlist.
    /// Silently succeeds if the product is not in the wishlist.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="productId">The product to remove.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<ServiceResult> RemoveAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Toggles a product in/out of the user's wishlist.
    /// Used by the heart button on product cards and the detail page.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="productId">The product to toggle.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> where <c>Data</c> contains
    /// <c>{ isInWishlist: bool }</c> reflecting the new state.
    /// </returns>
    Task<ServiceResult<bool>> ToggleAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the product IDs in the user's wishlist.
    /// Used by <c>ProductController</c> to set <c>IsInWishlist</c> flags
    /// on product cards without loading full wishlist data.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<IReadOnlyList<int>> GetProductIdsAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the count of products in the user's wishlist.
    /// Used for the wishlist navbar badge.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task<int> GetCountAsync(
        int userId,
        CancellationToken cancellationToken = default);
}