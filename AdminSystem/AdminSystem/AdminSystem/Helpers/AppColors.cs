// AdminSystem/Helpers/AppColors.cs
using System.Windows.Media;

namespace AdminSystem.Helpers
{
    /// <summary>
    /// Centralized color palette for AdminSystem.
    /// All SolidColorBrush instances are frozen for thread-safety and reuse.
    /// </summary>
    public static class AppColors
    {
        // ── Brand / UI chrome ─────────────────────────────────────────────
        public static readonly SolidColorBrush Accent       = Freeze(0xCC, 0x00, 0x00); // Taurus red
        public static readonly SolidColorBrush Success      = Freeze(0x4C, 0xAF, 0x50); // material green
        public static readonly SolidColorBrush CardBg       = Freeze(0x24, 0x24, 0x24);
        public static readonly SolidColorBrush CardBorder   = Freeze(0x2E, 0x2E, 0x2E);
        public static readonly SolidColorBrush NavText      = Freeze(0xF0, 0xF0, 0xF0);
        public static readonly SolidColorBrush Muted        = Freeze(0x88, 0x88, 0x88);
        public static readonly SolidColorBrush PrintGray    = Freeze(107, 107, 107);

        // ── Login eye-icon toggle ─────────────────────────────────────────
        public static readonly SolidColorBrush EyeOn        = Freeze(0xCC, 0x00, 0x00);
        public static readonly SolidColorBrush EyeOff       = Freeze(0x60, 0x60, 0x60);

        // ── User detail badges ────────────────────────────────────────────
        public static readonly SolidColorBrush ActiveBg     = Freeze(0x1A, 0x3A, 0x1A);
        public static readonly SolidColorBrush InactiveBg   = Freeze(0x3A, 0x1A, 0x1A);
        public static readonly SolidColorBrush VerifiedBg   = Freeze(0x1A, 0x2A, 0x3A);
        public static readonly SolidColorBrush UnverifiedBg = Freeze(0x2E, 0x2E, 0x2E);
        public static readonly SolidColorBrush VerifiedText = Freeze(0x21, 0x96, 0xF3);

        // ── Status semantic (StatusToColorConverter, activity feed) ───────
        public static readonly SolidColorBrush StatusPending    = Freeze(217, 119,   6); // amber
        public static readonly SolidColorBrush StatusProcessing = Freeze( 37,  99, 235); // blue
        public static readonly SolidColorBrush StatusSuccess    = Freeze( 22, 163,  74); // green
        public static readonly SolidColorBrush StatusDanger     = Freeze(220,  38,  38); // red
        public static readonly SolidColorBrush StatusNeutral    = Freeze(156, 163, 175); // gray
        public static readonly SolidColorBrush StatusTeal       = Freeze( 13, 148, 136); // teal

        private static SolidColorBrush Freeze(byte r, byte g, byte b)
        {
            var brush = new SolidColorBrush(Color.FromRgb(r, g, b));
            brush.Freeze();
            return brush;
        }
    }
}
