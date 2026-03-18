// WebApplication/BusinessLogic/Interfaces/IReviewService.cs

using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for customer product review operations.
/// Reviews can only be submitted for products in Delivered orders
/// (verified purchase requirement).
/// </summary>
public interface IReviewService
{
    /// <summary>
    /// Builds the view model for the Write a Review page.
    /// Verifies that <paramref name="userId"/> has a Delivered order containing
    /// <paramref name="productId"/> before returning the model.
    /// </summary>
    /// <param name="productId">The product to review.</param>
    /// <param name="orderId">The order that contained the product.</param>
    /// <param name="userId">The authenticated customer.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A populated <see cref="ReviewViewModel"/>, or <c>null</c> when the
    /// product / order is not found or the purchase cannot be verified.
    /// </returns>
    Task<ReviewViewModel?> GetReviewPageAsync(
        int productId,
        int orderId,
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Persists a new review after re-confirming the verified purchase.
    /// </summary>
    /// <param name="userId">The authenticated customer submitting the review.</param>
    /// <param name="productId">The product being reviewed.</param>
    /// <param name="orderId">The order that contained the product.</param>
    /// <param name="rating">Star rating 1–5.</param>
    /// <param name="comment">Optional free-text comment.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// <see cref="ServiceResult.Ok"/> on success, or a failure with an error
    /// message when the purchase cannot be verified or the review already exists.
    /// </returns>
    Task<ServiceResult> SubmitReviewAsync(
        int userId,
        int productId,
        int orderId,
        int rating,
        string? comment,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns a paginated list of review display items for a product.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Reviews per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Read-only list of <see cref="ReviewItemViewModel"/>.</returns>
    Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(
        int productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Builds the full paginated view model for the product reviews list page.
    /// Includes the product name, average rating, total count, and the current page of reviews.
    /// </summary>
    /// <param name="productId">The product ID.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Reviews per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A populated <see cref="ProductReviewsViewModel"/>, or <c>null</c> when the
    /// product does not exist.
    /// </returns>
    Task<ProductReviewsViewModel?> GetProductReviewsPageAsync(
        int productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}
