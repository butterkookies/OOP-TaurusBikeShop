// WebApplication/Controllers/CartController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles cart view and all AJAX cart mutation endpoints.
/// Supports both authenticated and guest users.
/// Flowchart: Part 3 — Cart &amp; Checkout.
/// </summary>
public sealed class CartController : Controller
{
    private readonly ICartService _cartService;
    private readonly ILogger<CartController> _logger;

    private const string GuestSessionCookieName = "tbs_guest";

    public CartController(
        ICartService cartService,
        ILogger<CartController> logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _logger      = logger      ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Cart — full cart page
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> ViewCart(CancellationToken cancellationToken)
    {
        try
        {
            (int? userId, int? guestId) = GetCartOwner();
            CartViewModel vm = await _cartService.GetCartAsync(userId, guestId, cancellationToken);
            ViewData["Title"] = "Shopping Cart";
            return View("~/Views/Customer/Cart.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading cart.");
            TempData["error"] = "Unable to load cart. Please try again.";
            return RedirectToAction("Index", "Home");
        }
    }

    // =========================================================================
    // AJAX POST /Cart/AddToCart
    // =========================================================================

    public sealed record AddToCartRequest(int ProductId, int? VariantId, int Qty = 1);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(
        [FromBody] AddToCartRequest req,
        CancellationToken cancellationToken = default)
    {
        try
        {
            int? userId  = GetCurrentUserId();
            int? guestId = userId.HasValue ? null : await EnsureGuestSessionAsync(cancellationToken);

            ServiceResult result = await _cartService.AddItemAsync(
                userId, guestId, req.ProductId, req.VariantId, req.Qty, cancellationToken);

            if (!result.IsSuccess)
                return Json(ApiResponse.Fail(result.Error!));

            int count = await _cartService.GetCartCountAsync(userId, guestId, cancellationToken);
            return Json(ApiResponse.Ok(new { cartCount = count }, "Item added to cart."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddToCart failed for product {ProductId}.", req.ProductId);
            return Json(ApiResponse.Fail("Unable to add item. Please try again."));
        }
    }

    // =========================================================================
    // AJAX POST /Cart/UpdateQuantity
    // =========================================================================

    public sealed record UpdateQuantityRequest(int CartItemId, int Qty);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(
        [FromBody] UpdateQuantityRequest req,
        CancellationToken cancellationToken = default)
    {
        int cartItemId = req.CartItemId;
        int qty        = req.Qty;
        try
        {
            (int? ownerUserId, int? ownerGuestId) = GetCartOwner();
            ServiceResult result = await _cartService.UpdateQuantityAsync(
                cartItemId, qty, ownerUserId, ownerGuestId, cancellationToken);

            if (!result.IsSuccess)
                return Json(ApiResponse.Fail(result.Error!));

            (int? userId, int? guestId) = GetCartOwner();
            CartViewModel vm = await _cartService.GetCartAsync(userId, guestId, cancellationToken);
            CartItemViewModel? item = vm.Items.FirstOrDefault(i => i.CartItemId == cartItemId);

            return Json(ApiResponse.Ok(new
            {
                lineTotal          = item?.LineTotal ?? 0m,
                formattedLineTotal = item?.FormattedLineTotal ?? "₱0.00",
                subtotal           = vm.SubTotal,
                formattedSubtotal  = vm.FormattedSubTotal,
                cartCount          = vm.TotalQuantity
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "UpdateQuantity failed for item {CartItemId}.", cartItemId);
            return Json(ApiResponse.Fail("Unable to update quantity."));
        }
    }

    // =========================================================================
    // AJAX POST /Cart/RemoveFromCart
    // =========================================================================

    public sealed record RemoveFromCartRequest(int CartItemId);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(
        [FromBody] RemoveFromCartRequest req,
        CancellationToken cancellationToken = default)
    {
        int cartItemId = req.CartItemId;
        try
        {
            (int? ownerUserId, int? ownerGuestId) = GetCartOwner();
            ServiceResult result = await _cartService.RemoveItemAsync(
                cartItemId, ownerUserId, ownerGuestId, cancellationToken);

            if (!result.IsSuccess)
                return Json(ApiResponse.Fail(result.Error!));

            (int? userId, int? guestId) = GetCartOwner();
            CartViewModel vm = await _cartService.GetCartAsync(userId, guestId, cancellationToken);

            return Json(ApiResponse.Ok(new
            {
                subtotal          = vm.SubTotal,
                formattedSubtotal = vm.FormattedSubTotal,
                cartCount         = vm.TotalQuantity,
                isEmpty           = vm.IsEmpty
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "RemoveFromCart failed for item {CartItemId}.", cartItemId);
            return Json(ApiResponse.Fail("Unable to remove item."));
        }
    }

    // =========================================================================
    // AJAX GET /Cart/GetCartCount
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> GetCartCount(CancellationToken cancellationToken)
    {
        try
        {
            (int? userId, int? guestId) = GetCartOwner();
            int count = await _cartService.GetCartCountAsync(userId, guestId, cancellationToken);
            return Json(ApiResponse.Ok(new { count }));
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "GetCartCount failed; returning 0.");
            return Json(ApiResponse.Ok(new { count = 0 }));
        }
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private int? GetCurrentUserId()
    {
        string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out int id) ? id : null;
    }

    private int? GetGuestSessionId()
    {
        string? cookie = Request.Cookies[GuestSessionCookieName];
        return int.TryParse(cookie, out int id) ? id : null;
    }

    private (int? UserId, int? GuestSessionId) GetCartOwner()
    {
        int? userId = GetCurrentUserId();
        return userId.HasValue
            ? (userId, null)
            : (null, GetGuestSessionId());
    }

    private async Task<int?> EnsureGuestSessionAsync(CancellationToken cancellationToken)
    {
        int? existing = GetGuestSessionId();
        if (existing.HasValue) return existing;

        int guestSessionId = await _cartService.CreateGuestSessionAsync(cancellationToken);

        Response.Cookies.Append(GuestSessionCookieName, guestSessionId.ToString(),
            new CookieOptions
            {
                HttpOnly = true,
                Secure   = true,
                SameSite = SameSiteMode.Lax,
                Expires  = DateTimeOffset.UtcNow.AddDays(7)
            });

        return guestSessionId;
    }
}