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
            if (userId == null) return Unauthorized();

            await _cartService.UpdateQuantityAsync(userId.Value, productId, quantity);
            return Ok();
        }

        // POST /Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(int productId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return Unauthorized();

            await _cartService.RemoveFromCartAsync(userId.Value, productId);
            return Ok();
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
