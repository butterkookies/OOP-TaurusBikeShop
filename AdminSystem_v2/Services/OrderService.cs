using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repo;

        public OrderService(IOrderRepository repo) => _repo = repo;

        public Task<IEnumerable<Order>> GetOrdersAsync(string? statusFilter = null, string? typeFilter = null)
            => _repo.GetOrdersAsync(statusFilter, typeFilter);

        public Task<Order?> GetOrderDetailAsync(int orderId)
            => _repo.GetOrderDetailAsync(orderId);

        public Task UpdateOrderStatusAsync(int orderId, string status, string? expectedCurrentStatus = null)
            => _repo.UpdateOrderStatusAsync(orderId, status, expectedCurrentStatus);

        public Task MarkReadyForPickupAsync(int orderId, string? expectedCurrentStatus = null)
            => _repo.MarkReadyForPickupAsync(orderId, expectedCurrentStatus);

        public Task ConfirmPickupAsync(int orderId, string? expectedCurrentStatus = null)
            => _repo.ConfirmPickupAsync(orderId, expectedCurrentStatus);

        public Task<Dictionary<string, int>> GetStatusCountsAsync()
            => _repo.GetStatusCountsAsync();

        public Task ApprovePaymentAsync(int orderId, string? expectedCurrentStatus = null)
            => _repo.ApprovePaymentAsync(orderId, expectedCurrentStatus);

        public Task HoldPaymentAsync(int orderId, string? expectedCurrentStatus = null)
            => _repo.HoldPaymentAsync(orderId, expectedCurrentStatus);

        public Task<int> AutoCancelExpiredPendingOrdersAsync()
            => _repo.AutoCancelExpiredPendingOrdersAsync();

        public Task ShipOrderAsync(int orderId, string courier, string? expectedCurrentStatus = null)
            => _repo.ShipOrderAsync(orderId, courier, expectedCurrentStatus);
    }
}
