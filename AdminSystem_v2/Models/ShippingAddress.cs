namespace AdminSystem_v2.Models
{
    /// <summary>Maps the Address table — shipping address snapshot linked to an order.</summary>
    public class ShippingAddress
    {
        public int     AddressId  { get; set; }
        public string  Label      { get; set; } = string.Empty;
        public string  Street     { get; set; } = string.Empty;
        public string  City       { get; set; } = string.Empty;
        public string? Province   { get; set; }
        public string  PostalCode { get; set; } = string.Empty;
        public string  Country    { get; set; } = "Philippines";

        /// <summary>Formatted single-line address for display.</summary>
        public string FullAddress
        {
            get
            {
                var parts = new List<string> { Street, City };
                if (!string.IsNullOrEmpty(Province)) parts.Add(Province);
                parts.Add(PostalCode);
                return string.Join(", ", parts);
            }
        }
    }
}
