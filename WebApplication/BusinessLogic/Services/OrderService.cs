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

        await using IDbContextTransaction tx =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
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
            string orderNumber = await GenerateOrderNumberAsync(cancellationToken);

            // ── 3. Calculate totals ────────────────────────────────────────
            decimal subTotal = cart.Items.Sum(i => i.PriceAtAdd * i.Quantity);
            decimal shippingFee = vm.DeliveryMethod switch
            {
                "Lalamove" => CheckoutViewModel.LalamoveFee,
                "LBC"      => CheckoutViewModel.LBCFee,
                _          => CheckoutViewModel.PickupFee
            };

            // ── 4. Create Order row ────────────────────────────────────────
            Order order = new()
            {
                UserId            = userId,
                OrderNumber       = orderNumber,
                OrderStatus       = OrderStatuses.Pending,
                OrderDate         = DateTime.UtcNow,
                ShippingAddressId = snapshotAddressId,
                ShippingFee       = shippingFee,
                DiscountAmount    = vm.DiscountAmount,
                IsWalkIn          = false,
                CreatedAt         = DateTime.UtcNow
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
                PickupOrder pickup = new()
                {
                    PickupOrderId   = order.OrderId,
                    OrderId         = order.OrderId,
                    PickupExpiresAt = DateTime.UtcNow.AddDays(3)
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

            // ── 9. Clear cart ──────────────────────────────────────────────
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

            // ── 11. Queue notification (outside transaction — non-critical) ─
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

            return ServiceResult<int>.Ok(order.OrderId);
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "CreateOrderAsync failed for user {UserId}.", userId);

            // Log rollback to SystemLog (best-effort — outside failed transaction)
            try
            {
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

        await using IDbContextTransaction tx =
            await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
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

            order.OrderStatus = OrderStatuses.Cancelled;

            SystemLog sysLog = new()
            {
                UserId    = userId,
                EventType = SystemLogEvents.OrderStatusChange,
                EventDescription =$"Order #{order.OrderNumber} cancelled by customer.",
                CreatedAt = DateTime.UtcNow
            };
            await _context.SystemLogs.AddAsync(sysLog, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);

            return ServiceResult.Ok();
        }
        catch (Exception ex)
        {
            await tx.RollbackAsync(cancellationToken);
            _logger.LogError(ex, "CancelOrderAsync failed for order {OrderId}.", orderId);
            return ServiceResult.Fail("Unable to cancel order. Please try again.");
        }
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

        return new OrderViewModel
        {
            OrderId         = order.OrderId,
            OrderNumber     = order.OrderNumber,
            OrderStatus     = order.OrderStatus,
            OrderDate       = order.OrderDate,
            ItemCount       = items.Count,
            Items           = items.AsReadOnly(),
            SubTotal        = subTotal,
            ShippingFee     = order.ShippingFee,
            DiscountAmount  = order.DiscountAmount,
            DeliveryMethod  = order.PickupOrder != null ? "Pickup" : "Delivery",
            ShippingAddress = order.ShippingAddress,
            PickupOrder     = order.PickupOrder,
            Payments        = order.Payments.ToList().AsReadOnly(),
            Deliveries      = order.Deliveries.ToList().AsReadOnly(),
            IsCancellable   = CancellableStatuses.Contains(order.OrderStatus)
        };
    }

    private async Task<string> GenerateOrderNumberAsync(CancellationToken ct)
    {
        string year   = DateTime.UtcNow.Year.ToString();
        int    count  = await _context.Orders.CountAsync(ct);
        return $"TBS-{year}-{(count + 1):D5}";
    }
}