using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private const string SessionUserId = "UserId";

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET /Order
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login", "Customer");

            var orders = await _orderService.GetOrderHistoryAsync(userId.Value);
            return View(orders);
        }

        // GET /Order/Details/{id}
        public async Task<IActionResult> Details(int id)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login", "Customer");

            var order = await _orderService.GetOrderDetailsAsync(id, userId.Value);
            if (order == null) return NotFound();

            return View(order);
        }
    }
}
