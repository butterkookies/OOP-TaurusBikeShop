// WebApplication/Controllers/ReviewController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the Write a Review page (GET + POST) and the product reviews list page.
/// <para>
/// Review submission requires authentication and a verified purchase
/// (Delivered order containing the product). Both checks are enforced by
/// <see cref="IReviewService"/>.
/// </para>
/// </summary>
[Authorize]
public sealed class ReviewController : Controller
{
    private readonly IReviewService _reviewService;
    private readonly ILogger<ReviewController> _logger;

    /// <inheritdoc/>
    public ReviewController(
        IReviewService reviewService,
        ILogger<ReviewController> logger)
    {
        _reviewService = reviewService ?? throw new ArgumentNullException(nameof(reviewService));
        _logger        = logger        ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Review?productId=1&orderId=5
    // =========================================================================

    /// <summary>
    /// Renders the Write a Review page.
    /// Returns 403 when the user does not have a verified (Delivered) purchase
    /// for the requested product.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(
        int productId,
        int orderId,
        CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();

        ReviewViewModel? vm = await _reviewService.GetReviewPageAsync(
            productId, orderId, userId, cancellationToken);

        if (vm is null)
        {
            TempData["error"] = "You can only review products from your delivered orders.";
            return RedirectToAction("Index", "Orders");
        }

        return View("~/Views/Customer/Review.cshtml", vm);
    }

    // =========================================================================
    // POST /Review/Submit
    // =========================================================================

    /// <summary>
    /// Handles review form submission.
    /// Re-verifies the purchase and prevents duplicate reviews server-side.
    /// On success, redirects to the order history; on failure, re-renders the form.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(
        ReviewViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Customer/Review.cshtml", model);
        }

        int userId = GetCurrentUserId();

        ServiceResult result = await _reviewService.SubmitReviewAsync(
            userId,
            model.ProductId,
            model.OrderId,
            model.Rating,
            model.Comment,
            cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View("~/Views/Customer/Review.cshtml", model);
        }

        TempData["success"] = "Your review has been submitted. Thank you!";
        return RedirectToAction("Index", "Orders");
    }

    // =========================================================================
    // GET /Review/Reviews/{productId}?page=1
    // Anonymous — does not require authentication.
    // =========================================================================

    /// <summary>
    /// Renders the paginated reviews list for a product.
    /// This action is publicly accessible — no authentication required.
    /// </summary>
    [AllowAnonymous]
    [HttpGet("Review/Reviews/{productId:int}")]
    public async Task<IActionResult> Reviews(
        int productId,
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        const int pageSize = 10;

        ProductReviewsViewModel? vm = await _reviewService.GetProductReviewsPageAsync(
            productId, page, pageSize, cancellationToken);

        if (vm is null)
            return NotFound();

        return View("~/Views/Customer/Reviews.cshtml", vm);
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
