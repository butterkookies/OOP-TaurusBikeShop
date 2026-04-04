using System;
using System.Globalization;
using System.Windows.Data;

namespace AdminSystem_v2.Converters
{
    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d) return d.ToString("C", CultureInfo.CurrentCulture);
            if (value is double db) return db.ToString("C", CultureInfo.CurrentCulture);
            return value?.ToString() ?? string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (decimal.TryParse(value?.ToString(), NumberStyles.Currency, CultureInfo.CurrentCulture, out decimal result))
                return result;
            return 0m;
        }
    }
}
