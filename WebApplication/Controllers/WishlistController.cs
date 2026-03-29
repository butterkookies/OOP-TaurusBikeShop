// WebApplication/Controllers/WishlistController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the wishlist page and AJAX toggle/remove/move-to-cart endpoints.
/// All actions require authentication.
/// </summary>
[Authorize]
public sealed class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    private readonly ICartService     _cartService;
    private readonly ILogger<WishlistController> _logger;

    public WishlistController(
        IWishlistService wishlistService,
        ICartService     cartService,
        ILogger<WishlistController> logger)
    {
        _wishlistService = wishlistService ?? throw new ArgumentNullException(nameof(wishlistService));
        _cartService     = cartService     ?? throw new ArgumentNullException(nameof(cartService));
        _logger          = logger          ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Wishlist
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> ViewWishlist(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        try
        {
            WishlistViewModel vm =
                await _wishlistService.GetWishlistAsync(userId, cancellationToken);

            ViewData["Title"] = "My Wishlist";
            return View("~/Views/Customer/Wishlist.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading wishlist for user {UserId}.", userId);
            TempData["error"] = "Unable to load wishlist. Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }

    // =========================================================================
    // AJAX POST /Wishlist/Toggle
    // =========================================================================

    public sealed record ToggleRequest(int ProductId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(
        [FromBody] ToggleRequest req,
        CancellationToken cancellationToken = default)
    {
        int productId = req?.ProductId ?? 0;
        try
        {
            ServiceResult<bool> result = await _wishlistService
                .ToggleAsync(GetCurrentUserId(), productId, cancellationToken);

            if (!result.IsSuccess)
                return Json(ApiResponse.Fail(result.Error!));

            bool isNowInWishlist = result.Value;
            string msg = isNowInWishlist ? "Added to wishlist." : "Removed from wishlist.";

            return Json(ApiResponse.Ok(new { isInWishlist = isNowInWishlist }, msg));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Wishlist toggle failed for product {ProductId}.", productId);
            return Json(ApiResponse.Fail("Unable to update wishlist. Please try again."));
        }
    }

    // =========================================================================
    // AJAX POST /Wishlist/Remove
    // =========================================================================

    public sealed record RemoveRequest(int ProductId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(
        [FromBody] RemoveRequest req,
        CancellationToken cancellationToken = default)
    {
        int productId = req?.ProductId ?? 0;
        try
        {
            ServiceResult result = await _wishlistService
                .RemoveAsync(GetCurrentUserId(), productId, cancellationToken);

            return result.IsSuccess
                ? Json(ApiResponse.Ok(message: "Removed from wishlist."))
                : Json(ApiResponse.Fail(result.Error!));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Wishlist remove failed for product {ProductId}.", productId);
            return Json(ApiResponse.Fail("Unable to remove item. Please try again."));
        }
    }

    // =========================================================================
    // AJAX POST /Wishlist/MoveToCart
    // =========================================================================

    public sealed record MoveToCartRequest(int ProductId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveToCart(
        [FromBody] MoveToCartRequest req,
        CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();
        int productId = req?.ProductId ?? 0;

        try
        {
            ServiceResult cartResult = await _cartService.AddItemAsync(
                userId, null, productId, null, 1, cancellationToken);

            if (!cartResult.IsSuccess)
                return Json(ApiResponse.Fail(cartResult.Error!));

            await _wishlistService.RemoveAsync(userId, productId, cancellationToken);

            int cartCount = await _cartService.GetCartCountAsync(userId, null, cancellationToken);

            return Json(ApiResponse.Ok(new { cartCount }, "Moved to cart."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "MoveToCart failed for product {ProductId}.", productId);
            return Json(ApiResponse.Fail("Unable to move item to cart."));
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