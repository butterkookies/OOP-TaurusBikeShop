namespace AdminSystem_v2.Models
{
    /// <summary>Maps vw_DeliveryDetail — Delivery base joined with courier-specific subtables.</summary>
    public class DeliveryDetail
    {
        public int       DeliveryId             { get; set; }
        public int       OrderId                { get; set; }
        public string    Courier                { get; set; } = string.Empty;
        public string    DeliveryStatus         { get; set; } = string.Empty;
        public bool      IsDelayed              { get; set; }
        public DateTime? DelayedUntil           { get; set; }
        public DateTime? EstimatedDeliveryTime  { get; set; }
        public DateTime? ActualDeliveryTime     { get; set; }
        public DateTime  CreatedAt              { get; set; }

        // Lalamove-specific (NULL for LBC orders)
        public string? LalamoveBookingRef { get; set; }
        public string? DriverName         { get; set; }
        public string? DriverPhone        { get; set; }

        // LBC-specific (NULL for Lalamove orders)
        public string? LbcTrackingNumber { get; set; }

        /// <summary>Human-readable tracking reference for the assigned courier.</summary>
        public string TrackingInfo => Courier switch
        {
            "Lalamove" => LalamoveBookingRef ?? "Pending booking",
            "LBC"      => LbcTrackingNumber  ?? "Pending tracking",
            _          => "—"
        };
    }
}
