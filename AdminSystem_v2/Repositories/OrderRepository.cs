using System.Data.Common;
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
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // 1 — Update order status
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = status }, tx);

                // 2 — Queue a notification for the customer
                await QueueOrderNotificationAsync(conn, tx, orderId, status);

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

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

                // Queue notification
                await QueueOrderNotificationAsync(conn, tx, orderId, OrderStatuses.ReadyForPickup);

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

                // Queue notification
                await QueueOrderNotificationAsync(conn, tx, orderId, OrderStatuses.PickedUp);

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

        /// <summary>
        /// Inserts a Notification row for the customer who owns the order.
        /// Runs inside the caller's existing transaction so the notification
        /// is committed atomically with the status change.
        /// </summary>
        private static async Task QueueOrderNotificationAsync(
            DbConnection conn, DbTransaction tx, int orderId, string status)
        {
            // Look up customer details for this order
            var info = await conn.QueryFirstOrDefaultAsync<OrderOwnerInfo>(
                @"SELECT o.UserId, u.Email, o.OrderNumber
                  FROM [Order] o
                  INNER JOIN [User] u ON o.UserId = u.UserId
                  WHERE o.OrderId = @OrderId",
                new { OrderId = orderId }, tx);

            if (info is null) return;

            string notifType = status switch
            {
                OrderStatuses.ReadyForPickup => "ReadyForPickup",
                OrderStatuses.PickedUp       => "DeliveryConfirmation",
                OrderStatuses.Shipped        => "TrackingUpdate",
                OrderStatuses.Delivered      => "DeliveryConfirmation",
                OrderStatuses.Cancelled      => "TrackingUpdate",
                _                            => "TrackingUpdate",
            };

            string subject = $"Order {info.OrderNumber} — {status}";
            string body = status switch
            {
                OrderStatuses.Processing     => $"Your order {info.OrderNumber} is now being processed.",
                OrderStatuses.ReadyForPickup => $"Your order {info.OrderNumber} is ready for pickup.",
                OrderStatuses.PickedUp       => $"Your order {info.OrderNumber} has been picked up.",
                OrderStatuses.Shipped        => $"Your order {info.OrderNumber} has been shipped.",
                OrderStatuses.Delivered      => $"Your order {info.OrderNumber} has been delivered.",
                OrderStatuses.Cancelled      => $"Your order {info.OrderNumber} has been cancelled.",
                _                            => $"Your order {info.OrderNumber} status changed to {status}.",
            };

            await conn.ExecuteAsync(
                @"INSERT INTO Notification
                    (UserId, OrderId, Channel, NotifType, Recipient,
                     Subject, Body, Status, RetryCount, CreatedAt, IsRead)
                  VALUES
                    (@UserId, @OrderId, 'Email', @NotifType, @Email,
                     @Subject, @Body, 'Pending', 0, GETUTCDATE(), 0)",
                new
                {
                    info.UserId,
                    OrderId = orderId,
                    NotifType = notifType,
                    info.Email,
                    Subject = subject,
                    Body = body,
                }, tx);
        }

        private sealed class OrderOwnerInfo
        {
            public int    UserId      { get; set; }
            public string Email       { get; set; } = string.Empty;
            public string OrderNumber { get; set; } = string.Empty;
        }
    }
}
