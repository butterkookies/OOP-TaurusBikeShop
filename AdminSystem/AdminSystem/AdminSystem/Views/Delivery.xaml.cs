using System.Windows;
using System.Windows.Controls;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class DeliveryView : UserControl
    {
        private readonly DeliveryViewModel _vm;

        public DeliveryView(DeliveryViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            DgDeliveries.SelectionChanged += (s, e) =>
            {
                _vm.SelectedDelivery = DgDeliveries.SelectedItem as Delivery;
            };

            BtnRefreshDeliveries.Click += (s, e) => Refresh();

            BtnAssignTracking.Click += (s, e) =>
            {
                _vm.TrackingInput = TbTrackingInput.Text.Trim();
                _vm.AssignTrackingCommand.Execute(null);
                ShowFeedback();
                if (!_vm.HasError)
                {
                    TbTrackingInput.Text = string.Empty;
                    DgDeliveries.ItemsSource = _vm.Deliveries;
                }
            };

            BtnMarkDelivered.Click += (s, e) =>
            {
                _vm.MarkDeliveredCommand.Execute(null);
                ShowFeedback();
                if (!_vm.HasError)
                    DgDeliveries.ItemsSource = _vm.Deliveries;
            };

            BtnMarkFailed.Click += (s, e) =>
            {
                _vm.MarkFailedCommand.Execute(null);
                ShowFeedback();
                if (!_vm.HasError)
                    DgDeliveries.ItemsSource = _vm.Deliveries;
            };
        }

        // ── Refresh (called by MainWindow.Navigate) ───────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgDeliveries.ItemsSource   = _vm.Deliveries;
            TbDeliveryMsg.Visibility   = Visibility.Collapsed;
            TbDeliveryError.Visibility = Visibility.Collapsed;
        }

        // ── Show success / error feedback ─────────────────────────────────
        private void ShowFeedback()
        {
            if (_vm.HasError)
            {
                TbDeliveryError.Text       = _vm.ErrorMessage;
                TbDeliveryError.Visibility = Visibility.Visible;
                TbDeliveryMsg.Visibility   = Visibility.Collapsed;
            }
            else
            {
                TbDeliveryMsg.Text         = _vm.SuccessMessage;
                TbDeliveryMsg.Visibility   = Visibility.Visible;
                TbDeliveryError.Visibility = Visibility.Collapsed;
            }
        }
    }
}
