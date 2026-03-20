using Microsoft.EntityFrameworkCore;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.BusinessLogic.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly AppDbContext _context;

        public DeliveryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery?> GetByOrderIdAsync(int orderId)
        {
            return await _context.Deliveries
                .Include(d => d.Order)
                .FirstOrDefaultAsync(d => d.OrderId == orderId);
        }

        public async Task CreateDeliveryAsync(int orderId, string? lalamoveRef, string? driverName, string? driverPhone, DateTime? estimatedTime)
        {
            var delivery = new Delivery
            {
                OrderId               = orderId,
                LalamoveReference     = lalamoveRef,
                DriverName            = driverName,
                DriverPhone           = driverPhone,
                EstimatedDeliveryTime = estimatedTime,
                DeliveryStatus        = "Pending",
                CreatedAt             = DateTime.UtcNow
            };

            _context.Deliveries.Add(delivery);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int deliveryId, string status)
        {
            var delivery = await _context.Deliveries.FindAsync(deliveryId);
            if (delivery == null) return;

            delivery.DeliveryStatus = status;

            if (status == "Delivered")
            {
                delivery.ActualDeliveryTime = DateTime.UtcNow;

                var order = await _context.Orders.FindAsync(delivery.OrderId);
                if (order != null)
                {
                    order.OrderStatus = "Delivered";
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
