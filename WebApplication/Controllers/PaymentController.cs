using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly AppDbContext    _context;
        private const string SessionUserId = "UserId";

        public PaymentController(IPaymentService paymentService, AppDbContext context)
        {
            _paymentService = paymentService;
            _context        = context;
        }

        // GET /Payment/Submit/{orderId}
        [HttpGet]
        public async Task<IActionResult> Submit(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.UserId != userId.Value)
                return NotFound();

            var payment = await _paymentService.GetByOrderIdAsync(orderId);

            var dto = new PaymentDetailDto
            {
                OrderId            = order.OrderId,
                OrderNumber        = order.OrderNumber,
                GrandTotal         = order.TotalAmount,
                PaymentMethod      = payment?.PaymentMethod ?? string.Empty,
                PaymentStatus      = payment?.PaymentStatus ?? string.Empty,
                HasExistingPayment = payment != null,
                ProofImageUrl      = payment?.ProofImageUrl,
                RejectionReason    = payment?.RejectionReason
            };

            return View("~/Views/Customer/Payment.cshtml", dto);
        }

        // POST /Payment/SubmitGCash
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitGCash(int orderId, string gcashRefNumber, IFormFile? screenshot)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.UserId != userId.Value)
                return NotFound();

            // Screenshot upload (GCS not configured locally — store null)
            string? proofUrl = null;

            await _paymentService.SubmitGCashAsync(orderId, gcashRefNumber, proofUrl);

            return RedirectToAction("Confirmation", "Customer", new { orderNumber = order.OrderNumber });
        }

        // POST /Payment/SubmitBankTransfer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitBankTransfer(int orderId, string bankRefNumber, IFormFile? screenshot)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var order = await _context.Orders.FindAsync(orderId);
            if (order == null || order.UserId != userId.Value)
                return NotFound();

            // Screenshot upload (GCS not configured locally — store null)
            string? proofUrl = null;

            await _paymentService.SubmitBankTransferAsync(orderId, bankRefNumber, proofUrl);

            return RedirectToAction("Confirmation", "Customer", new { orderNumber = order.OrderNumber });
        }
    }
}
