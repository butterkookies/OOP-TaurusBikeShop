using System.Globalization;
using System.Windows.Data;

namespace AdminSystem_v2.Converters
{
    /// <summary>
    /// Converts a 0–1 ratio to a pixel height, scaled to a given maximum height.
    /// Pass the max height as the ConverterParameter (default: 140).
    /// </summary>
    public class RatioToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double ratio    = value is double d ? d : 0.0;
            double maxHeight = parameter is string s && double.TryParse(s, out double p) ? p : 140.0;

            // Ensure a minimum visible height of 4px so empty bars are still visible
            double height = Math.Max(4, ratio * maxHeight);
            return height;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
