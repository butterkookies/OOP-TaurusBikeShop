using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminSystem.Converters
{
    [ValueConversion(typeof(decimal), typeof(string))]
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d) return string.Format("\u20B1 {0:N2}", d);
            if (value is double dbl) return string.Format("\u20B1 {0:N2}", dbl);
            return "\u20B1 0.00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                s = s.Replace("\u20B1", "").Replace(",", "").Trim();
                decimal d;
                return decimal.TryParse(s, out d) ? d : 0m;
            }
            return 0m;
        }
    }
}
