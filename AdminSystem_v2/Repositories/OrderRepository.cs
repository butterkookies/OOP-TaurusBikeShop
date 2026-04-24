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
                o.SubTotal, o.DiscountAmount, o.ShippingFee, o.ShippingAddressId,
                COALESCE(o.ContactPhone, u.PhoneNumber) AS ContactPhone,
                o.DeliveryInstructions, o.IsWalkIn,
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
            // Statuses that imply a specific delivery type override any passed typeFilter
            if (statusFilter == OrderStatuses.ReadyForPickup || statusFilter == OrderStatuses.PickedUp)
                typeFilter = OrderTypes.Pickup;
            else if (statusFilter == OrderStatuses.OutForDelivery || statusFilter == OrderStatuses.Delivered)
                typeFilter = OrderTypes.Delivery;

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

            // 6 — shipping address (if delivery order with a linked address)
            if (order.ShippingAddressId.HasValue)
            {
                order.Address = await conn.QueryFirstOrDefaultAsync<ShippingAddress>(
                    @"SELECT AddressId, Label, Street, City, Province, PostalCode, Country
                      FROM Address
                      WHERE AddressId = @Id",
                    new { Id = order.ShippingAddressId.Value });
            }

            return order;
        }

        public async Task UpdateOrderStatusAsync(int orderId, string newStatus, string? expectedCurrentStatus = null)
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

                if (expectedCurrentStatus != null && currentStatus != expectedCurrentStatus)
                    throw new InvalidOperationException($"Order status was modified by another process. Please refresh the page. (Expected: {expectedCurrentStatus}, Actual: {currentStatus})");

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

        public async Task MarkReadyForPickupAsync(int orderId, string? expectedCurrentStatus = null)
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

                if (expectedCurrentStatus != null && currentStatus != expectedCurrentStatus)
                    throw new InvalidOperationException($"Order status was modified by another process. Please refresh the page. (Expected: {expectedCurrentStatus}, Actual: {currentStatus})");

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

        public async Task ConfirmPickupAsync(int orderId, string? expectedCurrentStatus = null)
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

                if (expectedCurrentStatus != null && currentStatus != expectedCurrentStatus)
                    throw new InvalidOperationException($"Order status was modified by another process. Please refresh the page. (Expected: {expectedCurrentStatus}, Actual: {currentStatus})");

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
                @"SELECT
                      o.OrderStatus,
                      COUNT(*) AS Cnt
                  FROM [Order] o
                  LEFT JOIN PickupOrder po ON o.OrderId = po.OrderId
                  WHERE o.IsWalkIn = 0
                    AND (
                        (o.OrderStatus IN ('ReadyForPickup', 'PickedUp') AND po.PickupOrderId IS NOT NULL)
                        OR (o.OrderStatus IN ('OutForDelivery', 'Delivered') AND po.PickupOrderId IS NULL)
                        OR (o.OrderStatus NOT IN ('ReadyForPickup', 'PickedUp', 'OutForDelivery', 'Delivered'))
                    )
                  GROUP BY o.OrderStatus";

            await using var conn = GetConnection();
            var rows = await conn.QueryAsync<StatusCountRow>(sql);
            return rows.ToDictionary(r => r.OrderStatus, r => r.Cnt);
        }

        public async Task ApprovePaymentAsync(int orderId, string? expectedCurrentStatus = null)
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

                if (expectedCurrentStatus != null && currentStatus != expectedCurrentStatus)
                    throw new InvalidOperationException($"Order status was modified by another process. Please refresh the page. (Expected: {expectedCurrentStatus}, Actual: {currentStatus})");

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

        public async Task HoldPaymentAsync(int orderId, string? expectedCurrentStatus = null)
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

                if (expectedCurrentStatus != null && currentStatus != expectedCurrentStatus)
                    throw new InvalidOperationException($"Order status was modified by another process. Please refresh the page. (Expected: {expectedCurrentStatus}, Actual: {currentStatus})");

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

            // Also insert InApp so the notification bell updates immediately
            await conn.ExecuteAsync(
                @"INSERT INTO Notification
                    (UserId, OrderId, Channel, NotifType, Recipient,
                     Subject, Body, Status, RetryCount, CreatedAt, IsRead)
                  VALUES
                    (@UserId, @OrderId, 'InApp', @NotifType, @Email,
                     @Subject, @Body, 'Sent', 0, GETUTCDATE(), 0)",
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

        // ── Auto-cancel expired pending orders ─────────────────────────────────

        /// <summary>
        /// Auto-cancels any Pending orders older than 24 hours, unlocks their
        /// inventory, and logs the transitions. Matches the WebApplication's
        /// PendingOrderMonitorJob behaviour so the AdminSystem can independently
        /// enforce the 24-hour policy even when the web app isn't running.
        /// </summary>
        public async Task<int> AutoCancelExpiredPendingOrdersAsync()
        {
            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // 1 — Find all pending orders older than 24 hours
                // NOTE: OrderDate is stored in LOCAL time (UTC+8) by the web app,
                // but SQL Server GETUTCDATE() returns actual UTC.  We must compare
                // against a local-time threshold to avoid an 8-hour mismatch that
                // would cause orders to appear 8h younger than they really are.
                var expiredOrders = (await conn.QueryAsync<ExpiredOrderRow>(
                    @"SELECT o.OrderId, o.OrderNumber, o.UserId, u.Email AS CustomerEmail
                      FROM [Order] o
                      INNER JOIN [User] u ON o.UserId = u.UserId
                      WHERE o.IsWalkIn = 0
                        AND o.OrderStatus = @Status
                        AND o.OrderDate < DATEADD(HOUR, -24, DATEADD(HOUR, 8, GETUTCDATE()))",
                    new { Status = OrderStatuses.Pending }, tx)).ToList();

                if (expiredOrders.Count == 0)
                {
                    await tx.CommitAsync();
                    return 0;
                }

                foreach (var order in expiredOrders)
                {
                    // 2 — Unlock inventory: restore StockQuantity for each order item
                    var items = await conn.QueryAsync<LockedItemRow>(
                        @"SELECT oi.OrderItemId, oi.ProductId, oi.ProductVariantId, oi.Quantity
                          FROM OrderItem oi
                          WHERE oi.OrderId = @OrderId AND oi.ProductVariantId IS NOT NULL",
                        new { order.OrderId }, tx);

                    foreach (var item in items)
                    {
                        // Restore stock
                        await conn.ExecuteAsync(
                            @"UPDATE ProductVariant
                              SET StockQuantity = StockQuantity + @Qty
                              WHERE ProductVariantId = @VariantId",
                            new { Qty = item.Quantity, VariantId = item.ProductVariantId }, tx);

                        // Write InventoryLog Unlock entry
                        await conn.ExecuteAsync(
                            @"INSERT INTO InventoryLog
                                (ProductId, ProductVariantId, OrderId, ChangeQuantity, ChangeType, Notes, CreatedAt)
                              VALUES
                                (@ProductId, @VariantId, @OrderId, @Qty, 'Unlock',
                                 @Notes, GETUTCDATE())",
                            new
                            {
                                item.ProductId,
                                VariantId = item.ProductVariantId,
                                order.OrderId,
                                Qty = item.Quantity,
                                Notes = $"Stock unlocked — Order #{order.OrderNumber} auto-cancelled (24h pending timeout)."
                            }, tx);
                    }

                    // 3 — Set order status to Cancelled
                    await conn.ExecuteAsync(
                        @"UPDATE [Order]
                          SET OrderStatus = @NewStatus, UpdatedAt = GETUTCDATE()
                          WHERE OrderId = @OrderId",
                        new { order.OrderId, NewStatus = OrderStatuses.Cancelled }, tx);

                    // 4 — Queue notification for the customer
                    await conn.ExecuteAsync(
                        @"INSERT INTO Notification
                            (UserId, OrderId, Channel, NotifType, Recipient,
                             Subject, Body, Status, RetryCount, CreatedAt, IsRead)
                          VALUES
                            (@UserId, @OrderId, 'Email', 'OrderAutoCancelled', @Email,
                             @Subject, @Body, 'Pending', 0, GETUTCDATE(), 0)",
                        new
                        {
                            order.UserId,
                            order.OrderId,
                            Email   = order.CustomerEmail,
                            Subject = $"Order {order.OrderNumber} cancelled — payment not received",
                            Body    = $"Your order {order.OrderNumber} has been cancelled because payment " +
                                      $"was not submitted within 24 hours of placing it. If you still want " +
                                      $"the items, please place a new order."
                        }, tx);

                    // 5 — Audit log
                    await LogStatusTransitionAsync(conn, tx, order.OrderId,
                        OrderStatuses.Pending, OrderStatuses.Cancelled,
                        success: true, "Auto-cancelled: 24h pending timeout (AdminSystem)");
                }

                // 6 — SystemLog entry
                await conn.ExecuteAsync(
                    @"INSERT INTO SystemLog (EventType, EventDescription, CreatedAt)
                      VALUES ('OrderStatusChange', @Desc, GETUTCDATE())",
                    new { Desc = $"AdminSystem auto-cancelled {expiredOrders.Count} expired pending order(s)." }, tx);

                await tx.CommitAsync();
                return expiredOrders.Count;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        private sealed class ExpiredOrderRow
        {
            public int    OrderId       { get; set; }
            public string OrderNumber   { get; set; } = string.Empty;
            public int    UserId        { get; set; }
            public string CustomerEmail { get; set; } = string.Empty;
        }

        private sealed class LockedItemRow
        {
            public int  OrderItemId      { get; set; }
            public int  ProductId        { get; set; }
            public int  ProductVariantId { get; set; }
            public int  Quantity         { get; set; }
        }

        // ── Ship order via courier ─────────────────────────────────────────────

        /// <summary>
        /// Ships a Processing delivery order via the specified courier.
        /// <para>
        /// Atomically:
        /// <list type="number">
        ///   <item>Validates the Processing → OutForDelivery transition.</item>
        ///   <item>Generates a simulated courier booking reference / tracking number.</item>
        ///   <item>Inserts a <c>Delivery</c> row and the appropriate courier sub-row.</item>
        ///   <item>Advances the order status to <c>OutForDelivery</c>.</item>
        ///   <item>Queues both an Email and an InApp <c>TrackingUpdate</c> notification
        ///         containing the tracking link and the order's shipping fee.</item>
        ///   <item>Writes an audit log entry.</item>
        /// </list>
        /// </para>
        /// </summary>
        public async Task ShipOrderAsync(int orderId, string courier, string? expectedCurrentStatus = null)
        {
            if (courier != "Lalamove" && courier != "LBC")
                throw new ArgumentException($"Unsupported courier '{courier}'. Must be 'Lalamove' or 'LBC'.", nameof(courier));

            await using var conn = GetConnection();
            await using var tx   = await conn.BeginTransactionAsync();
            try
            {
                // 1 — Read current status & shipping fee
                var orderRow = await conn.QueryFirstOrDefaultAsync<ShipOrderRow>(
                    @"SELECT o.OrderStatus, o.ShippingFee, o.OrderNumber,
                             o.UserId,      u.Email        AS CustomerEmail
                      FROM [Order] o
                      INNER JOIN [User] u ON o.UserId = u.UserId
                      WHERE o.OrderId = @OrderId",
                    new { OrderId = orderId }, tx);

                if (orderRow == null)
                    throw new InvalidOperationException($"Order {orderId} not found.");

                if (expectedCurrentStatus != null && orderRow.OrderStatus != expectedCurrentStatus)
                    throw new InvalidOperationException(
                        $"Order status was modified by another process. Please refresh. " +
                        $"(Expected: {expectedCurrentStatus}, Actual: {orderRow.OrderStatus})");

                if (!OrderStatuses.IsValidTransition(orderRow.OrderStatus, OrderStatuses.OutForDelivery))
                {
                    await LogStatusTransitionAsync(conn, tx, orderId,
                        orderRow.OrderStatus, OrderStatuses.OutForDelivery,
                        success: false, "ShipOrderAsync: transition rejected by validation");
                    throw new InvalidStatusTransitionException(orderId, orderRow.OrderStatus, OrderStatuses.OutForDelivery);
                }

                // 2 — Generate simulated tracking data
                var (subject, body) = GenerateTrackingNotification(
                    courier, orderRow.OrderNumber, orderRow.ShippingFee,
                    out string bookingRef, out string? driverName, out string? driverPhone, out string trackingNumber);

                // 3 — Insert Delivery base row
                // OUTPUT without INTO is blocked when the table has triggers.
                // Routing through a table variable satisfies SQL Server's restriction.
                int deliveryId = await conn.QuerySingleAsync<int>(
                    @"DECLARE @t TABLE (DeliveryId INT);
                      INSERT INTO Delivery
                          (OrderId, Courier, DeliveryStatus, IsDelayed, CreatedAt)
                      OUTPUT INSERTED.DeliveryId INTO @t
                      VALUES (@OrderId, @Courier, 'Pending', 0, GETUTCDATE());
                      SELECT DeliveryId FROM @t;",
                    new { OrderId = orderId, Courier = courier }, tx);

                // 4 — Insert courier sub-row
                if (courier == "Lalamove")
                {
                    await conn.ExecuteAsync(
                        @"INSERT INTO LalamoveDelivery (DeliveryId, BookingRef, DriverName, DriverPhone)
                          VALUES (@DeliveryId, @BookingRef, @DriverName, @DriverPhone)",
                        new { DeliveryId = deliveryId, BookingRef = bookingRef,
                              DriverName = driverName, DriverPhone = driverPhone }, tx);
                }
                else // LBC
                {
                    await conn.ExecuteAsync(
                        @"INSERT INTO LBCDelivery (DeliveryId, TrackingNumber)
                          VALUES (@DeliveryId, @TrackingNumber)",
                        new { DeliveryId = deliveryId, TrackingNumber = trackingNumber }, tx);
                }

                // 5 — Advance order status
                await conn.ExecuteAsync(
                    @"UPDATE [Order]
                      SET OrderStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE OrderId = @OrderId",
                    new { OrderId = orderId, Status = OrderStatuses.OutForDelivery }, tx);

                // 6 — Queue Email notification
                await conn.ExecuteAsync(
                    @"INSERT INTO Notification
                        (UserId, OrderId, Channel, NotifType, Recipient,
                         Subject, Body, Status, RetryCount, CreatedAt, IsRead)
                      VALUES
                        (@UserId, @OrderId, 'Email', 'TrackingUpdate', @Email,
                         @Subject, @Body, 'Pending', 0, GETUTCDATE(), 0)",
                    new
                    {
                        orderRow.UserId,
                        OrderId  = orderId,
                        Email    = orderRow.CustomerEmail,
                        Subject  = subject,
                        Body     = body,
                    }, tx);

                // 7 — Queue InApp notification (instantly visible in notification bell)
                await conn.ExecuteAsync(
                    @"INSERT INTO Notification
                        (UserId, OrderId, Channel, NotifType, Recipient,
                         Subject, Body, Status, RetryCount, CreatedAt, IsRead)
                      VALUES
                        (@UserId, @OrderId, 'InApp', 'TrackingUpdate', @Email,
                         @Subject, @Body, 'Sent', 0, GETUTCDATE(), 0)",
                    new
                    {
                        orderRow.UserId,
                        OrderId  = orderId,
                        Email    = orderRow.CustomerEmail,
                        Subject  = subject,
                        Body     = body,
                    }, tx);

                // 8 — Audit trail
                await LogStatusTransitionAsync(conn, tx, orderId,
                    orderRow.OrderStatus, OrderStatuses.OutForDelivery,
                    success: true, $"Shipped via {courier} — ref: {(courier == "Lalamove" ? bookingRef : trackingNumber)}");

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Generates a realistic simulated tracking reference and returns
        /// the notification subject and body for the given courier.
        /// </summary>
        private static (string Subject, string Body) GenerateTrackingNotification(
            string   courier,
            string   orderNumber,
            decimal  shippingFee,
            out string bookingRef,
            out string? driverName,
            out string? driverPhone,
            out string  trackingNumber)
        {
            var rng = new Random();

            // Simulated Filipino driver name pool
            string[] firstNames = { "Juan", "Maria", "Jose", "Ana", "Carlo", "Liza", "Mark", "Rosa", "Rico", "Jenny" };
            string[] lastNames  = { "Santos", "Reyes", "Cruz", "Bautista", "Garcia", "Ramos", "Flores", "Torres", "Navarro", "Dela Cruz" };

            // Common Philippine mobile prefixes (Globe/Smart)
            string[] prefixes = { "0917", "0918", "0919", "0927", "0928", "0939", "0947", "0956", "0966", "0977" };

            bookingRef    = string.Empty;
            driverName    = null;
            driverPhone   = null;
            trackingNumber = string.Empty;

            if (courier == "Lalamove")
            {
                // Format: LM-YYMM-XXXXXX  (e.g. LM-2604-384912)
                string yymm = DateTime.Now.ToString("yyMM");
                bookingRef  = $"LM-{yymm}-{rng.Next(100000, 999999)}";

                driverName  = $"{firstNames[rng.Next(firstNames.Length)]} {lastNames[rng.Next(lastNames.Length)]}";
                driverPhone = $"{prefixes[rng.Next(prefixes.Length)]}{rng.Next(1000000, 9999999)}";

                string subject = $"Your order {orderNumber} has been shipped via Lalamove!";
                string body    =
                    $"Your order {orderNumber} is now out for delivery via Lalamove.\n\n" +
                    $"📦 Courier:       Lalamove\n" +
                    $"🚚 Driver:        {driverName}  |  {driverPhone}\n" +
                    $"🔖 Booking Ref:   {bookingRef}\n" +
                    $"🔗 Track Order:   https://www.lalamove.com/track/{bookingRef}\n" +
                    $"💰 Shipping Fee:  ₱{shippingFee:N2}\n\n" +
                    $"Estimated delivery: within today. You may call your driver directly if needed.";

                return (subject, body);
            }
            else // LBC
            {
                // Format: 1Z + 9 alphanumeric chars  (realistic LBC waybill style)
                const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ0123456789";
                var waybill = new System.Text.StringBuilder("1Z");
                for (int i = 0; i < 9; i++)
                    waybill.Append(chars[rng.Next(chars.Length)]);
                trackingNumber = waybill.ToString();

                string subject = $"Your order {orderNumber} has been shipped via LBC!";
                string body    =
                    $"Your order {orderNumber} is now out for delivery via LBC Express.\n\n" +
                    $"📦 Courier:          LBC Express\n" +
                    $"🔖 Tracking No.:     {trackingNumber}\n" +
                    $"🔗 Track Order:      https://www.lbcexpress.com/track/?tracking_no={trackingNumber}\n" +
                    $"💰 Shipping Fee:     ₱{shippingFee:N2}\n\n" +
                    $"Estimated delivery: 2–5 business days. Use the link above to track your parcel anytime.";

                return (subject, body);
            }
        }

        private sealed class ShipOrderRow
        {
            public string  OrderStatus    { get; set; } = string.Empty;
            public decimal ShippingFee    { get; set; }
            public string  OrderNumber    { get; set; } = string.Empty;
            public int     UserId         { get; set; }
            public string  CustomerEmail  { get; set; } = string.Empty;
        }
    }
}
