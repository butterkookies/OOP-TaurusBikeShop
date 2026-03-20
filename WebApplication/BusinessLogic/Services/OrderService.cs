using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository   _orderRepo;
        private readonly ICartRepository    _cartRepo;
        private readonly IProductRepository _productRepo;

        public OrderService(IOrderRepository orderRepo, ICartRepository cartRepo, IProductRepository productRepo)
        {
            _orderRepo   = orderRepo;
            _cartRepo    = cartRepo;
            _productRepo = productRepo;
        }

        public async Task<Order> PlaceOrderAsync(int userId, CheckoutViewModel checkout)
        {
            var cartItems = (await _cartRepo.GetCartByUserAsync(userId)).ToList();
            if (!cartItems.Any())
                throw new InvalidOperationException("Cart is empty.");

            decimal subtotal = cartItems.Sum(ci => ci.PriceAtAdd * ci.Quantity);
            decimal shipping = checkout.ShippingFee;
            decimal total    = subtotal + shipping;

            var order = new Order
            {
                UserId             = userId,
                OrderNumber        = GenerateOrderNumber(),
                OrderDate          = DateTime.UtcNow,
                OrderStatus        = "Pending",
                SubTotal           = subtotal,
                ShippingFee        = shipping,
                TotalAmount        = total,
                ShippingAddress    = checkout.Address,
                ShippingCity       = checkout.City,
                ShippingPostalCode = checkout.Zip,
                CreatedAt          = DateTime.UtcNow,
                OrderItems = cartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity  = ci.Quantity,
                    UnitPrice = ci.PriceAtAdd,
                    Subtotal  = ci.PriceAtAdd * ci.Quantity
                }).ToList()
            };

            await _orderRepo.AddAsync(order);
            await _orderRepo.SaveChangesAsync();

            // Record a pending payment
            order.Payments.Add(new Payment
            {
                OrderId       = order.OrderId,
                PaymentMethod = NormalizePaymentMethod(checkout.PaymentMethod),
                PaymentStatus = "Pending",
                Amount        = total,
                CreatedAt     = DateTime.UtcNow
            });
            _orderRepo.Update(order);
            await _orderRepo.SaveChangesAsync();

            await _cartRepo.ClearCartAsync(userId);
            return order;
        }

        public async Task<IEnumerable<OrderViewModel>> GetOrderHistoryAsync(int userId)
        {
            var orders = await _orderRepo.GetOrdersByUserAsync(userId);
            return orders.Select(o => new OrderViewModel
            {
                OrderId     = o.OrderId,
                OrderNumber = o.OrderNumber,
                OrderDate   = o.OrderDate,
                OrderStatus = o.OrderStatus,
                TotalAmount = o.TotalAmount
            });
        }

        public async Task<OrderViewModel?> GetOrderDetailsAsync(int orderId, int userId)
        {
            var order = await _orderRepo.GetOrderWithItemsAsync(orderId);
            if (order == null || order.UserId != userId) return null;

            return new OrderViewModel
            {
                OrderId     = order.OrderId,
                OrderNumber = order.OrderNumber,
                OrderDate   = order.OrderDate,
                OrderStatus = order.OrderStatus,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(oi => new OrderItemViewModel
                {
                    ProductName = oi.Product?.Name ?? "Unknown",
                    Quantity    = oi.Quantity,
                    UnitPrice   = oi.UnitPrice
                }).ToList()
            };
        }

        private static string GenerateOrderNumber()
            => $"TB{DateTime.UtcNow:yyyyMMddHHmm}{new Random().Next(1000, 9999)}";

        private static string NormalizePaymentMethod(string? method) => method?.ToLower() switch
        {
            "gcash"  => "GCash",
            "cod"    => "Cash",
            "credit" => "Card",
            _        => "Cash"
        };
    }
}
