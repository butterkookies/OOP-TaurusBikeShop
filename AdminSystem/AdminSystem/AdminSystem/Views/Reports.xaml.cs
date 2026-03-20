using System.Windows;
using System.Windows.Controls;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    /// <summary>
    /// Code-behind for the Reports UserControl.
    /// Delegates all data loading to ReportViewModel.
    /// </summary>
    public partial class ReportsView : UserControl
    {
        private readonly ReportViewModel _vm;

        public ReportsView(ReportViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            // Seed date pickers from VM defaults
            DpFrom.SelectedDate = _vm.DateFrom;
            DpTo.SelectedDate   = _vm.DateTo;
        }

        // ── Called by MainWindow.Navigate ─────────────────────────────────
        public void Refresh()
        {
            SyncDatePickersToVm();
            _vm.LoadCommand.Execute(null);
            BindGrids();
            UpdateStatCards();
            ShowError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── Refresh button ────────────────────────────────────────────────
        private void BtnRefreshReports_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Apply date filter ─────────────────────────────────────────────
        private void BtnApplyFilter_Click(object sender, RoutedEventArgs e)
        {
            if (!DpFrom.SelectedDate.HasValue || !DpTo.SelectedDate.HasValue)
            {
                ShowError("Please select both a From and To date.");
                return;
            }
            if (DpFrom.SelectedDate.Value > DpTo.SelectedDate.Value)
            {
                ShowError("From date must be before To date.");
                return;
            }

            SyncDatePickersToVm();
            _vm.LoadCommand.Execute(null);
            BindGrids();
            UpdateStatCards();
            ShowError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── Helpers ───────────────────────────────────────────────────────
        private void SyncDatePickersToVm()
        {
            if (DpFrom.SelectedDate.HasValue) _vm.DateFrom = DpFrom.SelectedDate.Value;
            if (DpTo.SelectedDate.HasValue)   _vm.DateTo   = DpTo.SelectedDate.Value;
        }

        private void BindGrids()
        {
            DgTopOrders.ItemsSource = _vm.TopOrders;
            DgLowStock.ItemsSource  = _vm.LowStock;
            DgMovements.ItemsSource = _vm.Movements;
        }

        private void UpdateStatCards()
        {
            TbTodaySales.Text     = _vm.TodaySalesDisplay;
            TbPeriodSales.Text    = _vm.PeriodSalesDisplay;
            TbDeliveredCount.Text = _vm.DeliveredCount.ToString();
            TbCancelledCount.Text = _vm.CancelledCount.ToString();
        }

        private void ShowError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                ReportsErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbReportsError.Text        = msg;
                ReportsErrorBar.Visibility = Visibility.Visible;
            }
        }
    }
}
