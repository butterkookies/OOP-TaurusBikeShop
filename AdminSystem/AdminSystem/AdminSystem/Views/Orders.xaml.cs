using System.Windows;
using System.Windows.Controls;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class OrdersView : UserControl
    {
        private readonly OrderViewModel _vm;

        public OrdersView(OrderViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        public void Refresh()
        {
            _vm.LoadOrders();
            DgOrders.ItemsSource = _vm.Orders;
            ShowError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── Filter button clicks ──────────────────────────────────────────
        private void FilterBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            foreach (Button b in new[] {
                BtnFilterAll, BtnFilterPending, BtnFilterProcessing,
                BtnFilterDelivered, BtnFilterCancelled })
            {
                b.Background = AppColors.CardBorder;
                b.Foreground = AppColors.NavText;
            }

            btn.Background = AppColors.Accent;
            btn.Foreground = System.Windows.Media.Brushes.White;

            string filter = btn.Tag != null ? btn.Tag.ToString() : string.Empty;
            _vm.StatusFilter = filter;
            DgOrders.ItemsSource = _vm.Orders;
            ShowError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        private void BtnOrdersRefresh_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Selection changed ─────────────────────────────────────────────
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Order order = DgOrders.SelectedItem as Order;
            _vm.SelectedOrder = order;
            UpdateDetailPanel(order);
            HideFeedback();
        }

        // ── Status ComboBox ───────────────────────────────────────────────
        private void CbOrderStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem item = CbOrderStatus.SelectedItem as ComboBoxItem;
            _vm.PendingStatus = item?.Tag?.ToString() ?? string.Empty;
            BtnUpdateStatus.IsEnabled = !string.IsNullOrWhiteSpace(_vm.PendingStatus)
                                        && _vm.SelectedOrder != null;
        }

        // ── Approve ───────────────────────────────────────────────────────
        private void BtnApproveOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedOrder == null) return;
            _vm.ApproveOrderCommand.Execute(null);
            DgOrders.ItemsSource = _vm.Orders;
            if (_vm.HasError)
                ShowActionError(_vm.ErrorMessage);
            else
                ShowActionSuccess(_vm.SuccessMessage ?? "Order approved.");
        }

        // ── Cancel ────────────────────────────────────────────────────────
        private void BtnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedOrder == null) return;

            if (MessageBox.Show(
                    string.Format("Cancel order {0}?", _vm.SelectedOrder.OrderNumber),
                    "Confirm Cancellation",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning)
                != MessageBoxResult.Yes) return;

            _vm.CancelOrderCommand.Execute(null);
            DgOrders.ItemsSource = _vm.Orders;
            if (_vm.HasError)
                ShowActionError(_vm.ErrorMessage);
            else
            {
                ShowActionSuccess(_vm.SuccessMessage ?? "Order cancelled.");
                ClearDetail();
            }
        }

        // ── Update Status ─────────────────────────────────────────────────
        private void BtnUpdateStatus_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedOrder == null || string.IsNullOrWhiteSpace(_vm.PendingStatus))
                return;

            _vm.UpdateStatusCommand.Execute(null);
            DgOrders.ItemsSource = _vm.Orders;
            CbOrderStatus.SelectedIndex = -1;
            BtnUpdateStatus.IsEnabled = false;

            if (_vm.HasError)
                ShowActionError(_vm.ErrorMessage);
            else
                ShowActionSuccess(_vm.SuccessMessage ?? "Status updated.");
        }

        // ── Detail panel helpers ──────────────────────────────────────────
        private void UpdateDetailPanel(Order order)
        {
            if (order == null) { ClearDetail(); return; }

            TbNoSelection.Visibility    = Visibility.Collapsed;
            PanelOrderDetail.Visibility = Visibility.Visible;

            TbDetailOrderNum.Text = "Order " + order.OrderNumber;
            TbDetailCustomer.Text = order.CustomerName ?? "—";
            TbDetailStatus.Text   = order.OrderStatus;

            BtnApproveOrder.IsEnabled =
                order.OrderStatus != OrderStatuses.Processing &&
                order.OrderStatus != OrderStatuses.Delivered  &&
                order.OrderStatus != OrderStatuses.Cancelled;

            BtnCancelOrder.IsEnabled =
                order.OrderStatus != OrderStatuses.Delivered &&
                order.OrderStatus != OrderStatuses.Cancelled;

            CbOrderStatus.SelectedIndex = -1;
            BtnUpdateStatus.IsEnabled = false;
        }

        private void ClearDetail()
        {
            TbNoSelection.Visibility    = Visibility.Visible;
            PanelOrderDetail.Visibility = Visibility.Collapsed;
            CbOrderStatus.SelectedIndex = -1;
            BtnUpdateStatus.IsEnabled   = false;
        }

        // ── Error/success helpers ─────────────────────────────────────────
        private void ShowError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                ErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbOrderError.Text   = msg;
                ErrorBar.Visibility = Visibility.Visible;
            }
        }

        private void ShowActionSuccess(string msg)
        {
            TbOrderActionSuccess.Text       = msg;
            TbOrderActionSuccess.Visibility = Visibility.Visible;
            TbOrderActionError.Visibility   = Visibility.Collapsed;
        }

        private void ShowActionError(string msg)
        {
            TbOrderActionError.Text       = msg;
            TbOrderActionError.Visibility = Visibility.Visible;
            TbOrderActionSuccess.Visibility = Visibility.Collapsed;
        }

        private void HideFeedback()
        {
            TbOrderActionSuccess.Visibility = Visibility.Collapsed;
            TbOrderActionError.Visibility   = Visibility.Collapsed;
        }
    }
}
