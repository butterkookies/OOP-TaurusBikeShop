using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminSystem_v2.Converters
{
    /// <summary>
    /// Returns the inverse of a boolean value.
    /// Use for IsEnabled bindings where you need !SomeFlag.
    /// </summary>
    public class InverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : true;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => value is bool b ? !b : false;
    }
}
