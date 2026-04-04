namespace AdminSystem_v2.Models
{
    /// <summary>Maps the PickupOrder table — one row per store-pickup order.</summary>
    public class PickupOrder
    {
        public int       PickupOrderId     { get; set; }
        public int       OrderId           { get; set; }
        public DateTime? PickupReadyAt     { get; set; }
        public DateTime? PickupExpiresAt   { get; set; }
        public DateTime? PickupConfirmedAt { get; set; }
    }
}
