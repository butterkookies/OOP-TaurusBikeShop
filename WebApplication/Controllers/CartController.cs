using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private const string SessionUserId = "UserId";

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        // GET /Cart
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return RedirectToAction("Login", "Customer");

            var cart = await _cartService.GetCartAsync(userId.Value);
            return View("~/Views/Customer/Cart.cshtml", cart);
        }

        // GET /Cart/GetItems
        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return Json(new { success = false });

            var cart = await _cartService.GetCartAsync(userId.Value);
            return Json(new
            {
                success = true,
                items = cart.Items,
                subtotal = cart.Items.Sum(i => i.Quantity * i.UnitPrice)
            });
        }

        // POST /Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null)
                return Json(new { success = false, message = "Please log in to add items to cart." });

            try
            {
                await _cartService.AddToCartAsync(userId.Value, productId, quantity);
                int count = await _cartService.GetCartCountAsync(userId.Value);
                return Json(new { success = true, cartCount = count });
            }
            catch (InvalidOperationException ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST /Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int productId, int quantity)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return Json(new { success = false, message = "Not logged in." });

            await _cartService.UpdateQuantityAsync(userId.Value, productId, quantity);

            var cart = await _cartService.GetCartAsync(userId.Value);
            return Json(new
            {
                success  = true,
                subtotal = cart.Subtotal,
                shipping = cart.ShippingFee,
                total    = cart.Total
            });
        }

        // POST /Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return Json(new { success = false, message = "Not logged in." });

            await _cartService.RemoveFromCartAsync(userId.Value, productId);

            var cart = await _cartService.GetCartAsync(userId.Value);
            return Json(new
            {
                success  = true,
                subtotal = cart.Subtotal,
                shipping = cart.ShippingFee,
                total    = cart.Total
            });
        }

        // GET /Cart/Count
        [HttpGet]
        public async Task<IActionResult> Count()
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return Json(new { count = 0 });

            int count = await _cartService.GetCartCountAsync(userId.Value);
            return Json(new { count });
        }
    }
}
