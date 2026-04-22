using System.Windows.Media;

namespace AdminSystem_v2.ViewModels
{
    /// <summary>
    /// Presentation-only model for a status count badge in the Orders badge bar.
    /// Holds a pre-built Brush so no converter is needed in XAML.
    /// </summary>
    public sealed class StatusBadge
    {
        /// <summary>The filter key passed to <c>SelectStatusCommand</c> (matches <see cref="Models.OrderStatuses"/> constants, or "All").</summary>
        public string Label        { get; init; } = string.Empty;

        /// <summary>Human-readable label shown inside the badge.</summary>
        public string DisplayLabel { get; init; } = string.Empty;

        /// <summary>Current order count for this status.</summary>
        public int Count           { get; set;  }

        /// <summary>Badge pill background brush.</summary>
        public Brush Background    { get; init; } = Brushes.Transparent;

        /// <summary>Badge text and border-highlight brush.</summary>
        public Brush Foreground    { get; init; } = Brushes.Gray;

        /// <summary>Semantic accent color (used for the dot and number badge).</summary>
        public Brush AccentColor   { get; init; } = Brushes.Gray;
    }
}
