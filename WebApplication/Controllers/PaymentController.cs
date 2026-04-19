// WebApplication/Controllers/PaymentController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles GCash and BankTransfer payment proof submission.
/// Cash is POS-only — no Cash actions here.
/// Flowchart: Part 3 — Payment Processing.
/// </summary>
[Authorize]
public sealed class PaymentController : Controller
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<PaymentController> _logger;

    public PaymentController(
        IPaymentService paymentService,
        ILogger<PaymentController> logger)
    {
        _paymentService = paymentService ?? throw new ArgumentNullException(nameof(paymentService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Payment/Submit/{orderId}
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> Submit(
        int orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            PaymentDetailDto? vm = await _paymentService.GetPaymentDetailAsync(
                orderId, GetCurrentUserId(), cancellationToken);

            if (vm is null)
            {
                TempData["error"] = "Order not found.";
                return RedirectToAction("History", "Order");
            }

            if (vm.HasExistingPayment)
            {
                TempData["info"] = "Payment already submitted. Awaiting verification.";
                return RedirectToAction("Detail", "Order", new { orderId });
            }

            ViewData["Title"] = $"Submit Payment — {vm.OrderNumber}";
            return View("~/Views/Customer/Payment.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading payment page for order {OrderId}.", orderId);
            TempData["error"] = "Unable to load payment page. Please try again.";
            return RedirectToAction("History", "Order");
        }
    }

    // =========================================================================
    // POST /Payment/SubmitGCash
    // =========================================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(20 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 15 * 1024 * 1024)]
    public async Task<IActionResult> SubmitGCash(
        int orderId,
        string gcashNumber,
        string referenceNumber,
        IFormFile screenshotFile,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _paymentService.SubmitGCashPaymentAsync(
                orderId, GetCurrentUserId(),
                referenceNumber,
                screenshotFile, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Submit), new { orderId });
            }

            TempData["success"] =
                "Payment submitted successfully. We'll verify it and update your order shortly.";
            return RedirectToAction("Detail", "Order", new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SubmitGCash failed for order {OrderId}.", orderId);
            TempData["error"] = "An unexpected error occurred. Please try again.";
            return RedirectToAction(nameof(Submit), new { orderId });
        }
    }

    // =========================================================================
    // POST /Payment/SubmitBankTransfer
    // =========================================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    [RequestSizeLimit(20 * 1024 * 1024)]
    [RequestFormLimits(MultipartBodyLengthLimit = 15 * 1024 * 1024)]
    public async Task<IActionResult> SubmitBankTransfer(
        int orderId,
        string depositorName,
        string? bpiReferenceNumber,
        IFormFile depositSlipFile,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _paymentService.SubmitBankTransferPaymentAsync(
                orderId, GetCurrentUserId(),
                bpiReferenceNumber ?? string.Empty,
                depositSlipFile, cancellationToken);

            if (!result.IsSuccess)
            {
                TempData["error"] = result.Error;
                return RedirectToAction(nameof(Submit), new { orderId });
            }

            TempData["success"] =
                "Deposit details submitted. Verification may take up to 24 hours.";
            return RedirectToAction("Detail", "Order", new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SubmitBankTransfer failed for order {OrderId}.", orderId);
            TempData["error"] = "An unexpected error occurred. Please try again.";
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