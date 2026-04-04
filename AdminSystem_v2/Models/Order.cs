namespace AdminSystem_v2.Models
{
    public class Order
    {
        // ── Order table columns ───────────────────────────────────────────────
        public int       OrderId              { get; set; }
        public int       UserId               { get; set; }
        public string    OrderNumber          { get; set; } = string.Empty;
        public DateTime  OrderDate            { get; set; }
        public string    OrderStatus          { get; set; } = string.Empty;
        public decimal   SubTotal             { get; set; }
        public decimal   DiscountAmount       { get; set; }
        public decimal   ShippingFee          { get; set; }
        public int?      ShippingAddressId    { get; set; }
        public string?   ContactPhone         { get; set; }
        public string?   DeliveryInstructions { get; set; }
        public bool      IsWalkIn             { get; set; }
        public DateTime  CreatedAt            { get; set; }
        public DateTime? UpdatedAt            { get; set; }

        // ── Joined from User ──────────────────────────────────────────────────
        public string CustomerName  { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;

        // ── "Pickup" or "Delivery" (derived via LEFT JOIN on PickupOrder) ─────
        public string DeliveryType { get; set; } = string.Empty;

        // ── Populated for detail view ─────────────────────────────────────────
        public List<OrderItem>  Items    { get; set; } = new();
        public DeliveryDetail?  Delivery { get; set; }
        public PickupOrder?     Pickup   { get; set; }

        // ── Computed ──────────────────────────────────────────────────────────
        public decimal GrandTotal => SubTotal - DiscountAmount + ShippingFee;
    }

    /// <summary>
    /// Centralises all valid OrderStatus string values to avoid magic strings.
    /// </summary>
    public static class OrderStatuses
    {
        public const string Pending        = "Pending";
        public const string Processing     = "Processing";
        public const string ReadyForPickup = "ReadyForPickup";
        public const string PickedUp       = "PickedUp";
        public const string Shipped        = "Shipped";
        public const string Delivered      = "Delivered";
        public const string Cancelled      = "Cancelled";
    }
}
