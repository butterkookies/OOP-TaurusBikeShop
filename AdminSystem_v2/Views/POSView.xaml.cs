using System.Windows.Controls;
using System.Windows.Input;
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

        private void ClearProductSearch_Click(object sender, System.Windows.RoutedEventArgs e)
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
    }
}
