using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null);
        Task<Order?> GetOrderDetailAsync(int orderId);
        Task UpdateOrderStatusAsync(int orderId, string status);
        Task MarkReadyForPickupAsync(int orderId);
        Task ConfirmPickupAsync(int orderId);
    }
}
