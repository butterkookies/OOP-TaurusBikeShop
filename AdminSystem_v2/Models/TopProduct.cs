namespace AdminSystem_v2.Models
{
    /// <summary>Aggregated sales for one product over a date range.</summary>
    public class TopProduct
    {
        public string  ProductName { get; set; } = string.Empty;
        public int     UnitsSold   { get; set; }
        public decimal Revenue     { get; set; }
    }
}
