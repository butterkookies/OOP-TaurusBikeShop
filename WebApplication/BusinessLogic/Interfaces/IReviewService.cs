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
    Task<ReviewViewModel?> GetReviewPageAsync(
        int productId, int orderId, int userId,
        CancellationToken cancellationToken = default);

    Task<ServiceResult> SubmitReviewAsync(
        int userId, int productId, int orderId, int rating, string? comment,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(
        int productId, int page, int pageSize,
        CancellationToken cancellationToken = default);

    Task<ProductReviewsViewModel?> GetProductReviewsPageAsync(
        int productId, int page, int pageSize,
        CancellationToken cancellationToken = default);

    /// <summary>Returns all reviews submitted by a specific user.</summary>
    Task<IReadOnlyList<ProductReviewViewModel>> GetByUserAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>Returns products in delivered orders that the user has not yet reviewed.</summary>
    Task<IReadOnlyList<ReviewViewModel>> GetPendingReviewsAsync(
        int userId,
        CancellationToken cancellationToken = default);
}
