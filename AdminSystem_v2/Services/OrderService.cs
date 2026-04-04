using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo) => _repo = repo;

        public Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null)
            => _repo.GetOrdersAsync(statusFilter);

        public Task<Order?> GetOrderDetailAsync(int orderId)
            => _repo.GetOrderDetailAsync(orderId);

        public Task UpdateOrderStatusAsync(int orderId, string status)
            => _repo.UpdateOrderStatusAsync(orderId, status);

        public Task MarkReadyForPickupAsync(int orderId)
            => _repo.MarkReadyForPickupAsync(orderId);

        public Task ConfirmPickupAsync(int orderId)
            => _repo.ConfirmPickupAsync(orderId);
    }
}
