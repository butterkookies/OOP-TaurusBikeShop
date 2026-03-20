using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IOrderService _orderService;
        private const string SessionUserId = "UserId";

        public CheckoutController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login", "Customer");

            return View(new CheckoutViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(CheckoutViewModel model)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login", "Customer");

            if (!ModelState.IsValid) return View(model);

            try
            {
                var order = await _orderService.PlaceOrderAsync(userId.Value, model);
                return RedirectToAction("Confirmation", "Customer", new { orderNumber = order.OrderNumber });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }
    }
}
