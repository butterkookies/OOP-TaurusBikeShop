// WebApplication/Controllers/OrderController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles order confirmation, history, detail, and customer cancellation.
/// All views live in Views/Customer/ — explicit paths used on every return.
/// Flowchart: Part 4 — Order Tracking.
/// </summary>
[Authorize]
public sealed class OrderController : Controller
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderController> _logger;

    private const int HistoryPageSize = 10;

    public OrderController(
        IOrderService orderService,
        ILogger<OrderController> logger)
    {
        _orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Order/Confirmation/{orderId}
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> Confirmation(
        int orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            OrderViewModel? vm = await _orderService.GetOrderConfirmationAsync(
                orderId, GetCurrentUserId(), cancellationToken);

            if (vm is null)
            {
                TempData["error"] = "Order not found.";
                return RedirectToAction(nameof(History));
            }

            ViewData["Title"] = $"Order Confirmed — {vm.OrderNumber}";
            return View("~/Views/Customer/Confirmation.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading confirmation for order {OrderId}.", orderId);
            TempData["error"] = "Unable to load order confirmation.";
            return RedirectToAction(nameof(History));
        }
    }

    // =========================================================================
    // GET /Order/History
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> History(
        int page = 1,
        CancellationToken cancellationToken = default)
    {
        try
        {
            (IReadOnlyList<OrderViewModel> orders, int totalCount) =
                await _orderService.GetOrderHistoryAsync(
                    GetCurrentUserId(), page, HistoryPageSize, cancellationToken);

            int totalPages = (int)Math.Ceiling(totalCount / (double)HistoryPageSize);

            ViewData["Title"]   = "My Orders";
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages  = totalPages;
            ViewBag.TotalCount  = totalCount;

            return View("~/Views/Customer/OrderHistory.cshtml", orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading order history for user {UserId}.",
                GetCurrentUserId());
            TempData["error"] = "Unable to load order history.";
            return View("~/Views/Customer/OrderHistory.cshtml",
                Array.Empty<OrderViewModel>());
        }
    }

    // =========================================================================
    // GET /Order/Detail/{orderId}
    // =========================================================================

    [HttpGet]
    public async Task<IActionResult> Detail(
        int orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            OrderViewModel? vm = await _orderService.GetOrderDetailAsync(
                orderId, GetCurrentUserId(), cancellationToken);

            if (vm is null)
            {
                TempData["error"] = "Order not found.";
                return RedirectToAction(nameof(History));
            }

            ViewData["Title"] = $"Order {vm.OrderNumber}";
            return View("~/Views/Customer/OrderDetail.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading order detail for order {OrderId}.", orderId);
            TempData["error"] = "Unable to load order details.";
            return RedirectToAction(nameof(History));
        }
    }

    // =========================================================================
    // POST /Order/Cancel
    // =========================================================================

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(
        int orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _orderService.CancelOrderAsync(
                orderId, GetCurrentUserId(), cancellationToken);

            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess
                    ? "Your order has been cancelled."
                    : result.Error;

            return RedirectToAction(nameof(Detail), new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Cancel failed for order {OrderId}.", orderId);
            TempData["error"] = "Unable to cancel order. Please try again.";
            return RedirectToAction(nameof(Detail), new { orderId });
        }
    }

    // =========================================================================
    // POST /Order/ConfirmDelivery
    // =========================================================================

    /// <summary>
    /// Customer confirms they have physically received their shipped order.
    /// Transitions status Shipped → Delivered and finalises inventory.
    /// Flowchart: Part 6 — D26 (Confirm Order Received) → D28A / D28B.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmDelivery(
        int orderId,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _orderService.ConfirmDeliveryAsync(
                orderId, GetCurrentUserId(), cancellationToken);

            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess
                    ? "Thank you! Your delivery has been confirmed."
                    : result.Error;

            return RedirectToAction(nameof(Detail), new { orderId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmDelivery failed for order {OrderId}.", orderId);
            TempData["error"] = "Unable to confirm delivery. Please try again.";
            return RedirectToAction(nameof(Detail), new { orderId });
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