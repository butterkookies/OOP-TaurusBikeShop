// WebApplication/Controllers/SupplierController.cs

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.Controllers;

/// <summary>
/// Read-only supplier and purchase order views for Admin and Manager roles.
/// Customers never reach these routes.
/// All write operations (create/receive purchase orders) are handled
/// exclusively by AdminSystem via Dapper.
/// </summary>
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public sealed class SupplierController : Controller
{
    private readonly SupplierRepository _supplierRepo;
    private readonly ILogger<SupplierController> _logger;

    public SupplierController(
        SupplierRepository supplierRepo,
        ILogger<SupplierController> logger)
    {
        _supplierRepo = supplierRepo ?? throw new ArgumentNullException(nameof(supplierRepo));
        _logger       = logger       ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // GET /Supplier — list of active suppliers
    // =========================================================================

    /// <summary>
    /// Renders the active supplier list with basic info.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        try
        {
            IReadOnlyList<Supplier> suppliers =
                await _supplierRepo.GetActiveSuppliersAsync(cancellationToken);

            ViewData["Title"] = "Suppliers";
            return View("~/Views/Customer/SupplierList.cshtml", suppliers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading supplier list.");
            TempData["error"] = "Unable to load suppliers.";
            return RedirectToAction("Dashboard", "Customer");
        }
    }

    // =========================================================================
    // GET /Supplier/PurchaseOrders/{supplierId}
    // =========================================================================

    /// <summary>
    /// Renders the purchase order list for a specific supplier.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseOrders(
        int supplierId,
        CancellationToken cancellationToken)
    {
        try
        {
            Supplier? supplier = await _supplierRepo.GetByIdAsync(
                supplierId, cancellationToken);

            if (supplier is null)
            {
                TempData["error"] = "Supplier not found.";
                return RedirectToAction(nameof(Index));
            }

            IReadOnlyList<PurchaseOrder> orders =
                await _supplierRepo.GetPurchaseOrdersAsync(supplierId, cancellationToken);

            ViewData["Title"] = $"Purchase Orders — {supplier.Name}";
            ViewBag.Supplier  = supplier;

            return View("~/Views/Customer/PurchaseOrderList.cshtml", orders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error loading purchase orders for supplier {SupplierId}.", supplierId);
            TempData["error"] = "Unable to load purchase orders.";
            return RedirectToAction(nameof(Index));
        }
    }

    // =========================================================================
    // GET /Supplier/PurchaseOrderDetail/{purchaseOrderId}
    // =========================================================================

    /// <summary>
    /// Renders the detail view for a single purchase order including all line items.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> PurchaseOrderDetail(
        int purchaseOrderId,
        CancellationToken cancellationToken)
    {
        try
        {
            // Load the PO with items via the context directly — SupplierRepository
            // exposes GetPurchaseOrdersAsync by supplierId, but for detail by ID
            // we query the context directly (no write operation needed).
            PurchaseOrder? po = await _supplierRepo.Context.PurchaseOrders
                .Include(p => p.Items)
                    .ThenInclude(i => i.Product)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Variant)
                .Include(p => p.Supplier)
                .Include(p => p.CreatedBy)
                .FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId,
                    cancellationToken);

            if (po is null)
            {
                TempData["error"] = "Purchase order not found.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["Title"] = $"Purchase Order #{purchaseOrderId}";
            return View("~/Views/Customer/PurchaseOrderDetail.cshtml", po);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error loading purchase order {PurchaseOrderId}.", purchaseOrderId);
            TempData["error"] = "Unable to load purchase order.";
            return RedirectToAction(nameof(Index));
        }
    }
}