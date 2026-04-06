using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using Dapper;

namespace AdminSystem_v2.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        // ── SQL fragments shared between methods ──────────────────────────────

        private const string OrderListSelect =
            @"SELECT
                o.OrderId, o.UserId, o.OrderNumber, o.OrderDate, o.OrderStatus,
                o.SubTotal, o.DiscountAmount, o.ShippingFee,
                o.ContactPhone, o.DeliveryInstructions, o.IsWalkIn,
                o.CreatedAt, o.UpdatedAt,
                u.FirstName + ' ' + u.LastName  AS CustomerName,
                u.Email                          AS CustomerEmail,
                CASE WHEN po.PickupOrderId IS NOT NULL THEN 'Pickup' ELSE 'Delivery' END
                                                 AS DeliveryType,
                (SELECT COUNT(*) FROM vw_OrderItemDetail vi WHERE vi.OrderId = o.OrderId)
                                                 AS ItemCount
              FROM [Order] o
              INNER JOIN [User]         u  ON o.UserId  = u.UserId
              LEFT  JOIN PickupOrder    po ON o.OrderId = po.OrderId
              WHERE o.IsWalkIn = 0";

        // ── Public methods ────────────────────────────────────────────────────

        public async Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null)
        {
            bool applyFilter = !string.IsNullOrEmpty(statusFilter)
                            && statusFilter != "All";

            string sql = OrderListSelect
                       + (applyFilter ? " AND o.OrderStatus = @Status" : string.Empty)
                       + " ORDER BY o.OrderDate DESC";

            await using var conn = GetConnection();
            return await conn.QueryAsync<Order>(
                sql,
                applyFilter ? new { Status = statusFilter } : null);
        }

        public async Task<Order?> GetOrderDetailAsync(int orderId)
        {
            await using var conn = GetConnection();

            // 1 — base order row
            var order = await conn.QueryFirstOrDefaultAsync<Order>(
                OrderListSelect + " AND o.OrderId = @Id",
                new { Id = orderId });

            if (order == null) return null;

            // 2 — line items (from view)
            order.Items = (await conn.QueryAsync<OrderItem>(
                @"SELECT OrderItemId, OrderId, ProductId, ProductVariantId,
                         ProductName, VariantName, Quantity, UnitPrice, Subtotal
                  FROM vw_OrderItemDetail
                  WHERE OrderId = @Id
                  ORDER BY OrderItemId",
                new { Id = orderId })).ToList();

            // 3 — delivery record (if delivery order)
            if (order.DeliveryType == "Delivery")
            {
                order.Delivery = await conn.QueryFirstOrDefaultAsync<DeliveryDetail>(
                    @"SELECT DeliveryId, OrderId, Courier, DeliveryStatus,
                             IsDelayed, DelayedUntil, EstimatedDeliveryTime,
                             ActualDeliveryTime, CreatedAt,
                             LalamoveBookingRef, DriverName, DriverPhone,
                             LbcTrackingNumber
                      FROM vw_DeliveryDetail
                      WHERE OrderId = @Id",
                    new { Id = orderId });
            }

            // 4 — pickup record (if pickup order)
            if (order.DeliveryType == "Pickup")
            {
                order.Pickup = await conn.QueryFirstOrDefaultAsync<PickupOrder>(
                    @"SELECT PickupOrderId, OrderId, PickupReadyAt,
                             PickupExpiresAt, PickupConfirmedAt
                      FROM PickupOrder
                      WHERE OrderId = @Id",
                    new { Id = orderId });
            }

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string status)
            => await ExecuteAsync(
                @"UPDATE [Order]
                  SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                  WHERE OrderId = @OrderId",
                new { OrderId = orderId, Status = status });

        public async Task MarkReadyForPickupAsync(int orderId)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // Update order status
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = OrderStatuses.ReadyForPickup }, tx);

                // Upsert PickupOrder — set ready time and 7-day expiry window
                await conn.ExecuteAsync(
                    @"IF NOT EXISTS (SELECT 1 FROM PickupOrder WHERE OrderId = @OrderId)
                          INSERT INTO PickupOrder (OrderId, PickupReadyAt, PickupExpiresAt)
                          VALUES (@OrderId, GETUTCDATE(), DATEADD(DAY, 7, GETUTCDATE()))
                      ELSE
                          UPDATE PickupOrder
                          SET PickupReadyAt   = GETUTCDATE(),
                              PickupExpiresAt = DATEADD(DAY, 7, GETUTCDATE())
                          WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task ConfirmPickupAsync(int orderId)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = OrderStatuses.PickedUp }, tx);

                await conn.ExecuteAsync(
                    @"UPDATE PickupOrder
                      SET PickupConfirmedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<Dictionary<string, int>> GetStatusCountsAsync()
        {
            const string sql =
                @"SELECT OrderStatus, COUNT(*) AS Cnt
                  FROM [Order]
                  WHERE IsWalkIn = 0
                  GROUP BY OrderStatus";

            await using var conn = GetConnection();
            var rows = await conn.QueryAsync<StatusCountRow>(sql);
            return rows.ToDictionary(r => r.OrderStatus, r => r.Cnt);
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private sealed class StatusCountRow
        {
            public string OrderStatus { get; set; } = string.Empty;
            public int    Cnt         { get; set; }
        }
    }
}
