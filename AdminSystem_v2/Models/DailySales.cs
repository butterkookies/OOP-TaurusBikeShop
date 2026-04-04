namespace AdminSystem_v2.Models
{
    /// <summary>Sales figures for a single calendar day.</summary>
    public class DailySales
    {
        public DateTime SaleDate   { get; set; }
        public int      OrderCount { get; set; }
        public decimal  Revenue    { get; set; }

        public string SaleDateDisplay => SaleDate.ToString("MMM d, yyyy");
    }
}
