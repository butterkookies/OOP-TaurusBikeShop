using System.Windows;
using System.Windows.Controls;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class InventoryView : UserControl
    {
        private readonly InventoryViewModel _vm;

        public InventoryView(InventoryViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            BtnAdjust.Click += Adjust_Click;
        }

        // ── Refresh (called by MainWindow.Navigate) ───────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgInventoryLog.ItemsSource = _vm.Logs;
            DgLowStock.ItemsSource     = _vm.LowStock;
        }

        // ── Adjust stock ──────────────────────────────────────────────────
        private void Adjust_Click(object sender, RoutedEventArgs e)
        {
            TbAdjError.Visibility   = Visibility.Collapsed;
            TbAdjSuccess.Visibility = Visibility.Collapsed;

            int variantId;
            if (!int.TryParse(TbAdjVariantId.Text.Trim(), out variantId)
                || variantId <= 0)
            {
                TbAdjError.Text       = "Enter a valid Variant ID.";
                TbAdjError.Visibility = Visibility.Visible;
                return;
            }

            int qty;
            if (!int.TryParse(TbAdjQty.Text.Trim(), out qty) || qty == 0)
            {
                TbAdjError.Text       = "Enter a non-zero quantity.";
                TbAdjError.Visibility = Visibility.Visible;
                return;
            }

            if (!(CbAdjType.SelectedItem is ComboBoxItem selected))
            {
                TbAdjError.Text       = "Select a change type.";
                TbAdjError.Visibility = Visibility.Visible;
                return;
            }

            _vm.AdjustVariantId  = variantId;
            _vm.AdjustQuantity   = qty;
            _vm.AdjustChangeType = selected.Content?.ToString() ?? string.Empty;
            _vm.AdjustNotes      = TbAdjNotes.Text.Trim();

            _vm.AdjustCommand.Execute(null);

            if (_vm.HasError)
            {
                TbAdjError.Text       = _vm.ErrorMessage;
                TbAdjError.Visibility = Visibility.Visible;
            }
            else
            {
                TbAdjSuccess.Text       = _vm.SuccessMessage;
                TbAdjSuccess.Visibility = Visibility.Visible;

                // Clear form
                TbAdjVariantId.Text     = string.Empty;
                TbAdjQty.Text           = string.Empty;
                TbAdjNotes.Text         = string.Empty;
                CbAdjType.SelectedIndex = -1;
                // ItemsSource reassignment not needed — ObservableCollections
                // already bound; VM.AdjustStock calls Load() which updates in-place.
            }
        }
    }
}
