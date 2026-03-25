// WebApplication/Controllers/ReviewController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles review submission, the customer reviews list page,
/// and the AJAX load-more endpoint for the product detail page.
/// Flowchart: Part 6 — Delivery &amp; Review.
/// </summary>
[Authorize]
public sealed class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    public ReviewController(
        IReviewService reviewService,
        ILogger<ReviewController> logger)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        _logger        = logger        ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Review/MyReviews — customer's own reviews page
    // =========================================================================

    /// <summary>
    /// Renders the page listing all reviews written by the authenticated customer,
    /// plus any products they haven't reviewed yet from delivered orders.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> MyReviews(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        try
        {
            IReadOnlyList<ProductReviewViewModel> submitted =
                await _reviewService.GetByUserAsync(userId, cancellationToken);

            IReadOnlyList<ReviewViewModel> pending =
                await _reviewService.GetPendingReviewsAsync(userId, cancellationToken);

            ViewData["Title"] = "My Reviews";
            ViewBag.Submitted = submitted;
            ViewBag.Pending   = pending;

            return View("~/Views/Customer/Reviews.cshtml");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading reviews for user {UserId}.", userId);
            TempData["error"] = "Unable to load reviews. Please try again.";
            return RedirectToAction("Dashboard", "Customer");
        }
    }

    // =========================================================================
    // POST /Review/Submit — submit a new review
    // =========================================================================

    /// <summary>
    /// Processes the review form. On success redirects back to the product
    /// detail page; on failure back to the review form with an error.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        ReviewViewModel vm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            TempData["error"] = "Please correct the form errors.";
            return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
        }

        int userId = GetCurrentUserId();

        try
        {
            ServiceResult result =
                await _reviewService.SubmitReviewAsync(
                    userId, vm.ProductId, vm.OrderId, vm.Rating, vm.Comment, cancellationToken);

            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess
                    ? "Thank you! Your review has been submitted."
                    : result.Error;

            return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Review submission failed for user {UserId}.", userId);
            TempData["error"] = "Unable to submit review. Please try again.";
            return RedirectToAction("Detail", "Product", new { id = vm.ProductId });
        }
    }

    // =========================================================================
    // AJAX GET /Review/LoadMore — paginated reviews for product detail page
    // =========================================================================

    /// <summary>
    /// Returns the next page of reviews for a product as a partial HTML fragment.
    /// Called by <c>review.js</c> when the customer clicks "Load More Reviews".
    /// Does not require authentication — reviews are visible to all users.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> LoadMore(
        int productId,
        int page,
        CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<ReviewItemViewModel> reviews =
                await _reviewService.GetProductReviewsAsync(productId, page, 5, cancellationToken);

            return PartialView(
                "~/Views/Customer/Partials/_ReviewList.cshtml", reviews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "LoadMore reviews failed for product {ProductId}.", productId);
            return Json(ApiResponse.Fail("Unable to load reviews."));
        }
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private int GetCurrentUserId()
    {
        string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out int id) ? id : 0;
    }
}