using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        private const string SessionUserId = "UserId";

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        // POST /Review/Submit
        [HttpPost]
        public async Task<IActionResult> Submit(int productId, int orderId, int rating, string? comment)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return Json(new { success = false, message = "You must be logged in to submit a review." });

            bool canReview = await _reviewService.CanReviewAsync(userId.Value, productId);
            if (!canReview)
                return Json(new { success = false, message = "You can only review products from delivered orders." });

            await _reviewService.SubmitReviewAsync(userId.Value, productId, orderId, rating, comment);

            return Json(new { success = true, message = "Review submitted successfully." });
        }

        // GET /Review/ProductReviews/{productId}
        [HttpGet]
        public async Task<IActionResult> ProductReviews(int productId)
        {
            var reviews = await _reviewService.GetProductReviewsAsync(productId);

            var result = reviews.Select(r => new
            {
                reviewId           = r.ReviewId,
                rating             = r.Rating,
                comment            = r.Comment,
                isVerifiedPurchase = r.IsVerifiedPurchase,
                createdAt          = r.CreatedAt.ToString("MMM dd, yyyy"),
                userName           = r.User != null ? r.User.FullName : "Anonymous"
            });

            return Json(result);
        }
    }
}
