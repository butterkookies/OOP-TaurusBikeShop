// AdminSystem/Models/Delivery.cs
using System;

namespace AdminSystem.Models
{
    public class Delivery
    {
        public int     DeliveryId     { get; set; }
        public int     OrderId        { get; set; }
        public string  Courier        { get; set; } = string.Empty;
        public string  DeliveryStatus { get; set; } = string.Empty;
        public bool    IsDelayed      { get; set; }
        public DateTime  CreatedAt   { get; set; }
        public DateTime? UpdatedAt   { get; set; }

        public LalamoveDelivery LalamoveDelivery { get; set; }
        public LBCDelivery      LBCDelivery      { get; set; }
    }

    public static class Couriers
    {
        public const string Lalamove = "Lalamove";
        public const string LBC      = "LBC";
    }

    public static class DeliveryStatuses
    {
        public const string Pending   = "Pending";
        public const string PickedUp  = "PickedUp";
        public const string InTransit = "InTransit";
        public const string Delivered = "Delivered";
        public const string Failed    = "Failed";
    }
}
