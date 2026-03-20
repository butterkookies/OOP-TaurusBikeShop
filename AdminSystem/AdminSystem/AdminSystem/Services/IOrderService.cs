using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        IEnumerable<Order> GetOrdersByStatus(string status);
        IEnumerable<Order> GetActiveOrders();
        Order GetOrderById(int orderId);
        void UpdateOrderStatus(int orderId, string newStatus);
        void AssignDelivery(int orderId, string courier);
        void MarkReadyForPickup(int orderId);
        void CancelOrder(int orderId);
    }
}
