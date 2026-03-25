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
    private readonly AppDbContext           _context;
    private readonly ReviewRepository       _reviewRepo;
    private readonly ILogger<ReviewService> _logger;

    public ReviewService(
        AppDbContext context, ReviewRepository reviewRepo, ILogger<ReviewService> logger)
    {
        _context    = context    ?? throw new ArgumentNullException(nameof(context));
        _reviewRepo = reviewRepo ?? throw new ArgumentNullException(nameof(reviewRepo));
        _logger     = logger     ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<ReviewViewModel?> GetReviewPageAsync(
        int productId, int orderId, int userId, CancellationToken cancellationToken = default)
    {
        bool verified = await _reviewRepo.HasVerifiedPurchaseAsync(userId, productId, cancellationToken);
        if (!verified) return null;

        Product? product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (product is null) return null;

        IReadOnlyList<Review> existing =
            await _reviewRepo.GetByProductAsync(productId, 1, 10, cancellationToken);

        return new ReviewViewModel
        {
            ProductId          = productId,
            OrderId            = orderId,
            ProductName        = product.Name,
            ProductCategory    = product.Category?.Name ?? string.Empty,
            BrandName          = product.Brand?.BrandName ?? string.Empty,
            SKU                = product.SKU,
            IsVerifiedPurchase = true,
            ExistingReviews    = existing.Select(r => new ReviewItemViewModel
            {
                UserName  = r.User?.FirstName ?? "Customer",
                Rating    = r.Rating,
                Comment   = r.Comment,
                CreatedAt = r.CreatedAt
            })
        };
    }

    public Task<ServiceResult> SubmitReviewAsync(
        int userId, ReviewViewModel vm,
        CancellationToken cancellationToken = default)
    {
        return SubmitReviewAsync(userId, vm.ProductId, vm.OrderId, vm.Rating, vm.Comment, cancellationToken);
    }

    public async Task<ServiceResult> SubmitReviewAsync(
        int userId, int productId, int orderId, int rating, string? comment,
        CancellationToken cancellationToken = default)
    {
        if (rating < 1 || rating > 5)
            return ServiceResult.Fail("Rating must be between 1 and 5.");

        bool verified = await _reviewRepo.HasVerifiedPurchaseAsync(userId, productId, cancellationToken);
        if (!verified)
            return ServiceResult.Fail("You can only review products from your delivered orders.");

        bool duplicate = await _context.Reviews
            .AnyAsync(r => r.UserId == userId && r.ProductId == productId && r.OrderId == orderId,
                cancellationToken);
        if (duplicate)
            return ServiceResult.Fail("You have already submitted a review for this product on this order.");

        try
        {
            await _context.Reviews.AddAsync(new Review
            {
                UserId             = userId,
                ProductId          = productId,
                OrderId            = orderId,
                Rating             = rating,
                Comment            = string.IsNullOrWhiteSpace(comment) ? null : comment.Trim(),
                IsVerifiedPurchase = true,
                CreatedAt          = DateTime.UtcNow
            }, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SubmitReviewAsync failed for user {UserId}, product {ProductId}.",
                userId, productId);
            return ServiceResult.Fail("Unable to submit review. Please try again.");
        }
    }

    public async Task<IReadOnlyList<ReviewItemViewModel>> GetProductReviewsAsync(
        int productId, int page, int pageSize, CancellationToken cancellationToken = default)
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

    public async Task<IReadOnlyList<ReviewViewModel>> GetByUserAsync(
        int userId, CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Review> reviews = await _context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Include(r => r.Product)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);

        return reviews.Select(r => new ReviewViewModel
        {
            ProductId      = r.ProductId,
            OrderId        = r.OrderId,
            ProductName    = r.Product?.Name ?? "Product",
            Rating         = r.Rating,
            Comment        = r.Comment,
            ReviewerName   = r.User?.FirstName ?? "You",
            CreatedAt      = r.CreatedAt,
            IsVerifiedPurchase = r.IsVerifiedPurchase
        }).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyList<ReviewViewModel>> GetPendingReviewsAsync(
        int userId, CancellationToken cancellationToken = default)
    {
        // Find products from delivered orders that the user hasn't reviewed yet
        List<OrderItem> deliveredItems = await _context.OrderItems
            .AsNoTracking()
            .Include(oi => oi.Product)
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.UserId == userId
                      && oi.Order.OrderStatus == OrderStatuses.Delivered)
            .ToListAsync(cancellationToken);

        HashSet<int> reviewedProductIds = (await _context.Reviews
            .AsNoTracking()
            .Where(r => r.UserId == userId)
            .Select(r => r.ProductId)
            .ToListAsync(cancellationToken))
            .ToHashSet();

        return deliveredItems
            .Where(oi => !reviewedProductIds.Contains(oi.ProductId))
            .GroupBy(oi => oi.ProductId)
            .Select(g =>
            {
                OrderItem first = g.First();
                return new ReviewViewModel
                {
                    ProductId          = first.ProductId,
                    OrderId            = first.OrderId,
                    ProductName        = first.Product?.Name ?? "Product",
                    IsVerifiedPurchase = true
                };
            })
            .ToList()
            .AsReadOnly();
    }

    public async Task<IReadOnlyList<ReviewViewModel>> GetByProductAsync(
        int productId, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Review> reviews =
            await _reviewRepo.GetByProductAsync(productId, page, pageSize, cancellationToken);

        return reviews.Select(r => new ReviewViewModel
        {
            ProductId          = r.ProductId,
            OrderId            = r.OrderId,
            Rating             = r.Rating,
            Comment            = r.Comment,
            ReviewerName       = r.User?.FirstName ?? "Customer",
            CreatedAt          = r.CreatedAt,
            IsVerifiedPurchase = r.IsVerifiedPurchase
        }).ToList().AsReadOnly();
    }

    public async Task<ProductReviewsViewModel?> GetProductReviewsPageAsync(
        int productId, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        Product? product = await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.ProductId == productId, cancellationToken);
        if (product is null) return null;

        int safePage     = Math.Max(1, page);
        int safePageSize = Math.Clamp(pageSize, 1, 50);
        int totalCount   = await _reviewRepo.GetReviewCountAsync(productId, cancellationToken);
        decimal avg      = await _reviewRepo.GetAverageRatingAsync(productId, cancellationToken);
        int totalPages   = totalCount == 0 ? 1 : (int)Math.Ceiling(totalCount / (double)safePageSize);

        IReadOnlyList<Review> reviews =
            await _reviewRepo.GetByProductAsync(productId, safePage, safePageSize, cancellationToken);

        return new ProductReviewsViewModel
        {
            ProductId     = productId,
            ProductName   = product.Name,
            AverageRating = (double)avg,
            TotalCount    = totalCount,
            CurrentPage   = safePage,
            TotalPages    = totalPages,
            Reviews       = reviews.Select(r => new ReviewItemViewModel
            {
                UserName  = r.User?.FirstName ?? "Customer",
                Rating    = r.Rating,
                Comment   = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList().AsReadOnly()
        };
    }
}
