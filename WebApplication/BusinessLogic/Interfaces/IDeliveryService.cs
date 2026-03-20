using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IDeliveryService
    {
        Task<Delivery?> GetByOrderIdAsync(int orderId);
        Task CreateDeliveryAsync(int orderId, string? lalamoveRef, string? driverName, string? driverPhone, DateTime? estimatedTime);
        Task UpdateStatusAsync(int deliveryId, string status);
    }
}
