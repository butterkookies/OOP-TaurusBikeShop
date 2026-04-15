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

        // ── Actual payment method from Payment table (e.g. "GCash", "BankTransfer") ──
        public string? ActualPaymentMethod { get; set; }

        // ── Populated for detail view ─────────────────────────────────────────
        public List<OrderItem>  Items    { get; set; } = new();
        public DeliveryDetail?  Delivery { get; set; }
        public PickupOrder?     Pickup   { get; set; }

        // ── Populated by list query (subquery count) ─────────────────────────
        public int ItemCount { get; set; }

        // ── Computed ──────────────────────────────────────────────────────────
        public decimal GrandTotal => SubTotal - DiscountAmount + ShippingFee;

        /// <summary>
        /// Structured payment display: "Online &gt; GCash", "Walk-in &gt; Cash", etc.
        /// Falls back to channel-only label when actual method is unknown.
        /// </summary>
        public string PaymentMethodDisplay
        {
            get
            {
                string channel = IsWalkIn ? "Walk-in" : "Online";
                if (string.IsNullOrEmpty(ActualPaymentMethod))
                    return channel;

                string friendly = PaymentMethods.ToDisplayName(ActualPaymentMethod);
                return $"{channel} > {friendly}";
            }
        }

        /// <summary>Kept for backward compatibility — prefer PaymentMethodDisplay.</summary>
        public string PaymentMethod => IsWalkIn ? "Walk-in" : "Online";
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
        public const string OutForDelivery = "OutForDelivery";
        public const string Delivered      = "Delivered";
        public const string Cancelled      = "Cancelled";

        /// <summary>All statuses that represent a final, irreversible outcome.</summary>
        public static readonly IReadOnlySet<string> TerminalStatuses = new HashSet<string>
        {
            Cancelled, PickedUp, Delivered
        };

        /// <summary>
        /// Defines the allowed forward transitions for each status.
        /// Any transition not in this map is rejected.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, IReadOnlySet<string>> AllowedTransitions =
            new Dictionary<string, IReadOnlySet<string>>
            {
                [Pending]         = new HashSet<string> { Processing, ReadyForPickup, OutForDelivery, Cancelled },
                [Processing]      = new HashSet<string> { ReadyForPickup, OutForDelivery, Cancelled },
                [ReadyForPickup]  = new HashSet<string> { PickedUp },
                [OutForDelivery]  = new HashSet<string> { Delivered },
                // Terminal states — no further transitions allowed
                [PickedUp]       = new HashSet<string>(),
                [Delivered]      = new HashSet<string>(),
                [Cancelled]      = new HashSet<string>(),
            };

        /// <summary>
        /// Returns true when moving from <paramref name="currentStatus"/> to
        /// <paramref name="newStatus"/> is a valid forward transition.
        /// </summary>
        public static bool IsValidTransition(string currentStatus, string newStatus)
        {
            return AllowedTransitions.TryGetValue(currentStatus, out var allowed)
                && allowed.Contains(newStatus);
        }
    }

    /// <summary>
    /// Standardised order delivery types — avoids free-text inconsistencies.
    /// </summary>
    public static class OrderTypes
    {
        public const string Delivery = "Delivery";
        public const string Pickup   = "Pickup";
    }

    /// <summary>
    /// Standardised payment method constants and display helpers.
    /// Covers both POS and online payment channels.
    /// </summary>
    public static class PaymentMethods
    {
        // ── POS methods ──
        public const string Cash         = "Cash";
        public const string Card         = "Card";

        // ── Online / shared methods ──
        public const string GCash        = "GCash";
        public const string BankTransfer = "BankTransfer";

        /// <summary>Converts a stored payment method value to a human-readable label.</summary>
        public static string ToDisplayName(string method) => method switch
        {
            BankTransfer => "Bank Transfer",
            GCash        => "GCash",
            Cash         => "Cash",
            Card         => "Card",
            _            => method, // pass through unknown values gracefully
        };
    }
}
