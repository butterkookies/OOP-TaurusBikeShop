// WebApplication/Controllers/PaymentController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;

namespace WebApplication.Controllers;

/// <summary>
/// Handles payment proof submission for GCash and BankTransfer.
/// Flowchart: Part 5 — Payment Processing.
/// </summary>
[Authorize]
public sealed class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    /// <inheritdoc/>
    public PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger)
    {
        _paymentService = paymentService
            ?? throw new ArgumentNullException(nameof(paymentService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Payment/Submit/{orderId}
    // =========================================================================

    /// <summary>
    /// Renders the payment submission page for an order.
    /// Displays the correct upload form based on the order's payment method
    /// selected at checkout, and shows existing proof if already submitted.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Submit(
        int orderId,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        PaymentDetailDto? detail =
            await _paymentService.GetPaymentDetailAsync(orderId, userId, cancellationToken);

        if (detail is null)
        {
            TempData["error"] = "Order not found.";
            return RedirectToAction("History", "Order");
        }

        ViewData["Title"] = $"Payment — {detail.OrderNumber}";
        return View("~/Views/Customer/Payment.cshtml", detail);
    }

    // =========================================================================
    // POST /Payment/SubmitGCash
    // =========================================================================

    /// <summary>
    /// Receives GCash proof submission (reference number + screenshot).
    /// On success, redirects to the order confirmation page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitGCash(
        int orderId,
        string gcashRefNumber,
        IFormFile? screenshot,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        if (screenshot is null || screenshot.Length == 0)
        {
            TempData["error"] = "Please upload your GCash payment screenshot.";
            return RedirectToAction(nameof(Submit), new { orderId });
        }

        try
        {
            ServiceResult result = await _paymentService.SubmitGCashPaymentAsync(
                orderId, userId, gcashRefNumber ?? string.Empty,
                screenshot, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Submit), new { orderId });
            }

            TempData["success"] =
                "GCash payment submitted. We'll verify it and update your order shortly.";
            return RedirectToAction("Confirmation", "Order", new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitGCash failed for order {OrderId}.", orderId);
            TempData["error"] = "Unable to submit payment. Please try again.";
            return RedirectToAction(nameof(Submit), new { orderId });
        }
    }

    // =========================================================================
    // POST /Payment/SubmitBankTransfer
    // =========================================================================

    /// <summary>
    /// Receives BPI bank transfer proof submission (reference number + proof file).
    /// On success, redirects to the order confirmation page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitBankTransfer(
        int orderId,
        string bpiReferenceNumber,
        IFormFile? proofFile,
        CancellationToken cancellationToken)
    {
        int userId = GetCurrentUserId();

        if (proofFile is null || proofFile.Length == 0)
        {
            TempData["error"] =
                "Please upload your bank transfer proof (image or PDF).";
            return RedirectToAction(nameof(Submit), new { orderId });
        }

        try
        {
            ServiceResult result = await _paymentService.SubmitBankTransferPaymentAsync(
                orderId, userId, bpiReferenceNumber ?? string.Empty,
                proofFile, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Submit), new { orderId });
            }

            TempData["success"] =
                "Bank transfer proof submitted. We'll verify it within 24 hours.";
            return RedirectToAction("Confirmation", "Order", new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "SubmitBankTransfer failed for order {OrderId}.", orderId);
            TempData["error"] = "Unable to submit payment. Please try again.";
            return RedirectToAction(nameof(Submit), new { orderId });
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