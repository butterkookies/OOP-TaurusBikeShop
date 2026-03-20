using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IReviewService
    {
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
        Task<bool> CanReviewAsync(int userId, int productId);
        Task SubmitReviewAsync(int userId, int productId, int orderId, int rating, string? comment);
        Task<double> GetAverageRatingAsync(int productId);
    }
}
