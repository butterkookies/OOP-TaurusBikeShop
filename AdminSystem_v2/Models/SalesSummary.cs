namespace AdminSystem_v2.Models
{
    /// <summary>Aggregate sales figures for a date range.</summary>
    public class SalesSummary
    {
        public int     TotalOrders    { get; set; }
        public decimal TotalRevenue   { get; set; }   // SubTotal - Discount + Shipping
        public decimal AvgOrderValue  { get; set; }
        public decimal GrossRevenue   { get; set; }   // SubTotal only (before discount)
        public decimal TotalDiscounts { get; set; }
        public decimal TotalShipping  { get; set; }
    }
}
