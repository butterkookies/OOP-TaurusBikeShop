using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AdminSystem.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool b = value is bool bval && bval;
            bool invert = parameter?.ToString()?.ToLower() == "invert";
            return (invert ? !b : b) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is Visibility v && v == Visibility.Visible;
    }
}
