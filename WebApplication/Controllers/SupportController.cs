using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class SupportController : Controller
    {
        readonly ISupportService _supportService;
        readonly ILogger<SupportController> _logger;

        const string SessionUserId = "UserId";

        public SupportController(ISupportService supportService, ILogger<SupportController> logger)
        {
            _supportService = supportService;
            _logger         = logger;
        }

        // ─── Auth helper ────────────────────────────────────────────
        int? GetUserId() => HttpContext.Session.GetInt32(SessionUserId);

        // GET /Support/Index
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var tickets = await _supportService.GetUserTicketsAsync(userId.Value);
            return View("~/Views/Customer/Support.cshtml", tickets);
        }

        // POST /Support/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? orderId, string subject, string message)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(message))
            {
                TempData["Error"] = "Subject and message are required.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                await _supportService.CreateTicketAsync(userId.Value, orderId, subject.Trim(), message.Trim());
                TempData["Success"] = "Your support ticket has been submitted.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create support ticket for user {UserId}.", userId);
                TempData["Error"] = "Failed to submit ticket. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET /Support/Detail/{ticketId}
        public async Task<IActionResult> Detail(int ticketId)
        {
            var userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var ticket = await _supportService.GetByIdAsync(ticketId);

            if (ticket == null || ticket.UserId != userId.Value)
                return NotFound();

            return View("~/Views/Customer/SupportDetail.cshtml", ticket);
        }
    }
}
