namespace AdminSystem.Models
{
    public class PickupOrder
    {
        public int    OrderId          { get; set; }
        public string PickupStatus     { get; set; }
        public System.DateTime? PickupExpiresAt { get; set; }
        public System.DateTime? PickedUpAt      { get; set; }
    }
}
