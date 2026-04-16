// WebApplication/Controllers/CheckoutController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the checkout flow: address selection, delivery method,
/// payment method selection, and order submission.
/// Flowchart: Part 3 — Cart &amp; Checkout.
/// </summary>
[Authorize]
public sealed class CheckoutController : Controller
{
    private readonly IOrderService   _orderService;
    private readonly ICartService    _cartService;
    private readonly UserRepository  _userRepo;
    private readonly IVoucherService _voucherService;
    private readonly ILogger<CheckoutController> _logger;

    private const string SessionKeyVoucherCode     = "checkout_voucher_code";
    private const string SessionKeyVoucherDiscount = "checkout_voucher_discount";

    /// <inheritdoc/>
    public CheckoutController(
        IOrderService   orderService,
        ICartService    cartService,
        UserRepository  userRepo,
        IVoucherService voucherService,
        ILogger<CheckoutController> logger)
    {
        _orderService   = orderService   ?? throw new ArgumentNullException(nameof(orderService));
        _cartService    = cartService    ?? throw new ArgumentNullException(nameof(cartService));
        _userRepo       = userRepo       ?? throw new ArgumentNullException(nameof(userRepo));
        _voucherService = voucherService ?? throw new ArgumentNullException(nameof(voucherService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Checkout
    // =========================================================================

    /// <summary>
    /// Renders the checkout page with the order summary sidebar,
    /// address selector, delivery method, and payment method selector.
    /// Redirects to cart if cart is empty.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        CartViewModel cart = await _cartService.GetCartAsync(userId, null, cancellationToken);
        if (cart.IsEmpty)
        {
            TempData["info"] = "Your cart is empty.";
            return RedirectToAction("ViewCart", "Cart");
        }

        Models.Entities.User? user =
            await _userRepo.GetWithAddressesAsync(userId, cancellationToken);

        // Read any previously validated voucher from session
        string? voucherCode     = HttpContext.Session.GetString(SessionKeyVoucherCode);
        string? voucherDiscount = HttpContext.Session.GetString(SessionKeyVoucherDiscount);
        decimal discountAmount  = decimal.TryParse(voucherDiscount, out decimal d) ? d : 0m;

        CheckoutViewModel vm = new()
        {
            SavedAddresses  = (user?.Addresses ?? new List<Address>())
                .Where(a => !a.IsSnapshot)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.CreatedAt)
                .ToList()
                .AsReadOnly(),
            CartItems       = cart.Items,
            SubTotal        = cart.SubTotal,
            VoucherCode     = voucherCode,
            DiscountAmount  = discountAmount,
            SelectedAddressId = user?.DefaultAddressId,
            AssignedVouchers = await GetAssignedVoucherViewModelsAsync(userId, cancellationToken)
        };

        ViewData["Title"] = "Checkout";
        return View("~/Views/Customer/Checkout.cshtml", vm);
    }

    // =========================================================================
    // POST /Checkout/PlaceOrder
    // =========================================================================

    /// <summary>
    /// Validates the checkout form and creates the order.
    /// On success, clears the voucher session keys and redirects to
    /// the order confirmation page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PlaceOrder(
        CheckoutViewModel vm,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        // Read voucher from session (never from form — prevents tampering)
        vm.VoucherCode    = HttpContext.Session.GetString(SessionKeyVoucherCode);
        string? rawDiscount = HttpContext.Session.GetString(SessionKeyVoucherDiscount);
        vm.DiscountAmount = decimal.TryParse(rawDiscount, out decimal d) ? d : 0m;

        if (!ModelState.IsValid)
        {
            // Reload read-only fields before returning the view
            CartViewModel cart = await _cartService.GetCartAsync(userId, null, cancellationToken);
            Models.Entities.User? user =
                await _userRepo.GetWithAddressesAsync(userId, cancellationToken);

            vm.CartItems      = cart.Items;
            vm.SubTotal       = cart.SubTotal;
            vm.SavedAddresses = (user?.Addresses ?? new List<Address>())
                .Where(a => !a.IsSnapshot)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.CreatedAt)
                .ToList()
                .AsReadOnly();
            vm.AssignedVouchers = await GetAssignedVoucherViewModelsAsync(userId, cancellationToken);

            ViewData["Title"] = "Checkout";
            return View("~/Views/Customer/Checkout.cshtml", vm);
        }

        try
        {
            ServiceResult<int> result =
                await _orderService.CreateOrderAsync(userId, vm, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Index));
            }

            // Clear voucher session keys after successful order
            HttpContext.Session.Remove(SessionKeyVoucherCode);
            HttpContext.Session.Remove(SessionKeyVoucherDiscount);

            TempData["success"] = "Your order has been placed successfully!";
            return RedirectToAction("Confirmation", "Order",
                new { orderId = result.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "PlaceOrder failed for user {UserId}.", userId);
            TempData["error"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // =========================================================================
    // AJAX GET /Checkout/GetShippingFee
    // =========================================================================

    /// <summary>
    /// Returns the shipping fee for the selected delivery method.
    /// Called by <c>checkout.js</c> when the customer changes delivery method.
    /// </summary>
    [HttpGet]
    public IActionResult GetShippingFee(string method)
    {
        decimal fee = method switch
        {
            "Lalamove" => CheckoutViewModel.LalamoveFee,
            "LBC"      => CheckoutViewModel.LBCFee,
            "Pickup"   => CheckoutViewModel.PickupFee,
            _          => 0m
        };

        return Json(ApiResponse.Ok(new
        {
            fee,
            formattedFee = fee == 0 ? "Free" : $"₱{fee:N2}"
        }));
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private int GetCurrentUserId()
    {
        string? value = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return int.TryParse(value, out int id) ? id : 0;
    }

    private async Task<IReadOnlyList<AssignedVoucherViewModel>> GetAssignedVoucherViewModelsAsync(int userId, CancellationToken cancellationToken)
    {
        var userVouchers = await _voucherService.GetActiveAssignedVouchersAsync(userId, cancellationToken);
        return userVouchers.Select(uv => new AssignedVoucherViewModel
        {
            Code = uv.Voucher.Code,
            Description = uv.Voucher.DiscountType == DiscountTypes.Percentage
                ? $"{uv.Voucher.DiscountValue:0.##}% off"
                : $"\u20b1{uv.Voucher.DiscountValue:N2} off",
            TimeLeft = FormatTimeLeft(uv.ExpiresAt ?? uv.Voucher.EndDate)
        }).ToList().AsReadOnly();
    }

    private static string FormatTimeLeft(DateTime? expiry)
    {
        if (!expiry.HasValue) return "No expiry";
        TimeSpan diff = expiry.Value - DateTime.Now;
        if (diff.TotalDays >= 1) return $"Expires in {Math.Floor(diff.TotalDays)} day(s)";
        if (diff.TotalHours >= 1) return $"Expires in {Math.Floor(diff.TotalHours)} hour(s)";
        if (diff.TotalMinutes >= 1) return $"Expires in {Math.Floor(diff.TotalMinutes)} min(s)";
        return "Expires soon";
    }
}