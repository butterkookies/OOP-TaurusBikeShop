// WebApplication/DataAccess/Repositories/ReviewRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Review"/> entities.
/// Handles review retrieval, verified purchase checks, and average rating calculation.
/// </summary>
public sealed class ReviewRepository : Repository<Review>
{
    /// <inheritdoc/>
    public ReviewRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns a paginated list of reviews for a specific product,
    /// ordered most recent first. Each review includes the reviewer's name.
    /// </summary>
    /// <param name="productId">The product to retrieve reviews for.</param>
    /// <param name="page">1-based page number.</param>
    /// <param name="pageSize">Number of reviews per page.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Paginated reviews with User nav property loaded.</returns>
    public async Task<IReadOnlyList<Review>> GetByProductAsync(
        int productId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await Context.Reviews
            .AsNoTracking()
            .Include(r => r.User)
            .Where(r => r.ProductId == productId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Checks whether a user has a <c>Delivered</c> order containing the
    /// specified product, qualifying them to submit a verified review.
    /// </summary>
    /// <param name="userId">The user who wants to submit a review.</param>
    /// <param name="productId">The product being reviewed.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// <c>true</c> if the user has at least one Delivered order containing
    /// the product; <c>false</c> otherwise.
    /// </returns>
    public async Task<bool> HasVerifiedPurchaseAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await Context.OrderItems
            .AnyAsync(
                oi => oi.ProductId == productId
                   && oi.Order.UserId == userId
                   && oi.Order.OrderStatus == OrderStatuses.Delivered,
                cancellationToken);
    }

    /// <summary>
    /// Returns the OrderId of a Delivered order containing the specified product
    /// for the given user, or <c>0</c> if none exists.
    /// </summary>
    public async Task<int> GetVerifiedPurchaseOrderIdAsync(
        int userId,
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await Context.OrderItems
            .Where(oi => oi.ProductId == productId
                      && oi.Order.UserId == userId
                      && oi.Order.OrderStatus == OrderStatuses.Delivered)
            .Select(oi => oi.OrderId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// Calculates the average star rating for a product across all reviews.
    /// </summary>
    /// <param name="productId">The product ID to calculate the average for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// The average rating as a decimal, or <c>0</c> if the product has no reviews.
    /// </returns>
    public async Task<decimal> GetAverageRatingAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        bool hasReviews = await Context.Reviews
            .AnyAsync(r => r.ProductId == productId, cancellationToken);

        if (!hasReviews)
            return 0m;

        double average = await Context.Reviews
            .Where(r => r.ProductId == productId)
            .AverageAsync(r => r.Rating, cancellationToken);

        return (decimal)Math.Round(average, 1);
    }

    /// <summary>
    /// Returns the total number of reviews for a product.
    /// Used alongside <see cref="GetByProductAsync"/> for pagination.
    /// </summary>
    /// <param name="productId">The product ID to count reviews for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Total review count for the product.</returns>
    public async Task<int> GetReviewCountAsync(
        int productId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Reviews
            .CountAsync(r => r.ProductId == productId, cancellationToken);
    }
}