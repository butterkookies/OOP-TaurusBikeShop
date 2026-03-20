using System.Windows;
using System.Windows.Controls;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    /// <summary>
    /// Code-behind for the Vouchers management UserControl.
    /// Delegates all data operations to VoucherViewModel.
    /// </summary>
    public partial class VouchersView : UserControl
    {
        private readonly VoucherViewModel _vm;
        private bool _isEditMode;

        public VouchersView(VoucherViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        // ── Called by MainWindow.Navigate ─────────────────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgVouchers.ItemsSource = _vm.Vouchers;
            HideFormFeedback();
            ShowListError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── New voucher button ────────────────────────────────────────────
        private void BtnNewVoucher_Click(object sender, RoutedEventArgs e)
        {
            _isEditMode = false;
            _vm.NewCommand.Execute(null);
            PopulateForm(_vm.EditingVoucher);
            TbPanelTitle.Text = "New Voucher";
            BtnToggleActive.Visibility = Visibility.Collapsed;
            HideFormFeedback();
        }

        // ── Refresh button ────────────────────────────────────────────────
        private void BtnRefreshVouchers_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Selection changed ─────────────────────────────────────────────
        private void DgVouchers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Voucher selected = DgVouchers.SelectedItem as Voucher;
            if (selected == null) return;

            _isEditMode = true;
            _vm.SelectedVoucher = selected;
            PopulateForm(_vm.EditingVoucher);
            TbPanelTitle.Text = "Edit Voucher";
            BtnToggleActive.Content =
                selected.IsActive ? "Deactivate Voucher" : "Activate Voucher";
            BtnToggleActive.Visibility = Visibility.Visible;
            HideFormFeedback();
        }

        // ── Save ──────────────────────────────────────────────────────────
        private void BtnSaveVoucher_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.EditingVoucher == null) _vm.NewCommand.Execute(null);
            ReadFormIntoEditing();

            _vm.SaveCommand.Execute(null);
            if (_vm.HasError)
                ShowFormError(_vm.ErrorMessage);
            else
            {
                ShowFormSuccess(_vm.SuccessMessage ?? "Saved.");
                DgVouchers.ItemsSource = _vm.Vouchers;
                if (!_isEditMode)
                {
                    ClearForm();
                    TbPanelTitle.Text          = "New Voucher";
                    BtnToggleActive.Visibility = Visibility.Collapsed;
                }
            }
        }

        // ── Toggle active ─────────────────────────────────────────────────
        private void BtnToggleActive_Click(object sender, RoutedEventArgs e)
        {
            _vm.ToggleCommand.Execute(null);
            if (_vm.HasError)
                ShowFormError(_vm.ErrorMessage);
            else
            {
                ShowFormSuccess(_vm.SuccessMessage ?? "Updated.");
                DgVouchers.ItemsSource = _vm.Vouchers;
                if (_vm.SelectedVoucher != null)
                    BtnToggleActive.Content =
                        _vm.SelectedVoucher.IsActive ? "Deactivate Voucher" : "Activate Voucher";
            }
        }

        // ── Form helpers ──────────────────────────────────────────────────

        private void PopulateForm(Voucher v)
        {
            if (v == null) { ClearForm(); return; }
            TbCode.Text          = v.Code ?? string.Empty;
            TbDiscountValue.Text = v.DiscountValue.ToString("N2");
            TbMinOrder.Text      = v.MinimumOrderAmount.HasValue
                ? v.MinimumOrderAmount.Value.ToString("N2")
                : string.Empty;
            TbMaxUsage.Text   = v.MaxUsageCount.HasValue
                ? v.MaxUsageCount.Value.ToString()
                : string.Empty;
            TbMaxPerUser.Text = v.MaxUsagePerUser.HasValue
                ? v.MaxUsagePerUser.Value.ToString()
                : string.Empty;
            DpExpiresAt.SelectedDate = v.ExpiresAt;
            SelectComboByTag(CbDiscountType, v.DiscountType ?? "Percentage");
        }

        private void ReadFormIntoEditing()
        {
            Voucher v = _vm.EditingVoucher;
            if (v == null) return;

            v.Code         = TbCode.Text.Trim().ToUpperInvariant();
            v.DiscountType = GetComboTag(CbDiscountType) ?? "Percentage";

            if (decimal.TryParse(TbDiscountValue.Text, out decimal disc))
                v.DiscountValue = disc;

            v.MinimumOrderAmount = decimal.TryParse(TbMinOrder.Text, out decimal minOrder)
                ? (decimal?)minOrder : null;
            v.MaxUsageCount = int.TryParse(TbMaxUsage.Text, out int maxUse)
                ? (int?)maxUse : null;
            v.MaxUsagePerUser = int.TryParse(TbMaxPerUser.Text, out int maxPer)
                ? (int?)maxPer : null;
            v.ExpiresAt = DpExpiresAt.SelectedDate;
            v.IsActive  = true;
        }

        private void ClearForm()
        {
            TbCode.Text          = string.Empty;
            TbDiscountValue.Text = string.Empty;
            TbMinOrder.Text      = string.Empty;
            TbMaxUsage.Text      = string.Empty;
            TbMaxPerUser.Text    = string.Empty;
            DpExpiresAt.SelectedDate = null;
            if (CbDiscountType.Items.Count > 0)
                CbDiscountType.SelectedIndex = 0;
        }

        private static void SelectComboByTag(ComboBox cb, string tag)
        {
            foreach (ComboBoxItem item in cb.Items)
                if (item.Tag?.ToString() == tag) { cb.SelectedItem = item; return; }
            if (cb.Items.Count > 0) cb.SelectedIndex = 0;
        }

        private static string GetComboTag(ComboBox cb)
        {
            ComboBoxItem selected = cb.SelectedItem as ComboBoxItem;
            return selected?.Tag?.ToString();
        }

        private void ShowListError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                VoucherErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbVoucherError.Text        = msg;
                VoucherErrorBar.Visibility = Visibility.Visible;
            }
        }

        private void ShowFormSuccess(string msg)
        {
            TbVoucherSuccess.Text         = msg;
            TbVoucherSuccess.Visibility   = Visibility.Visible;
            TbVoucherFormError.Visibility = Visibility.Collapsed;
        }

        private void ShowFormError(string msg)
        {
            TbVoucherFormError.Text       = msg;
            TbVoucherFormError.Visibility = Visibility.Visible;
            TbVoucherSuccess.Visibility   = Visibility.Collapsed;
        }

        private void HideFormFeedback()
        {
            TbVoucherSuccess.Visibility   = Visibility.Collapsed;
            TbVoucherFormError.Visibility = Visibility.Collapsed;
        }
    }
}
