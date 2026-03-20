// AdminSystem/Models/Order.cs
using System;
using System.Collections.Generic;

namespace AdminSystem.Models
{
    public class Order
    {
        public int     OrderId           { get; set; }
        public int     UserId            { get; set; }
        public int?    ShippingAddressId { get; set; }
        public string  OrderNumber       { get; set; } = string.Empty;
        public string  OrderStatus       { get; set; } = string.Empty;
        public string  DeliveryMethod    { get; set; }
        public string  VoucherCode       { get; set; }
        public decimal DiscountAmount    { get; set; }
        public decimal ShippingFee       { get; set; }
        public DateTime  OrderDate       { get; set; }
        public DateTime  CreatedAt       { get; set; }
        public DateTime? UpdatedAt       { get; set; }

        // Populated by join queries
        public string CustomerName  { get; set; }
        public string CustomerEmail { get; set; }
        public List<OrderItem>  Items    { get; set; } = new List<OrderItem>();
        public List<Payment>    Payments { get; set; } = new List<Payment>();
        public Delivery    Delivery { get; set; }
        public PickupOrder Pickup   { get; set; }
    }

    public static class OrderStatuses
    {
        public const string Pending             = "Pending";
        public const string PendingVerification = "PendingVerification";
        public const string OnHold              = "OnHold";
        public const string Processing          = "Processing";
        public const string ReadyForPickup      = "ReadyForPickup";
        public const string PickedUp            = "PickedUp";
        public const string Shipped             = "Shipped";
        public const string Delivered           = "Delivered";
        public const string Cancelled           = "Cancelled";
    }
}
