using System;
using System.Collections.Generic;
using AdminSystem.Helpers;
using AdminSystem.Models;
using Dapper;

namespace AdminSystem.Services
{
    public class DeliveryService : IDeliveryService
    {
        public IEnumerable<Delivery> GetActiveDeliveries()
        {
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                return conn.Query<Delivery>(
                    @"SELECT d.*, o.OrderNumber
                      FROM Delivery d
                      INNER JOIN [Order] o ON d.OrderId = o.OrderId
                      WHERE d.DeliveryStatus NOT IN (@S1, @S2)
                      ORDER BY d.CreatedAt DESC",
                    new { S1 = DeliveryStatuses.Delivered, S2 = DeliveryStatuses.Failed });
            }
        }

        public Delivery GetDeliveryById(int deliveryId)
        {
            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                Delivery d = conn.QueryFirstOrDefault<Delivery>(
                    "SELECT * FROM Delivery WHERE DeliveryId = @Id",
                    new { Id = deliveryId });
                if (d == null) return null;

                if (d.Courier == Couriers.Lalamove)
                    d.LalamoveDelivery = conn.QueryFirstOrDefault<LalamoveDelivery>(
                        "SELECT * FROM LalamoveDelivery WHERE DeliveryId = @Id",
                        new { Id = deliveryId });
                else if (d.Courier == Couriers.LBC)
                    d.LBCDelivery = conn.QueryFirstOrDefault<LBCDelivery>(
                        "SELECT * FROM LBCDelivery WHERE DeliveryId = @Id",
                        new { Id = deliveryId });
                return d;
            }
        }

        public void UpdateDeliveryStatus(int deliveryId, string newStatus)
        {
            switch (newStatus)
            {
                case DeliveryStatuses.Pending:
                case DeliveryStatuses.PickedUp:
                case DeliveryStatuses.InTransit:
                case DeliveryStatuses.Delivered:
                case DeliveryStatuses.Failed:
                    break;
                default:
                    throw new ArgumentException(
                        "Invalid delivery status: " + newStatus);
            }

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE Delivery
                      SET DeliveryStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE DeliveryId = @Id",
                    new { Status = newStatus, Id = deliveryId });
            }
        }

        public void AssignLalamoveBooking(int deliveryId, string bookingRef)
        {
            if (string.IsNullOrWhiteSpace(bookingRef))
                throw new ArgumentException("Booking reference is required.");

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE LalamoveDelivery
                      SET BookingRef = @Ref
                      WHERE DeliveryId = @Id",
                    new { Ref = bookingRef, Id = deliveryId });
                conn.Execute(
                    @"UPDATE Delivery
                      SET DeliveryStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE DeliveryId = @Id",
                    new { Status = DeliveryStatuses.PickedUp, Id = deliveryId });
            }
        }

        public void AssignLBCTracking(int deliveryId, string trackingNumber)
        {
            if (string.IsNullOrWhiteSpace(trackingNumber))
                throw new ArgumentException("Tracking number is required.");

            using (System.Data.SqlClient.SqlConnection conn =
                DatabaseHelper.GetConnection())
            {
                conn.Execute(
                    @"UPDATE LBCDelivery
                      SET TrackingNumber = @Tracking
                      WHERE DeliveryId = @Id",
                    new { Tracking = trackingNumber, Id = deliveryId });
                conn.Execute(
                    @"UPDATE Delivery
                      SET DeliveryStatus = @Status, UpdatedAt = GETUTCDATE()
                      WHERE DeliveryId = @Id",
                    new { Status = DeliveryStatuses.PickedUp, Id = deliveryId });
            }
        }

        public void MarkDelivered(int deliveryId)
            => UpdateDeliveryStatus(deliveryId, DeliveryStatuses.Delivered);

        public void MarkFailed(int deliveryId)
            => UpdateDeliveryStatus(deliveryId, DeliveryStatuses.Failed);
    }
}
