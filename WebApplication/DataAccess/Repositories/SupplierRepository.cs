// WebApplication/DataAccess/Repositories/SupplierRepository.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="Supplier"/>, <see cref="PurchaseOrder"/>,
/// and <see cref="PurchaseOrderItem"/> entities.
/// The WebApplication uses this repository as read-only.
/// The AdminSystem uses the shared database directly via Dapper — but
/// <see cref="ReceiveStockAsync"/> is implemented here so the EF Core
/// transaction logic is available to any process using this context.
/// </summary>
public sealed class SupplierRepository : Repository<Supplier>
{
    /// <inheritdoc/>
    public SupplierRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Returns all active suppliers ordered by name.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>All active suppliers.</returns>
    public async Task<IReadOnlyList<Supplier>> GetActiveSuppliersAsync(
        CancellationToken cancellationToken = default)
    {
        return await Context.Suppliers
            .AsNoTracking()
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Returns all purchase orders for a specific supplier,
    /// ordered by most recent first, with line items included.
    /// </summary>
    /// <param name="supplierId">The supplier ID to retrieve orders for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Purchase orders with items included.</returns>
    public async Task<IReadOnlyList<PurchaseOrder>> GetPurchaseOrdersAsync(
        int supplierId,
        CancellationToken cancellationToken = default)
    {
        return await Context.PurchaseOrders
            .AsNoTracking()
            .Include(po => po.Items)
                .ThenInclude(poi => poi.Product)
            .Include(po => po.Items)
                .ThenInclude(poi => poi.Variant)
            .Where(po => po.SupplierId == supplierId)
            .OrderByDescending(po => po.OrderDate)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Creates a purchase order with all its line items in a single transaction.
    /// </summary>
    /// <param name="po">The purchase order header record.</param>
    /// <param name="items">The line items belonging to the purchase order.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The created purchase order with its generated ID.</returns>
    public async Task<PurchaseOrder> CreatePurchaseOrderAsync(
        PurchaseOrder po,
        IEnumerable<PurchaseOrderItem> items,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(po);
        ArgumentNullException.ThrowIfNull(items);

        await using IDbContextTransaction transaction =
            await Context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            await Context.PurchaseOrders.AddAsync(po, cancellationToken);
            await Context.SaveChangesAsync(cancellationToken);

            foreach (PurchaseOrderItem item in items)
            {
                item.PurchaseOrderId = po.PurchaseOrderId;
                await Context.PurchaseOrderItems.AddAsync(item, cancellationToken);
            }

            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            return po;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Processes stock receipt for a purchase order within a single DB transaction:
    /// <list type="number">
    ///   <item>Updates <c>ProductVariant.StockQuantity</c> for each received item.</item>
    ///   <item>Writes an <c>InventoryLog</c> Purchase entry per item.</item>
    ///   <item>Sets <c>PurchaseOrder.Status</c> = Received.</item>
    /// </list>
    /// This is the critical stock-receiving transaction — any failure rolls back all changes.
    /// </summary>
    /// <param name="purchaseOrderId">The purchase order being received.</param>
    /// <param name="receivedItems">
    /// A collection of (variantId, receivedQty) tuples for each item received.
    /// Use the ProductVariantId from the PurchaseOrderItem.
    /// </param>
    /// <param name="receivedByUserId">
    /// The admin user receiving the stock. Written to InventoryLog.ChangedByUserId.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the purchase order does not exist.
    /// </exception>
    public async Task ReceiveStockAsync(
        int purchaseOrderId,
        IEnumerable<(int VariantId, int ReceivedQty)> receivedItems,
        int receivedByUserId,
        CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction transaction =
            await Context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            PurchaseOrder? po = await Context.PurchaseOrders
                .Include(p => p.Items)
                .FirstOrDefaultAsync(p => p.PurchaseOrderId == purchaseOrderId, cancellationToken);

            if (po is null)
                throw new InvalidOperationException(
                    $"PurchaseOrder {purchaseOrderId} not found.");

            foreach ((int variantId, int receivedQty) in receivedItems)
            {
                if (receivedQty <= 0)
                    continue;

                ProductVariant? variant = await Context.ProductVariants
                    .FirstOrDefaultAsync(v => v.ProductVariantId == variantId, cancellationToken);

                if (variant is null)
                    continue;

                // Update stock
                variant.StockQuantity += receivedQty;
                variant.UpdatedAt = DateTime.UtcNow;

                // Write immutable InventoryLog entry
                InventoryLog log = new()
                {
                    ProductId = variant.ProductId,
                    ProductVariantId = variantId,
                    ChangeQuantity = receivedQty,
                    ChangeType = InventoryChangeTypes.Purchase,
                    PurchaseOrderId = purchaseOrderId,
                    ChangedByUserId = receivedByUserId,
                    Notes = $"Stock received from PurchaseOrder #{purchaseOrderId}",
                    CreatedAt = DateTime.UtcNow
                };

                await Context.InventoryLogs.AddAsync(log, cancellationToken);
            }

            po.Status = PurchaseOrderStatuses.Received;
            await Context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}