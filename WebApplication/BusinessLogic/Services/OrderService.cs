// WebApplication/BusinessLogic/Services/OrderService.cs

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IOrderService"/> — order creation, history,
/// detail retrieval, and customer-initiated cancellation.
/// Flowchart: Part 3 — Cart &amp; Checkout, Part 4 — Order Tracking.
/// </summary>
public sealed class OrderService : IOrderService
{
    private readonly AppDbContext          _context;
    private readonly OrderRepository       _orderRepo;
    private readonly CartRepository        _cartRepo;
    private readonly IVoucherService       _voucherService;
    private readonly INotificationService  _notifications;
    private readonly ILogger<OrderService> _logger;

    private static readonly IReadOnlySet<string> CancellableStatuses =
        new HashSet<string>
        {
            OrderStatuses.Pending,
            OrderStatuses.PendingVerification
        };

    /// <inheritdoc/>
    public OrderService(
        AppDbContext          context,
        OrderRepository       orderRepo,
        CartRepository        cartRepo,
        IVoucherService       voucherService,
        INotificationService  notifications,
        ILogger<OrderService> logger)
    {
        _context        = context        ?? throw new ArgumentNullException(nameof(context));
        _orderRepo      = orderRepo      ?? throw new ArgumentNullException(nameof(orderRepo));
        _cartRepo       = cartRepo       ?? throw new ArgumentNullException(nameof(cartRepo));
        _voucherService = voucherService ?? throw new ArgumentNullException(nameof(voucherService));
        _notifications  = notifications  ?? throw new ArgumentNullException(nameof(notifications));
        _logger         = logger         ?? throw new ArgumentNullException(nameof(logger));
    }

    // =========================================================================
    // CreateOrderAsync
    // =========================================================================

    /// <inheritdoc/>
    public async Task<ServiceResult<int>> CreateOrderAsync(
        int userId,
        CheckoutViewModel vm,
        CancellationToken cancellationToken = default)
    {
        // Load the active cart with all items, products, and variants
        Cart? cart = await _cartRepo.GetActiveCartAsync(userId, null, cancellationToken);

        if (cart is null || !cart.Items.Any())
            return ServiceResult<int>.Fail("Your cart is empty.");

        // Pre-flight: validate stock for every item before opening the transaction
        foreach (CartItem item in cart.Items)
        {
            if (item.ProductVariantId is null) continue;

            ProductVariant? variant = await _context.ProductVariants
                .FirstOrDefaultAsync(v => v.ProductVariantId == item.ProductVariantId,
                    cancellationToken);

            if (variant is null || !variant.IsActive)
                return ServiceResult<int>.Fail(
                    $"'{item.Product?.Name ?? "A product"}' is no longer available.");

            if (variant.StockQuantity < item.Quantity)
                return ServiceResult<int>.Fail(
                    $"Only {variant.StockQuantity} unit(s) of " +
                    $"'{item.Product?.Name ?? "a product"}' remain in stock.");
        }

        // Load shipping address and create immutable snapshot
        Address? shippingAddress = null;
        int? snapshotAddressId = null;

        if (vm.DeliveryMethod != "Pickup")
        {
            if (!vm.SelectedAddressId.HasValue)
                return ServiceResult<int>.Fail("Please select a delivery address.");

            shippingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a =>
                    a.AddressId == vm.SelectedAddressId &&
                    a.UserId == userId &&
                    !a.IsSnapshot,
                    cancellationToken);

            if (shippingAddress is null)
                return ServiceResult<int>.Fail("Selected address not found.");
        }

        // Declared outside the strategy lambda so they are accessible for the
        // post-commit notification (step 11) and the return value.
        Order  order       = null!;
        string orderNumber = string.Empty;

        // SqlServerRetryingExecutionStrategy (EnableRetryOnFailure) forbids
        // SaveChangesAsync inside a manually-opened transaction unless every
        // SaveChanges call is wrapped in CreateExecutionStrategy().ExecuteAsync().
        IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
        try
        {
            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction tx =
                    await _context.Database.BeginTransactionAsync(cancellationToken);

                // ── 1. Create immutable address snapshot ──────────────────────
                if (shippingAddress != null)
                {
                    Address snapshot = new()
                    {
                        UserId     = userId,
                        Label      = shippingAddress.Label,
                        Street     = shippingAddress.Street,
                        City       = shippingAddress.City,
                        PostalCode = shippingAddress.PostalCode,
                        Province   = shippingAddress.Province,
                        Country    = shippingAddress.Country,
                        IsDefault  = false,
                        IsSnapshot = true,
                        CreatedAt  = DateTime.UtcNow
                    };
                    await _context.Addresses.AddAsync(snapshot, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                    snapshotAddressId = snapshot.AddressId;
                }

                // ── 2. Generate order number ───────────────────────────────────
                orderNumber = await GenerateOrderNumberAsync(cancellationToken);

                // ── 3. Calculate totals ────────────────────────────────────────
                decimal subTotal = cart.Items.Sum(i => i.PriceAtAdd * i.Quantity);
                decimal shippingFee = vm.DeliveryMethod switch
                {
                    "Lalamove" => CheckoutViewModel.LalamoveFee,
                    "LBC"      => CheckoutViewModel.LBCFee,
                    _          => CheckoutViewModel.PickupFee
                };

                // ── 4. Create Order row ────────────────────────────────────────
                order = new()
                {
                    UserId            = userId,
                    OrderNumber       = orderNumber,
                    OrderStatus       = OrderStatuses.Pending,
                    OrderDate         = DateTime.UtcNow,
                    ShippingAddressId = snapshotAddressId,
                    SubTotal          = subTotal,
                    ShippingFee       = shippingFee,
                    DiscountAmount    = vm.DiscountAmount,
                    IsWalkIn          = false,
                    CreatedAt         = DateTime.UtcNow,
                    // Schema v8.2 columns
                    FulfillmentType   = vm.DeliveryMethod == "Pickup"
                        ? FulfillmentTypes.Pickup
                        : FulfillmentTypes.Delivery,
                    CartId            = cart.CartId
                };
                await _context.Orders.AddAsync(order, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                // ── 5. Create OrderItems ──────────────────────────────────────
                foreach (CartItem cartItem in cart.Items)
                {
                    OrderItem orderItem = new()
                    {
                        OrderId          = order.OrderId,
                        ProductId        = cartItem.ProductId,
                        ProductVariantId = cartItem.ProductVariantId,
                        Quantity         = cartItem.Quantity,
                        UnitPrice        = cartItem.PriceAtAdd
                    };
                    await _context.OrderItems.AddAsync(orderItem, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);

                // ── 6. Lock inventory (InventoryLog Lock per variant) ─────────
                foreach (CartItem cartItem in cart.Items)
                {
                    if (cartItem.ProductVariantId is null) continue;

                    ProductVariant variant = (await _context.ProductVariants
                        .FirstAsync(v => v.ProductVariantId == cartItem.ProductVariantId,
                            cancellationToken))!;

                    variant.StockQuantity -= cartItem.Quantity;

                    InventoryLog lockLog = new()
                    {
                        ProductId        = cartItem.ProductId,
                        ProductVariantId = cartItem.ProductVariantId,
                        OrderId          = order.OrderId,
                        ChangeQuantity   = -cartItem.Quantity,
                        ChangeType       = InventoryChangeTypes.Lock,
                        ChangedByUserId  = userId,
                        Notes            = $"Stock locked for Order #{orderNumber}",
                        CreatedAt        = DateTime.UtcNow
                    };
                    await _context.InventoryLogs.AddAsync(lockLog, cancellationToken);
                }
                await _context.SaveChangesAsync(cancellationToken);

                // ── 7. Create PickupOrder if in-store pickup ───────────────────
                if (vm.DeliveryMethod == "Pickup")
                {
                    // PickupReadyAt and PickupExpiresAt are set by AdminSystem when the
                    // order is marked ReadyForPickup (PickupExpiresAt = PickupReadyAt + 3 days).
                    // Do NOT set PickupExpiresAt here — the expiry window must start when
                    // the customer is actually notified, not at order placement.
                    PickupOrder pickup = new()
                    {
                        OrderId = order.OrderId
                    };
                    await _context.PickupOrders.AddAsync(pickup, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                // ── 8. Redeem voucher if applied ───────────────────────────────
                if (!string.IsNullOrWhiteSpace(vm.VoucherCode) && vm.DiscountAmount > 0)
                {
                    await _voucherService.RedeemAsync(
                        vm.VoucherCode, userId, order.OrderId,
                        vm.DiscountAmount, cancellationToken);
                }

                // ── 9. Clear cart — delete all items and mark cart checked out ─
                _context.CartItems.RemoveRange(cart.Items);
                cart.IsCheckedOut  = true;
                cart.LastUpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);

                // ── 10. SystemLog ──────────────────────────────────────────────
                SystemLog sysLog = new()
                {
                    UserId    = userId,
                    EventType = SystemLogEvents.OrderStatusChange,
                    EventDescription =$"Order #{orderNumber} created with status Pending.",
                    CreatedAt = DateTime.UtcNow
                };
                await _context.SystemLogs.AddAsync(sysLog, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await tx.CommitAsync(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CreateOrderAsync failed for user {UserId}.", userId);

            // Log rollback to SystemLog (best-effort — outside failed transaction).
            // Clear tracked entities first so only the error log is saved,
            // not any half-built entities (e.g. snapshot Address) left in the
            // change tracker after the rolled-back transaction.
            try
            {
                _context.ChangeTracker.Clear();
                SystemLog errLog = new()
                {
                    UserId    = userId,
                    EventType = SystemLogEvents.BackgroundJobError,
                    EventDescription =$"Order creation rolled back: {ex.Message}",
                    CreatedAt = DateTime.UtcNow
                };
                await _context.SystemLogs.AddAsync(errLog, CancellationToken.None);
                await _context.SaveChangesAsync(CancellationToken.None);
            }
            catch { /* best-effort */ }

            return ServiceResult<int>.Fail("Unable to place order. Please try again.");
        }

        // ── 11. Queue notification (outside transaction — non-critical) ────
        // Runs after CommitAsync so a notification failure never rolls back
        // the committed order.
        try
        {
            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user?.Email != null)
            {
                await _notifications.QueueAsync(
                    channel:   NotifChannels.Email,
                    notifType: NotifTypes.OrderConfirmation,
                    recipient: user.Email,
                    subject:   $"Order Confirmed — {orderNumber}",
                    body:      $"Hi {user.FirstName}, your order {orderNumber} has been received and is being processed.",
                    userId:    userId,
                    orderId:   order.OrderId,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception notifEx)
        {
            _logger.LogWarning(notifEx,
                "Failed to queue OrderConfirmation notification for order {OrderId}. " +
                "Order was committed successfully — notification failure is non-critical.",
                order.OrderId);
        }

        return ServiceResult<int>.Ok(order.OrderId);
    }

    // =========================================================================
    // Query methods
    // =========================================================================

    /// <inheritdoc/>
    public async Task<OrderViewModel?> GetOrderConfirmationAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
        => await BuildOrderViewModelAsync(orderId, userId, cancellationToken);

    /// <inheritdoc/>
    public async Task<(IReadOnlyList<OrderViewModel> Orders, int TotalCount)> GetOrderHistoryAsync(
        int userId,
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyList<Order> orders =
            await _orderRepo.GetOrdersByUserAsync(userId, page, pageSize, cancellationToken);
        int total = await _orderRepo.GetOrderCountByUserAsync(userId, cancellationToken);

        IReadOnlyList<OrderViewModel> vms = orders
            .Select(MapToListViewModel)
            .ToList()
            .AsReadOnly();

        return (vms, total);
    }

    /// <inheritdoc/>
    public async Task<OrderViewModel?> GetOrderDetailAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
        => await BuildOrderViewModelAsync(orderId, userId, cancellationToken);

    // =========================================================================
    // CancelOrderAsync
    // =========================================================================

    /// <inheritdoc/>
    public async Task<ServiceResult> CancelOrderAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        Order? order = await _orderRepo.GetOrderWithDetailsAsync(orderId, cancellationToken);

        if (order is null || order.UserId != userId)
            return ServiceResult.Fail("Order not found.");

        if (!CancellableStatuses.Contains(order.OrderStatus))
            return ServiceResult.Fail(
                "This order can no longer be cancelled. " +
                "Please contact support if you need assistance.");

        IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
        try
        {
            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction tx =
                    await _context.Database.BeginTransactionAsync(cancellationToken);

                // Load order WITH change-tracking so the status update is persisted.
                // GetOrderWithDetailsAsync uses AsNoTracking — setting a property on
                // that entity would be silently discarded by SaveChangesAsync.
                Order trackedOrder = await _context.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken)
                    ?? throw new InvalidOperationException(
                        $"Order {orderId} not found for status update.");

                // Unlock inventory — InventoryLog Unlock entry per item
                foreach (OrderItem item in order.Items)
                {
                    if (item.ProductVariantId is null) continue;

                    ProductVariant? variant = await _context.ProductVariants
                        .FirstOrDefaultAsync(
                            v => v.ProductVariantId == item.ProductVariantId,
                            cancellationToken);

                    if (variant != null)
                        variant.StockQuantity += item.Quantity;

                    InventoryLog unlockLog = new()
                    {
                        ProductId        = item.ProductId,
                        ProductVariantId = item.ProductVariantId,
                        OrderId          = order.OrderId,
                        ChangeQuantity   = item.Quantity,
                        ChangeType       = InventoryChangeTypes.Unlock,
                        ChangedByUserId  = userId,
                        Notes            = $"Stock unlocked — Order #{order.OrderNumber} cancelled by customer.",
                        CreatedAt        = DateTime.UtcNow
                    };
                    await _context.InventoryLogs.AddAsync(unlockLog, cancellationToken);
                }

                trackedOrder.OrderStatus = OrderStatuses.Cancelled;
                trackedOrder.UpdatedAt   = DateTime.UtcNow;

                SystemLog sysLog = new()
                {
                    UserId           = userId,
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription = $"Order #{order.OrderNumber} cancelled by customer.",
                    CreatedAt        = DateTime.UtcNow
                };
                await _context.SystemLogs.AddAsync(sysLog, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);
            });

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "CancelOrderAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to cancel order. Please try again.");
        }
    }

    // =========================================================================
    // ConfirmDeliveryAsync
    // =========================================================================

    /// <inheritdoc/>
    public async Task<ServiceResult> ConfirmDeliveryAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken = default)
    {
        // Load full order (AsNoTracking) for guard checks and item iteration.
        Order? order = await _orderRepo.GetOrderWithDetailsAsync(orderId, cancellationToken);

        if (order is null || order.UserId != userId)
            return ServiceResult.Fail("Order not found.");

        if (order.IsWalkIn)
            return ServiceResult.Fail(
                "Walk-in POS orders are fulfilled immediately and cannot be confirmed here.");

        if (order.PickupOrder != null)
            return ServiceResult.Fail(
                "Store pickup orders are confirmed at the counter by staff.");

        if (order.OrderStatus == OrderStatuses.Delivered)
            return ServiceResult.Fail("This order has already been confirmed as delivered.");

        if (order.OrderStatus != OrderStatuses.OutForDelivery)
            return ServiceResult.Fail(
                "Only orders that are out for delivery can be confirmed. " +
                "Please contact support if you believe this is an error.");

        IExecutionStrategy strategy = _context.Database.CreateExecutionStrategy();
        try
        {
            await strategy.ExecuteAsync(async () =>
            {
                await using IDbContextTransaction tx =
                    await _context.Database.BeginTransactionAsync(cancellationToken);

                // Load order WITH change-tracking for the status update.
                Order trackedOrder = await _context.Orders
                    .FirstOrDefaultAsync(o => o.OrderId == orderId, cancellationToken)
                    ?? throw new InvalidOperationException(
                        $"Order {orderId} not found for status update.");

                trackedOrder.OrderStatus = OrderStatuses.Delivered;
                trackedOrder.UpdatedAt   = DateTime.UtcNow;

                // ── Inventory: Unlock + Sale pair per line item ────────────────
                // Lock was applied at order creation (StockQuantity was already
                // decremented then). This pair restores it in the ledger (Unlock)
                // and immediately re-consumes it as a confirmed sale (Sale).
                // Net StockQuantity change = 0. Audit trail: Lock → Unlock → Sale.
                foreach (OrderItem item in order.Items)
                {
                    if (item.ProductVariantId is null) continue;

                    ProductVariant? variant = await _context.ProductVariants
                        .FirstOrDefaultAsync(
                            v => v.ProductVariantId == item.ProductVariantId,
                            cancellationToken);

                    if (variant is null) continue;

                    // Unlock: reverse the reservation (+qty)
                    variant.StockQuantity += item.Quantity;
                    await _context.InventoryLogs.AddAsync(new InventoryLog
                    {
                        ProductId        = item.ProductId,
                        ProductVariantId = item.ProductVariantId,
                        OrderId          = orderId,
                        ChangeQuantity   = item.Quantity,
                        ChangeType       = InventoryChangeTypes.Unlock,
                        ChangedByUserId  = userId,
                        Notes            = $"Lock released — Order #{order.OrderNumber} confirmed delivered.",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);

                    // Sale: record actual consumption (-qty) — net change = 0
                    variant.StockQuantity -= item.Quantity;
                    await _context.InventoryLogs.AddAsync(new InventoryLog
                    {
                        ProductId        = item.ProductId,
                        ProductVariantId = item.ProductVariantId,
                        OrderId          = orderId,
                        ChangeQuantity   = -item.Quantity,
                        ChangeType       = InventoryChangeTypes.Sale,
                        ChangedByUserId  = userId,
                        Notes            = $"Sale finalised — Order #{order.OrderNumber} delivered to customer.",
                        CreatedAt        = DateTime.UtcNow
                    }, cancellationToken);
                }

                await _context.SystemLogs.AddAsync(new SystemLog
                {
                    UserId           = userId,
                    EventType        = SystemLogEvents.OrderStatusChange,
                    EventDescription =
                        $"Order #{order.OrderNumber} confirmed delivered by customer. " +
                        $"Status: {OrderStatuses.OutForDelivery} → {OrderStatuses.Delivered}.",
                    CreatedAt = DateTime.UtcNow
                }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                await tx.CommitAsync(cancellationToken);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ConfirmDeliveryAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to confirm delivery. Please try again.");
        }

        // ── Queue notification outside transaction (non-critical) ──────
        try
        {
            User? user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);

            if (user?.Email != null)
            {
                await _notifications.QueueAsync(
                    channel:   NotifChannels.Email,
                    notifType: NotifTypes.DeliveryConfirmation,
                    recipient: user.Email,
                    subject:   $"Delivery confirmed — Order {order.OrderNumber}",
                    body:
                        $"Hi {user.FirstName},\n\n" +
                        $"Thank you for confirming receipt of your order {order.OrderNumber}.\n\n" +
                        $"We hope you enjoy your purchase! If you have any concerns, please " +
                        $"contact our support team from your order detail page.\n\n" +
                        $"— Taurus Bike Shop",
                    userId:            userId,
                    orderId:           orderId,
                    cancellationToken: cancellationToken);
            }
        }
        catch (Exception notifEx)
        {
            _logger.LogWarning(notifEx,
                "Failed to queue DeliveryConfirmation notification for order {OrderId}. " +
                "Order was committed successfully — notification failure is non-critical.",
                orderId);
        }

        return ServiceResult.Ok();
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    private async Task<OrderViewModel?> BuildOrderViewModelAsync(
        int orderId,
        int userId,
        CancellationToken cancellationToken)
    {
        Order? order = await _orderRepo.GetOrderWithDetailsAsync(orderId, cancellationToken);

        if (order is null || order.UserId != userId)
            return null;

        return MapToDetailViewModel(order);
    }

    private static OrderViewModel MapToListViewModel(Order order) => new()
    {
        OrderId       = order.OrderId,
        OrderNumber   = order.OrderNumber,
        OrderStatus   = order.OrderStatus,
        OrderDate     = order.OrderDate,
        ItemCount     = order.Items.Count,
        DeliveryMethod= order.PickupOrder != null ? "Pickup" : "Delivery",
        SubTotal      = order.SubTotal,
        ShippingFee   = order.ShippingFee,
        DiscountAmount= order.DiscountAmount
    };

    private static OrderViewModel MapToDetailViewModel(Order order)
    {
        List<OrderItemViewModel> items = order.Items.Select(oi => new OrderItemViewModel
        {
            OrderItemId  = oi.OrderItemId,
            ProductId    = oi.ProductId,
            ProductName  = oi.Product?.Name ?? "Product",
            VariantName  = oi.Variant?.VariantName == "Default"
                ? null : oi.Variant?.VariantName,
            ImageUrl     = oi.Product?.Images
                ?.FirstOrDefault(i => i.IsPrimary)?.ImageUrl,
            Quantity     = oi.Quantity,
            UnitPrice    = oi.UnitPrice
        }).ToList();

        decimal subTotal = items.Sum(i => i.LineTotal);

        bool isPickup = order.PickupOrder != null;

        return new OrderViewModel
        {
            OrderId               = order.OrderId,
            OrderNumber           = order.OrderNumber,
            OrderStatus           = order.OrderStatus,
            OrderDate             = order.OrderDate,
            ItemCount             = items.Count,
            Items                 = items.AsReadOnly(),
            SubTotal              = subTotal,
            ShippingFee           = order.ShippingFee,
            DiscountAmount        = order.DiscountAmount,
            DeliveryMethod        = isPickup ? "Pickup" : "Delivery",
            ShippingAddress       = order.ShippingAddress,
            PickupOrder           = order.PickupOrder,
            Payments              = (order.Payments  ?? []).ToList().AsReadOnly(),
            Deliveries            = (order.Deliveries ?? []).ToList().AsReadOnly(),
            IsCancellable         = CancellableStatuses.Contains(order.OrderStatus),
            // Delivery confirmation is only available for courier-delivery orders
            // (not pickup, not walk-in) that are currently in OutForDelivery status.
            IsDeliveryConfirmable = !order.IsWalkIn
                                    && !isPickup
                                    && order.OrderStatus == OrderStatuses.OutForDelivery
        };
    }

    private async Task<string> GenerateOrderNumberAsync(CancellationToken ct)
    {
        string year   = DateTime.UtcNow.Year.ToString();
        int    count  = await _context.Orders.CountAsync(ct);
        return $"TBS-{year}-{(count + 1):D5}";
    }
}