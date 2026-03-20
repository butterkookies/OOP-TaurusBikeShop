using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class ReviewService : IReviewService
    {
        private readonly AppDbContext _context;

        public ReviewService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<bool> CanReviewAsync(int userId, int productId)
        {
            // User must have a delivered order containing this product
            bool hasDeliveredOrder = await _context.OrderItems
                .Include(oi => oi.Order)
                .AnyAsync(oi =>
                    oi.ProductId == productId &&
                    oi.Order != null &&
                    oi.Order.UserId == userId &&
                    oi.Order.OrderStatus == "Delivered");

            if (!hasDeliveredOrder) return false;

            // User must not have already reviewed this product
            bool alreadyReviewed = await _context.Reviews
                .AnyAsync(r => r.UserId == userId && r.ProductId == productId);

            return !alreadyReviewed;
        }

        public async Task SubmitReviewAsync(int userId, int productId, int orderId, int rating, string? comment)
        {
            var review = new Review
            {
                UserId              = userId,
                ProductId           = productId,
                OrderId             = orderId,
                Rating              = rating,
                Comment             = comment,
                IsVerifiedPurchase  = true,
                CreatedAt           = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<double> GetAverageRatingAsync(int productId)
        {
            bool hasReviews = await _context.Reviews.AnyAsync(r => r.ProductId == productId);
            if (!hasReviews) return 0;

            return await _context.Reviews
                .Where(r => r.ProductId == productId)
                .AverageAsync(r => (double)r.Rating);
        }
    }
}
