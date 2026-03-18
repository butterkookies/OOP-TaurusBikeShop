// WebApplication/BusinessLogic/Services/ReviewService.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IReviewService"/> — verified-purchase review submission
/// and product review retrieval.
/// </summary>
public sealed class ReviewService : IReviewService
{
    private readonly AppDbContext      _context;
    private readonly ReviewRepository  _reviewRepo;
    private readonly ILogger<ReviewService> _logger;

    /// <inheritdoc/>
    public ReviewService(
        AppDbContext      context,
        ReviewRepository  reviewRepo,
        ILogger<ReviewService> logger)
    {
        _context    = context    ?? throw new ArgumentNullException(nameof(context));
        _reviewRepo = reviewRepo ?? throw new ArgumentNullException(nameof(reviewRepo));
        _logger     = logger     ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task<ReviewViewModel?> GetReviewPageAsync(
        int productId,
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        // Verify the user has a delivered purchase for this product
        bool verified = await _reviewRepo.HasVerifiedPurchaseAsync(
            userId, productId, cancellationToken);

        if (!verified)
            return null;

        // Load the product for display data
        Product? product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        if (product is null)
            return null;

        // Existing reviews for the feed (first page, up to 10)
        IReadOnlyList<Review> existing =
            await _reviewRepo.GetByProductAsync(productId, 1, 10, cancellationToken);

        IEnumerable<ReviewItemViewModel> existingVms = existing.Select(r => new ReviewItemViewModel
        {
            UserName  = r.User?.FirstName ?? "Customer",
            Rating    = r.Rating,
            Comment   = r.Comment,
            CreatedAt = r.CreatedAt
        });

        return new ReviewViewModel
        {
            ProductId       = productId,
            OrderId         = orderId,
            ProductName     = product.Name,
            ProductCategory = product.Category?.Name ?? string.Empty,
            BrandName       = product.Brand?.Name   ?? string.Empty,
            SKU             = product.SKU,
            IsVerifiedPurchase = true,
            ExistingReviews = existingVms
        };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SubmitReviewAsync(
        int userId,
        int productId,
        int orderId,
        int rating,
        string? comment,
        CancellationToken cancellationToken = default)
    {
        if (rating < 1 || rating > 5)
            return ServiceResult.Fail("Rating must be between 1 and 5.");

        // Re-verify purchase to prevent spoofed POSTs
        bool verified = await _reviewRepo.HasVerifiedPurchaseAsync(
            userId, productId, cancellationToken);

        if (!verified)
            return ServiceResult.Fail(
                "You can only review products from your delivered orders.");

        // Prevent duplicate reviews for the same product + order
        bool duplicate = await _context.Reviews
            .AnyAsync(
                r => r.UserId    == userId
                  && r.ProductId == productId
                  && r.OrderId   == orderId,
                cancellationToken);

        if (duplicate)
            return ServiceResult.Fail(
                "You have already submitted a review for this product on this order.");

        try
        {
            Review review = new()
            {
                UserId             = userId,
                ProductId          = productId,
                OrderId            = orderId,
                Rating             = rating,
                Comment            = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
                IsVerifiedPurchase = true,
                CreatedAt          = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(review, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitReviewAsync failed for user {UserId}, product {ProductId}.",
                userId, productId);
            return ServiceResult.Fail("Unable to submit review. Please try again.");
        }
    }

    /// <inheritdoc/>
    public async Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(
        int productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Review> reviews =
            await _reviewRepo.GetByProductAsync(productId, page, pageSize, cancellationToken);

        return reviews.Select(r => new ReviewItemViewModel
        {
            UserName  = r.User?.FirstName ?? "Customer",
            Rating    = r.Rating,
            Comment   = r.Comment,
            CreatedAt = r.CreatedAt
        }).ToList().AsReadOnly();
    }

    /// <inheritdoc/>
    public async Task<ProductReviewsViewModel?> GetProductReviewsPageAsync(
        int productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        Product? product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);

        if (product is null)
            return null;

        int safePage     = Math.Max(1, page);
        int safePageSize = Math.Clamp(pageSize, 1, 50);

        int     totalCount     = await _reviewRepo.GetReviewCountAsync(productId, cancellationToken);
        decimal averageRating  = await _reviewRepo.GetAverageRatingAsync(productId, cancellationToken);
        int     totalPages     = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)safePageSize);

        IReadOnlyList<Review> reviews =
            await _reviewRepo.GetByProductAsync(productId, safePage, safePageSize, cancellationToken);

        IReadOnlyList<ReviewItemViewModel> reviewVms = reviews
            .Select(r => new ReviewItemViewModel
            {
                UserName  = r.User?.FirstName ?? "Customer",
                Rating    = r.Rating,
                Comment   = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToList()
            .AsReadOnly();

        return new ProductReviewsViewModel
        {
            ProductId     = productId,
            ProductName   = product.Name,
            AverageRating = (double)averageRating,
            TotalCount    = totalCount,
            CurrentPage   = safePage,
            TotalPages    = totalPages,
            Reviews       = reviewVms
        };
    }
}
