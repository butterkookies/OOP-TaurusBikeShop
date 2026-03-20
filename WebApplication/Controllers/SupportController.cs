// WebApplication/Controllers/SupportController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles customer-facing support ticket creation, listing, and detail.
/// Flowchart: Part 13 — Support System.
/// </summary>
[Authorize]
public sealed class SupportController : Controller
{
    private readonly ISupportService _supportService;
    private readonly ILogger<SupportController> _logger;

    public SupportController(
        ISupportService supportService,
        ILogger<SupportController> logger)
    {
        _supportService = supportService
            ?? throw new ArgumentNullException(nameof(supportService));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Support — ticket list
    // =========================================================================

    /// <summary>
    /// Renders the customer's support ticket list.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<SupportTicketViewModel> tickets =
                await _supportService.GetByUserAsync(
                    GetCurrentUserId(), cancellationToken);

            ViewData["Title"] = "Support Tickets";
            return View("~/Views/Customer/SupportList.cshtml", tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading support tickets for user {UserId}.",
                GetCurrentUserId());
            TempData["error"] = "Unable to load support tickets.";
            return RedirectToAction("Dashboard", "Customer");
        }
    }

    // =========================================================================
    // GET /Support/Create
    // =========================================================================

    /// <summary>
    /// Renders the create ticket form.
    /// Pre-populates OrderId when opened from the Order Detail page.
    /// </summary>
    [HttpGet]
    public IActionResult Create(int? orderId = null)
    {
        ViewData["Title"] = "New Support Ticket";
        return View("~/Views/Customer/SupportCreate.cshtml",
            new SupportCreateViewModel { OrderId = orderId });
    }

    // =========================================================================
    // POST /Support/Create
    // =========================================================================

    /// <summary>
    /// Processes the create ticket form.
    /// On success redirects to the new ticket's detail page.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        SupportCreateViewModel vm,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "New Support Ticket";
            return View("~/Views/Customer/SupportCreate.cshtml", vm);
        }

        try
        {
            ServiceResult<int> result = await _supportService.CreateAsync(
                GetCurrentUserId(), vm, cancellationToken);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, result.Error!);
                ViewData["Title"] = "New Support Ticket";
                return View("~/Views/Customer/SupportCreate.cshtml", vm);
            }

            TempData["success"] = "Support ticket created. We'll respond shortly.";
            return RedirectToAction(nameof(Detail), new { ticketId = result.Value });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create support ticket failed for user {UserId}.",
                GetCurrentUserId());
            TempData["error"] = "Unable to create ticket. Please try again.";
            return View("~/Views/Customer/SupportCreate.cshtml", vm);
        }
    }

    // =========================================================================
    // GET /Support/Detail/{ticketId}
    // =========================================================================

    /// <summary>
    /// Renders the ticket detail page with the full reply thread.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Detail(
        int ticketId,
        CancellationToken cancellationToken)
    {
        try
        {
            SupportTicketViewModel? vm = await _supportService.GetDetailAsync(
                ticketId, GetCurrentUserId(), cancellationToken);

            if (vm is null)
            {
                TempData["error"] = "Ticket not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = $"Ticket #{ticketId} — {vm.Subject}";
            return View("~/Views/Customer/SupportDetail.cshtml", vm);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading ticket {TicketId}.", ticketId);
            TempData["error"] = "Unable to load ticket.";
            return RedirectToAction(nameof(Index));
        }
    }

    // =========================================================================
    // POST /Support/Reply
    // =========================================================================

    /// <summary>
    /// Adds a customer reply to an existing ticket.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reply(
        int ticketId,
        string message,
        CancellationToken cancellationToken)
    {
        try
        {
            ServiceResult result = await _supportService.AddReplyAsync(
                ticketId, GetCurrentUserId(), message, cancellationToken);

            TempData[result.IsSuccess ? "success" : "error"] =
                result.IsSuccess ? "Reply sent." : result.Error;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Reply failed for ticket {TicketId}.", ticketId);
            TempData["error"] = "Unable to send reply. Please try again.";
        }

        return RedirectToAction(nameof(Detail), new { ticketId });
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