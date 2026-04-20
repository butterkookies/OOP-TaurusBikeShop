namespace AdminSystem_v2.Models
{
    /// <summary>Sales figures for a grouped period (day / week / month / year).</summary>
    public class DailySales
    {
        public DateTime SaleDate      { get; set; }
        public int      OrderCount    { get; set; }
        public decimal  Revenue       { get; set; }

        /// <summary>
        /// Format string applied by the ViewModel to produce a human-readable period label.
        /// Defaults to single-day format.
        /// </summary>
        public string DisplayFormat { get; set; } = "MMM d, yyyy";

        public string SaleDateDisplay => SaleDate.ToString(DisplayFormat);
    }
}
