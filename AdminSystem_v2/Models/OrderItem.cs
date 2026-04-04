namespace AdminSystem_v2.Models
{
    /// <summary>Maps vw_OrderItemDetail — one row per line item in an order.</summary>
    public class OrderItem
    {
        public int      OrderItemId      { get; set; }
        public int      OrderId          { get; set; }
        public int      ProductId        { get; set; }
        public int?     ProductVariantId { get; set; }
        public string   ProductName      { get; set; } = string.Empty;
        public string?  VariantName      { get; set; }
        public int      Quantity         { get; set; }
        public decimal  UnitPrice        { get; set; }
        public decimal  Subtotal         { get; set; }

        /// <summary>Display-friendly product + variant name.</summary>
        public string DisplayName => VariantName is { Length: > 0 }
            ? $"{ProductName} — {VariantName}"
            : ProductName;
    }
}
