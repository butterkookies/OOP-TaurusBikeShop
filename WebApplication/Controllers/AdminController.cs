using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext    _context;
        private readonly IPaymentService _paymentService;
        private readonly ISupportService _supportService;
        private const string SessionAdminId = "AdminId";

        public AdminController(AppDbContext context, IPaymentService paymentService, ISupportService supportService)
        {
            _context        = context;
            _paymentService = paymentService;
            _supportService = supportService;
        }

        private bool IsAdminLoggedIn() => HttpContext.Session.GetString("IsAdmin") == "true";

        // GET /Admin/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (IsAdminLoggedIn())
                return RedirectToAction("Dashboard");
            return View();
        }

        // POST /Admin/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Email and password are required.";
                return View();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.IsAdmin && u.IsActive);

            if (user == null)
            {
                ViewBag.Error = "Invalid credentials or not an admin account.";
                return View();
            }

            bool validPassword = PasswordHelper.VerifyPassword(password, user.PasswordHash ?? string.Empty);
            if (!validPassword)
            {
                ViewBag.Error = "Invalid email or password.";
                return View();
            }

            HttpContext.Session.SetString("IsAdmin", "true");
            HttpContext.Session.SetInt32(SessionAdminId, user.UserId);

            return RedirectToAction("Dashboard");
        }

        // GET /Admin/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // GET /Admin/Dashboard
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var orderCount   = await _context.Orders.CountAsync();
            var userCount    = await _context.Users.CountAsync(u => !u.IsAdmin);
            var pendingCount = await _context.Payments.CountAsync(p => p.PaymentStatus == "PendingVerification");
            var totalRevenue = await _context.Payments
                .Where(p => p.PaymentStatus == "Completed")
                .SumAsync(p => p.Amount);

            ViewBag.OrderCount   = orderCount;
            ViewBag.UserCount    = userCount;
            ViewBag.PendingCount = pendingCount;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }

        // GET /Admin/Orders
        [HttpGet]
        public async Task<IActionResult> Orders(string? status)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var query = _context.Orders
                .Include(o => o.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(o => o.OrderStatus == status);

            var orders = await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            ViewBag.StatusFilter = status;
            return View(orders);
        }

        // GET /Admin/OrderDetail/{orderId}
        [HttpGet]
        public async Task<IActionResult> OrderDetail(int orderId)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Include(o => o.Payments)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST /Admin/ApprovePayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePayment(int paymentId, int orderId)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            await _paymentService.ApproveAsync(paymentId);
            return RedirectToAction("OrderDetail", new { orderId });
        }

        // POST /Admin/RejectPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPayment(int paymentId, int orderId, string reason)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            await _paymentService.RejectAsync(paymentId, reason);
            return RedirectToAction("OrderDetail", new { orderId });
        }

        // GET /Admin/Payments
        [HttpGet]
        public async Task<IActionResult> Payments()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var payments = await _paymentService.GetPendingPaymentsAsync();
            return View(payments);
        }

        // GET /Admin/Support
        [HttpGet]
        public async Task<IActionResult> Support()
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            var tickets = await _supportService.GetAllTicketsAsync();
            return View("~/Views/Admin/Support.cshtml", tickets);
        }

        // POST /Admin/RespondToTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RespondToTicket(int ticketId, string response, string status)
        {
            if (!IsAdminLoggedIn())
                return RedirectToAction("Login");

            await _supportService.RespondAsync(ticketId, response, status);
            TempData["Success"] = "Response saved.";
            return RedirectToAction(nameof(Support));
        }
    }
}
