// WebApplication/Controllers/CartController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models;
using WebApplication.Models.Entities;
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
    private readonly AppDbContext _context;
    private readonly ILogger<CartController> _logger;

    private const string GuestSessionCookieName = "tbs_guest";

    public CartController(
        ICartService cartService,
        AppDbContext context,
        ILogger<CartController> logger)
    {
        _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
        _context     = context     ?? throw new ArgumentNullException(nameof(context));
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(
        int productId,
        int? variantId,
        int qty = 1,
        CancellationToken cancellationToken = default)
    {
        try
        {
            int? userId  = GetCurrentUserId();
            int? guestId = userId.HasValue ? null : await EnsureGuestSessionAsync(cancellationToken);

            ServiceResult result = await _cartService.AddItemAsync(
                userId, guestId, productId, variantId, qty, cancellationToken);

            if (!result.IsSuccess)
                return Json(ApiResponse.Fail(result.Error!));

            int count = await _cartService.GetCartCountAsync(userId, guestId, cancellationToken);
            return Json(ApiResponse.Ok(new { cartCount = count }, "Item added to cart."));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddToCart failed for product {ProductId}.", productId);
            return Json(ApiResponse.Fail("Unable to add item. Please try again."));
        }
    }

    // =========================================================================
    // AJAX POST /Cart/UpdateQuantity
    // =========================================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateQuantity(
        int cartItemId,
        int qty,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ServiceResult result = await _cartService.UpdateQuantityAsync(
                cartItemId, qty, GetCurrentUserId(), cancellationToken);

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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(
        int cartItemId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ServiceResult result = await _cartService.RemoveItemAsync(
                cartItemId, GetCurrentUserId(), cancellationToken);

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
        catch
        {
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

        GuestSession session = new()
        {
            SessionToken = Guid.NewGuid().ToString("N"),
            ExpiresAt    = DateTime.UtcNow.AddDays(7),
            CreatedAt    = DateTime.UtcNow
        };

        await _context.GuestSessions.AddAsync(session, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        Response.Cookies.Append(GuestSessionCookieName, session.GuestSessionId.ToString(),
            new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires  = DateTimeOffset.UtcNow.AddDays(7)
            });

        return session.GuestSessionId;
    }
}