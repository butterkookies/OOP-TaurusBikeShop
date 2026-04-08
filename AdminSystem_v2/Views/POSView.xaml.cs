using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using AdminSystem_v2.Models;
using AdminSystem_v2.ViewModels;

namespace AdminSystem_v2.Views
{
    public partial class POSView : UserControl
    {
        public POSView() => InitializeComponent();

        private POSViewModel? VM => DataContext as POSViewModel;

        // Code-behind event handlers bridge mouse clicks to ViewModel commands
        // because WPF doesn't support MouseLeftButtonDown command bindings natively.

        private void ClearProductSearch_Click(object sender, RoutedEventArgs e)
        {
            if (VM != null) VM.ProductSearch = string.Empty;
        }

        private void CustomerDisplay_Click(object sender, MouseButtonEventArgs e)
        {
            VM?.ToggleCustomerDropdownCommand.Execute(null);
        }

        private void WalkIn_Click(object sender, MouseButtonEventArgs e)
        {
            VM?.SetWalkInCommand.Execute(null);
        }

        private void CustomerResult_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is POSCustomer customer)
                VM?.SelectCustomerCommand.Execute(customer);
        }

        // ── Voucher autocomplete ──────────────────────────────────────────────

        private void VoucherInput_GotFocus(object sender, RoutedEventArgs e)
        {
            VM?.LoadVoucherSuggestionsCommand.Execute(null);
        }

        private void VoucherInput_LostFocus(object sender, RoutedEventArgs e)
        {
            // Delay so that a MouseLeftButtonDown on a suggestion fires before the dropdown closes.
            Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() =>
            {
                if (VM != null) VM.IsVoucherDropdownOpen = false;
            }));
        }

        private void VoucherSuggestion_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border && border.Tag is POSVoucherSuggestion suggestion)
            {
                VM?.SelectVoucherCommand.Execute(suggestion);
                VoucherCodeBox.Focus();
            }
        }
    }
}
