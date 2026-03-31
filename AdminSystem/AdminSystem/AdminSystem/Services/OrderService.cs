using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminSystem.Models;
using AdminSystem.Repositories;

namespace AdminSystem.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepo;

        public OrderService(OrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public IEnumerable<Order> GetAllOrders()
            => _orderRepo.GetAll();

        public IEnumerable<Order> GetOrdersByStatus(string status)
            => _orderRepo.GetByStatus(status);

        public IEnumerable<Order> GetActiveOrders()
            => _orderRepo.GetActiveOrders();

        public Task<List<Order>> GetActiveOrdersAsync()
            => Task.Run(() => _orderRepo.GetActiveOrders().ToList());

        public Order GetOrderById(int orderId)
            => _orderRepo.GetById(orderId);

        public void UpdateOrderStatus(int orderId, string newStatus)
        {
            if (string.IsNullOrWhiteSpace(newStatus))
                throw new ArgumentException("Status cannot be empty.");

            // Validate status is a known value
            switch (newStatus)
            {
                case OrderStatuses.Pending:
                case OrderStatuses.PendingVerification:
                case OrderStatuses.OnHold:
                case OrderStatuses.Processing:
                case OrderStatuses.ReadyForPickup:
                case OrderStatuses.PickedUp:
                case OrderStatuses.Shipped:
                case OrderStatuses.Delivered:
                case OrderStatuses.Cancelled:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid order status: " + newStatus);
            }

            _orderRepo.UpdateStatus(orderId, newStatus);
        }

        public void AssignDelivery(int orderId, string courier)
        {
            // Only Lalamove and LBC are valid — never any other courier
            if (courier != Couriers.Lalamove && courier != Couriers.LBC)
                throw new ArgumentException(
                    "Invalid courier. Only Lalamove and LBC are supported.");

            _orderRepo.UpdateStatus(orderId, OrderStatuses.Processing);
        }

        public void MarkReadyForPickup(int orderId)
            => _orderRepo.UpdateStatus(orderId, OrderStatuses.ReadyForPickup);

        public void CancelOrder(int orderId)
        {
            Order order = _orderRepo.GetById(orderId);
            if (order == null)
                throw new InvalidOperationException(
                    "Order #" + orderId + " not found.");

            // Cannot cancel already-delivered orders
            if (order.OrderStatus == OrderStatuses.Delivered)
                throw new InvalidOperationException(
                    "Cannot cancel a delivered order.");

            _orderRepo.UpdateStatus(orderId, OrderStatuses.Cancelled);
        }
    }
}
