using System;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Data;
using AdminSystem.Helpers;

namespace AdminSystem.Converters
{
    [ValueConversion(typeof(string), typeof(SolidColorBrush))]
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value != null ? value.ToString() : string.Empty;
            switch (status)
            {
                case "Pending":
                case "PendingVerification":
                case "VerificationPending":
                case "InProgress":
                    return AppColors.StatusPending;
                case "Processing":
                case "Open":
                    return AppColors.StatusProcessing;
                case "Delivered":
                case "Completed":
                case "Resolved":
                    return AppColors.StatusSuccess;
                case "Cancelled":
                case "Failed":
                case "VerificationRejected":
                case "Closed":
                    return AppColors.StatusDanger;
                case "OnHold":
                case "AwaitingResponse":
                    return AppColors.StatusNeutral;
                case "Shipped":
                case "ReadyForPickup":
                    return AppColors.StatusTeal;
                default:
                    return AppColors.StatusNeutral;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
