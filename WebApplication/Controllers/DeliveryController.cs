using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class DeliveryController : Controller
    {
        private readonly IDeliveryService _deliveryService;
        private readonly IOrderService    _orderService;

        private const string SessionUserId = "UserId";

        public DeliveryController(IDeliveryService deliveryService, IOrderService orderService)
        {
            _deliveryService = deliveryService;
            _orderService    = orderService;
        }

        // GET /Delivery/Track/{orderId}
        [HttpGet]
        public async Task<IActionResult> Track(int orderId)
        {
            int? userId = HttpContext.Session.GetInt32(SessionUserId);
            if (userId == null) return RedirectToAction("Login", "Customer");

            // Verify order belongs to the logged-in user
            var orderDetails = await _orderService.GetOrderDetailsAsync(orderId, userId.Value);
            if (orderDetails == null) return NotFound();

            var delivery = await _deliveryService.GetByOrderIdAsync(orderId);

            ViewBag.OrderId      = orderId;
            ViewBag.OrderNumber  = orderDetails.OrderNumber;
            ViewBag.OrderStatus  = orderDetails.OrderStatus;
            ViewBag.Delivery     = delivery;

            return View("~/Views/Customer/DeliveryTracking.cshtml");
        }
    }
}
