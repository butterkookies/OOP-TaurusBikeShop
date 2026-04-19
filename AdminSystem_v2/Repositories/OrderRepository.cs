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
                                                 AS ItemCount,
                (SELECT TOP 1 p.PaymentMethod FROM Payment p WHERE p.OrderId = o.OrderId
                 ORDER BY p.CreatedAt DESC)      AS ActualPaymentMethod
              FROM [Order] o
              INNER JOIN [User]         u  ON o.UserId  = u.UserId
              LEFT  JOIN PickupOrder    po ON o.OrderId = po.OrderId
              WHERE o.IsWalkIn = 0";

        // ── Public methods ────────────────────────────────────────────────────

        public async Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null,
                                                              string? typeFilter = null)
        {
            bool applyStatusFilter = !string.IsNullOrEmpty(statusFilter)
                                  && statusFilter != "All";
            bool applyTypeFilter   = !string.IsNullOrEmpty(typeFilter)
                                  && typeFilter != "All";

            string sql = OrderListSelect
                       + (applyStatusFilter ? " AND o.OrderStatus = @Status" : string.Empty)
                       + " ORDER BY o.OrderDate DESC";

            await using var conn = GetConnection();
            var orders = await conn.QueryAsync<Order>(
                sql,
                applyStatusFilter ? new { Status = statusFilter } : null);

            // Apply type filter in-memory (derived from LEFT JOIN, not a direct column)
            if (applyTypeFilter)
                orders = orders.Where(o => o.DeliveryType == typeFilter);

            return orders;
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
            if (order.DeliveryType == OrderTypes.Delivery)
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
            if (order.DeliveryType == OrderTypes.Pickup)
            {
                order.Pickup = await conn.QueryFirstOrDefaultAsync<PickupOrder>(
                    @"SELECT PickupOrderId, OrderId, PickupReadyAt,
                             PickupExpiresAt, PickupConfirmedAt
                      FROM PickupOrder
                      WHERE OrderId = @Id",
                    new { Id = orderId });
            }

            // 5 — payment proof (GCash or BankTransfer, whichever was most recently submitted)
            var proof = await conn.QueryFirstOrDefaultAsync<PaymentProofInfo>(
                @"SELECT TOP 1
                      p.PaymentMethod          AS PaymentProofMethod,
                      p.PaymentStatus          AS PaymentStatus,
                      COALESCE(g.ScreenshotUrl, bt.ProofUrl) AS PaymentProofUrl
                  FROM Payment p
                  LEFT JOIN GCashPayment       g  ON g.PaymentId       = p.PaymentId
                  LEFT JOIN BankTransferPayment bt ON bt.PaymentId      = p.PaymentId
                  WHERE p.OrderId = @Id
                  ORDER BY p.CreatedAt DESC",
                new { Id = orderId });

            if (proof != null)
            {
                order.PaymentProofUrl    = proof.PaymentProofUrl;
                order.PaymentProofMethod = proof.PaymentProofMethod;
                order.PaymentStatus      = proof.PaymentStatus;
            }

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // 1 — Read current status (server-side validation)
                string? currentStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT OrderStatus FROM [Order] WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (currentStatus == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                // 2 — Validate transition against allowed forward-only map
                if (!OrderStatuses.IsValidTransition(currentStatus, newStatus))
                {
                    // Log the rejected transition for audit
                    await LogStatusTransitionAsync(conn, tx, orderId, currentStatus, newStatus,
                                                   success: false, "Transition rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, currentStatus, newStatus);
                }

                // 3 — Apply the status change
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = newStatus }, tx);

                // 4 — Queue a notification for the customer
                await QueueOrderNotificationAsync(conn, tx, orderId, newStatus);

                // 5 — Log successful transition for audit trail
                await LogStatusTransitionAsync(conn, tx, orderId, currentStatus, newStatus,
                                               success: true, null);

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
                // Validate transition server-side
                string? currentStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT OrderStatus FROM [Order] WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (currentStatus == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                if (!OrderStatuses.IsValidTransition(currentStatus, OrderStatuses.ReadyForPickup))
                {
                    await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                        OrderStatuses.ReadyForPickup, success: false, "Transition rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, currentStatus, OrderStatuses.ReadyForPickup);
                }

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

                // Audit trail
                await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                    OrderStatuses.ReadyForPickup, success: true, null);

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
                // Validate transition server-side
                string? currentStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT OrderStatus FROM [Order] WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (currentStatus == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                if (!OrderStatuses.IsValidTransition(currentStatus, OrderStatuses.PickedUp))
                {
                    await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                        OrderStatuses.PickedUp, success: false, "Transition rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, currentStatus, OrderStatuses.PickedUp);
                }

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

                // Audit trail
                await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                    OrderStatuses.PickedUp, success: true, null);

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

        public async Task ApprovePaymentAsync(int orderId)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                string? currentStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT OrderStatus FROM [Order] WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (currentStatus == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                if (!OrderStatuses.IsValidTransition(currentStatus, OrderStatuses.Processing))
                {
                    await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                        OrderStatuses.Processing, success: false, "Payment approval rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, currentStatus, OrderStatuses.Processing);
                }

                // Advance order to Processing
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = OrderStatuses.Processing }, tx);

                // Mark payment as Completed
                await conn.ExecuteAsync(
                    @"UPDATE Payment
                      SET PaymentStatus = 'Completed', PaymentDate = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                // Notify customer
                await QueueOrderNotificationAsync(conn, tx, orderId, OrderStatuses.Processing);

                await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                    OrderStatuses.Processing, success: true, "Payment proof approved by admin");

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task HoldPaymentAsync(int orderId)
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                string? currentStatus = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT OrderStatus FROM [Order] WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (currentStatus == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                if (!OrderStatuses.IsValidTransition(currentStatus, OrderStatuses.OnHold))
                {
                    await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                        OrderStatuses.OnHold, success: false, "Payment hold rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, currentStatus, OrderStatuses.OnHold);
                }

                // Move order to OnHold pending admin resolution
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = OrderStatuses.OnHold }, tx);

                // Mark payment as VerificationRejected (discrepancy flagged by admin)
                await conn.ExecuteAsync(
                    @"UPDATE Payment
                      SET PaymentStatus = 'VerificationRejected'
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                // Notify customer that the order is on hold due to a payment-proof discrepancy
                await QueueOrderNotificationAsync(conn, tx, orderId, "PaymentHeld");

                await LogStatusTransitionAsync(conn, tx, orderId, currentStatus,
                    OrderStatuses.OnHold, success: true, "Payment proof flagged; order placed on hold by admin");

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private sealed class StatusCountRow
        {
            public string OrderStatus { get; set; } = string.Empty;
            public int    Cnt         { get; set; }
        }

        private sealed class PaymentProofInfo
        {
            public string? PaymentProofUrl    { get; set; }
            public string? PaymentProofMethod { get; set; }
            public string? PaymentStatus      { get; set; }
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
                OrderStatuses.OutForDelivery => "TrackingUpdate",
                OrderStatuses.Delivered      => "DeliveryConfirmation",
                OrderStatuses.Cancelled      => "TrackingUpdate",
                "PaymentHeld"                => "PaymentHeld",
                _                            => "TrackingUpdate",
            };

            string subject = status == "PaymentHeld"
                ? $"Order {info.OrderNumber} — On Hold"
                : $"Order {info.OrderNumber} — {status}";
            string body = status switch
            {
                OrderStatuses.Processing     => $"Your payment for order {info.OrderNumber} has been verified and approved. Your order is now being processed.",
                OrderStatuses.ReadyForPickup => $"Your order {info.OrderNumber} is ready for pickup.",
                OrderStatuses.PickedUp       => $"Your order {info.OrderNumber} has been picked up.",
                OrderStatuses.OutForDelivery => $"Your order {info.OrderNumber} is out for delivery.",
                OrderStatuses.Delivered      => $"Your order {info.OrderNumber} has been delivered.",
                OrderStatuses.Cancelled      => $"Your order {info.OrderNumber} has been cancelled.",
                "PaymentHeld"                => $"We found a discrepancy with the payment proof for order {info.OrderNumber}. The order has been placed on hold — please contact us so we can resolve it.",
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

        /// <summary>
        /// Logs every status transition attempt (successful or rejected) to the
        /// OrderStatusAudit table for accountability and debugging.
        /// </summary>
        private static async Task LogStatusTransitionAsync(
            DbConnection conn, DbTransaction tx,
            int orderId, string fromStatus, string toStatus,
            bool success, string? reason)
        {
            try
            {
                await conn.ExecuteAsync(
                    @"IF OBJECT_ID('OrderStatusAudit', 'U') IS NOT NULL
                      INSERT INTO OrderStatusAudit
                          (OrderId, FromStatus, ToStatus, Success, Reason, CreatedAt)
                      VALUES
                          (@OrderId, @FromStatus, @ToStatus, @Success, @Reason, GETUTCDATE())",
                    new
                    {
                        OrderId    = orderId,
                        FromStatus = fromStatus,
                        ToStatus   = toStatus,
                        Success    = success,
                        Reason     = reason,
                    }, tx);
            }
            catch
            {
                // Audit logging is best-effort — never block the primary operation
                System.Diagnostics.Debug.WriteLine(
                    $"[Audit] Failed to log status transition: {orderId} {fromStatus} → {toStatus}");
            }
        }

        private sealed class OrderOwnerInfo
        {
            public int    UserId      { get; set; }
            public string Email       { get; set; } = string.Empty;
            public string OrderNumber { get; set; } = string.Empty;
        }
    }
}
