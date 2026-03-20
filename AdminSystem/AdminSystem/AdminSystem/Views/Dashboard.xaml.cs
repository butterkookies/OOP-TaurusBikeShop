using System.Windows;
using System.Windows.Controls;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly DashboardViewModel _vm;

        public DashboardView(DashboardViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        public void Refresh()
        {
            _vm.LoadData();

            // Update stat TextBlocks (DataContext binding covers DgRecentOrders)
            TbActiveOrders.Text     = _vm.ActiveOrderCount.ToString();
            TbPendingPayments.Text  = _vm.PendingPaymentCount.ToString();
            TbLowStock.Text         = _vm.LowStockCount.ToString();
            TbTodaySales.Text       = _vm.TodaySalesDisplay;

            DgRecentOrders.ItemsSource = _vm.RecentOrders;

            if (_vm.HasError)
            {
                TbDashError.Text       = _vm.ErrorMessage;
                TbDashError.Visibility = Visibility.Visible;
            }
            else
            {
                TbDashError.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnDashRefresh_Click(object sender, RoutedEventArgs e)
            => Refresh();
    }
}
