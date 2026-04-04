using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace AdminSystem_v2.Converters
{
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.ToString()?.ToLower() switch
            {
                // Product statuses
                "active"          => new SolidColorBrush(Color.FromRgb(34,  197, 94)),  // green
                "inactive"        => new SolidColorBrush(Color.FromRgb(156, 163, 175)), // gray

                // Order statuses
                "pending"         => new SolidColorBrush(Color.FromRgb(234, 179, 8)),   // amber
                "processing"      => new SolidColorBrush(Color.FromRgb(251, 146, 60)),  // orange
                "readyforpickup"  => new SolidColorBrush(Color.FromRgb(96,  165, 250)), // blue
                "pickedup"        => new SolidColorBrush(Color.FromRgb(34,  197, 94)),  // green
                "shipped"         => new SolidColorBrush(Color.FromRgb(167, 139, 250)), // purple
                "delivered"       => new SolidColorBrush(Color.FromRgb(34,  197, 94)),  // green
                "cancelled"       => new SolidColorBrush(Color.FromRgb(239, 68,  68)),  // red

                _                 => new SolidColorBrush(Color.FromRgb(156, 163, 175))  // gray
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
