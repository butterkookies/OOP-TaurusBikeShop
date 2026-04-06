using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminSystem_v2.Converters
{
    /// <summary>
    /// Returns <c>true</c> when both bound values are equal (string comparison).
    /// Used in DataTrigger.Binding via MultiBinding to work around the WPF restriction
    /// that DataTrigger.Value cannot itself be a Binding.
    /// </summary>
    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length != 2) return false;
            return Equals(values[0], values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
