// WebApplication/Controllers/SupportController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Handles the customer-facing support ticket pages: list, detail, and create.
/// All actions require authentication — support tickets are customer-specific.
/// </summary>
[Authorize]
public sealed class SupportController : Controller
{
    private readonly ISupportService _supportService;
    private readonly ILogger<SupportController> _logger;

    /// <inheritdoc/>
    public SupportController(
        ISupportService supportService,
        ILogger<SupportController> logger)
    {
        _supportService = supportService ?? throw new ArgumentNullException(nameof(supportService));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Support  — ticket list
    // =========================================================================

    /// <summary>
    /// Renders the authenticated customer's support ticket list.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();

        IReadOnlyList<SupportTicketListItemViewModel> tickets =
            await _supportService.GetUserTicketsAsync(userId, cancellationToken);

        SupportListViewModel vm = new() { Tickets = tickets };

        ViewData["Title"] = "My Support Tickets";
        return View("~/Views/Customer/SupportList.cshtml", vm);
    }

    // =========================================================================
    // GET /Support/{ticketId}  — ticket detail
    // =========================================================================

    /// <summary>
    /// Renders the full detail and reply thread for a single support ticket.
    /// Returns 404 when the ticket does not exist or does not belong to the
    /// authenticated user.
    /// </summary>
    [HttpGet("{controller}/{ticketId:int}")]
    public async Task<IActionResult> Detail(
        int ticketId,
        CancellationToken cancellationToken = default)
    {
        int userId = GetCurrentUserId();

        SupportTicketDetailViewModel? vm =
            await _supportService.GetTicketDetailAsync(ticketId, userId, cancellationToken);

        if (vm is null)
            return NotFound();

        return View("~/Views/Customer/SupportDetail.cshtml", vm);
    }

    // =========================================================================
    // GET /Support/Create  — create form
    // =========================================================================

    /// <summary>
    /// Renders the new support ticket form.
    /// Accepts an optional <paramref name="orderId"/> query parameter so that
    /// the form can be pre-populated when navigating from an order detail page.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create(
        int? orderId = null,
        CancellationToken cancellationToken = default)
    {
        SupportCreateViewModel vm = new()
        {
            OrderId = orderId
        };

        // If pre-linked to an order, look up the order number for display
        if (orderId.HasValue)
        {
            // Order number is display-only; fetch it cheaply via a direct query
            // rather than injecting a full OrderService dependency.
            // The FK is carried by the hidden OrderId field.
            vm.OrderNumber = TempData["OrderNumber"] as string;
        }

        ViewData["Title"] = "New Support Ticket";
        return await Task.FromResult(View("~/Views/Customer/SupportCreate.cshtml", vm));
    }

    // =========================================================================
    // POST /Support/Create  — create submission
    // =========================================================================

    /// <summary>
    /// Handles the support ticket creation form submission.
    /// Uploads any attachment to GCS and inserts the ticket row.
    /// On success, redirects to the ticket list with a success message.
    /// On failure, re-renders the form with an error message.
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        SupportCreateViewModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View("~/Views/Customer/SupportCreate.cshtml", model);
        }

        int userId = GetCurrentUserId();

        Microsoft.AspNetCore.Http.IFormFile? attachment =
            Request.Form.Files.GetFile("attachment");

        ServiceResult<int> result = await _supportService.CreateTicketAsync(
            userId,
            model.Category!,
            model.Subject!,
            model.Description,
            model.OrderId,
            attachment,
            cancellationToken);

        if (!result.IsSuccess)
        {
            ModelState.AddModelError(string.Empty, result.Error!);
            return View("~/Views/Customer/SupportCreate.cshtml", model);
        }

        TempData["success"] = $"Ticket #{result.Value} has been created. We'll be in touch soon.";
        return RedirectToAction(nameof(Index));
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
