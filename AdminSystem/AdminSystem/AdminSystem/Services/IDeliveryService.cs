using System.Collections.Generic;
using AdminSystem.Models;

namespace AdminSystem.Services
{
    public interface IDeliveryService
    {
        IEnumerable<Delivery> GetActiveDeliveries();
        Delivery GetDeliveryById(int deliveryId);
        void UpdateDeliveryStatus(int deliveryId, string newStatus);
        void AssignLalamoveBooking(int deliveryId, string bookingRef);
        void AssignLBCTracking(int deliveryId, string trackingNumber);
        void MarkDelivered(int deliveryId);
        void MarkFailed(int deliveryId);
    }
}
