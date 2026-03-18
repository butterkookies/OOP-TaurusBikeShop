// WebApplication/Controllers/WishlistController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the wishlist page and AJAX toggle endpoint.
/// All actions require authentication — wishlist is not available to guests.
/// </summary>
[Authorize]
public sealed class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    private readonly ICartService     _cartService;
    private readonly ILogger<WishlistController> _logger;

    /// <inheritdoc/>
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

    /// <summary>
    /// Renders the full wishlist page with all saved products.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ViewWishlist(CancellationToken cancellationToken)
    {
        try
        {
            WishlistViewModel vm =
                await _wishlistService.GetWishlistAsync(GetCurrentUserId(), cancellationToken);

            ViewData["Title"] = "My Wishlist";
            return View("~/Views/Customer/Wishlist.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading wishlist for user {UserId}.", GetCurrentUserId());
            TempData["error"] = "Unable to load wishlist. Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }

    // =========================================================================
    // AJAX POST /Wishlist/Toggle
    // =========================================================================

    /// <summary>
    /// Toggles a product in or out of the user's wishlist.
    /// Returns JSON with the new <c>isInWishlist</c> state.
    /// Called by <c>product-catalog.js</c> for both catalog cards
    /// and the product detail page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Toggle(
        int productId,
        CancellationToken cancellationToken = default)
    {
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

    /// <summary>
    /// Removes a product from the wishlist.
    /// Used by the Remove button on the Wishlist page.
    /// Returns JSON for the AJAX call in <c>wishlist.js</c>.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Remove(
        int productId,
        CancellationToken cancellationToken = default)
    {
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

    /// <summary>
    /// Moves a wishlisted product directly into the cart, then removes
    /// it from the wishlist. The default variant and quantity 1 are used.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveToCart(
        int productId,
        CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();

        try
        {
            ServiceResult cartResult = await _cartService.AddItemAsync(
                userId, null, productId, null, 1, cancellationToken);

            if (!cartResult.IsSuccess)
                return Json(ApiResponse.Fail(cartResult.Error!));

            // Remove from wishlist after successfully adding to cart
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