namespace AdminSystem_v2.Models
{
    /// <summary>
    /// One bar in the Dashboard orders bar chart.
    /// </summary>
    public class OrderBarItem
    {
        /// <summary>Label shown below the bar (e.g. "Mon", "Apr 20").</summary>
        public string Label { get; set; } = string.Empty;

        /// <summary>Total number of orders for this period.</summary>
        public int Count { get; set; }

        /// <summary>
        /// Height ratio 0–1, relative to the max bar in the dataset.
        /// Computed by the ViewModel after all bars are collected.
        /// </summary>
        public double HeightRatio { get; set; }
    }
}
