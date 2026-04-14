// WebApplication/Controllers/VoucherController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles voucher validation AJAX calls from the checkout page.
/// All actions require authentication — vouchers are not applicable to guests.
/// </summary>
[Authorize]
public sealed class VoucherController : Controller
{
    private readonly IVoucherService _voucherService;
    private readonly ICartService    _cartService;
    private readonly ILogger<VoucherController> _logger;

    // Session key used to persist the validated voucher across the checkout flow
    private const string SessionKeyVoucherCode     = "checkout_voucher_code";
    private const string SessionKeyVoucherDiscount = "checkout_voucher_discount";

    /// <inheritdoc/>
    public VoucherController(
        IVoucherService voucherService,
        ICartService    cartService,
        ILogger<VoucherController> logger)
    {
        _voucherService = voucherService ?? throw new ArgumentNullException(nameof(voucherService));
        _cartService    = cartService    ?? throw new ArgumentNullException(nameof(cartService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // AJAX POST /Voucher/Validate
    // =========================================================================

    /// <summary>
    /// Validates a voucher code against the cart subtotal.
    /// On success, stores the code and discount amount in session so the
    /// checkout controller can apply it during order creation.
    /// Returns a <see cref="VoucherValidationResult"/> as JSON.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Validate(
        [FromBody] VoucherApplyRequest request,
        CancellationToken cancellationToken = default)
    {
        string? code = request?.Code;
        if (string.IsNullOrWhiteSpace(code))
            return Json(ApiResponse.Fail("Please enter a voucher code."));

        int userId = GetCurrentUserId();

        try
        {
            // Get the cart subtotal to calculate the discount against
            CartViewModel cart = await _cartService.GetCartAsync(
                userId, null, cancellationToken);

            if (cart.IsEmpty)
                return Json(ApiResponse.Fail("Your cart is empty."));

            VoucherValidationResult result = await _voucherService.ValidateAsync(
                code.Trim().ToUpperInvariant(), userId, cart.SubTotal, cancellationToken);

            if (!result.IsValid)
                return Json(ApiResponse.Fail(result.Error!));

            // Persist to session for checkout flow
            HttpContext.Session.SetString(SessionKeyVoucherCode,     result.VoucherCode!);
            HttpContext.Session.SetString(SessionKeyVoucherDiscount,
                result.DiscountAmount.ToString("F2"));

            return Json(ApiResponse.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Voucher validation failed for code {Code}.", code);
            return Json(ApiResponse.Fail("Unable to validate voucher. Please try again."));
        }
    }

    // =========================================================================
    // AJAX POST /Voucher/Remove
    // =========================================================================

    /// <summary>
    /// Removes the currently applied voucher from session,
    /// restoring the full cart subtotal in the checkout summary.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Remove()
    {
        HttpContext.Session.Remove(SessionKeyVoucherCode);
        HttpContext.Session.Remove(SessionKeyVoucherDiscount);
        return Json(ApiResponse.Ok(message: "Voucher removed."));
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