using Microsoft.AspNetCore.Mvc;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.Controllers
{
    public class InventoryController : Controller
    {
        readonly IInventoryService _inventoryService;
        readonly ILogger<InventoryController> _logger;

        public InventoryController(IInventoryService inventoryService, ILogger<InventoryController> logger)
        {
            _inventoryService = inventoryService;
            _logger           = logger;
        }

        // ─── Auth helper ────────────────────────────────────────────
        bool IsAdmin() => HttpContext.Session.GetString("IsAdmin") == "true";

        // GET /Inventory/Index
        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Customer");

            var products = await _inventoryService.GetAllWithStockAsync();
            return View("~/Views/Admin/Inventory.cshtml", products);
        }

        // POST /Inventory/Adjust
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adjust(int productId, int delta, string reason)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Customer");

            try
            {
                await _inventoryService.AdjustStockAsync(productId, delta, reason ?? "Manual adjustment");
                TempData["Success"] = "Stock updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to adjust stock for product {ProductId}.", productId);
                TempData["Error"] = "Failed to update stock. Please try again.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
