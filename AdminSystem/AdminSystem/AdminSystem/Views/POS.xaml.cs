using System.Windows;
using System.Windows.Controls;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class POSView : UserControl
    {
        private readonly POSViewModel _vm;

        public POSView(POSViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;

            DgCart.ItemsSource = _vm.CartItems;
            _vm.CartItems.CollectionChanged += (s, e) => UpdateTotalsDisplay();
        }

        public void Refresh()
        {
            _vm.LoadProducts();
            RebuildProductTiles();
            UpdateTotalsDisplay();
            ShowError(null);
        }

        // ── Product tiles (dynamic WrapPanel) ────────────────────────────
        private void RebuildProductTiles()
        {
            ProductTilesPanel.Children.Clear();
            foreach (Product p in _vm.Products)
            {
                Border tile = BuildProductTile(p);
                ProductTilesPanel.Children.Add(tile);
            }
        }

        private Border BuildProductTile(Product product)
        {
            string priceText = string.Format("\u20B1 {0:N2}", product.Price);

            // Product name TextBlock
            TextBlock tbName = new TextBlock
            {
                Text             = product.Name,
                Foreground       = System.Windows.Media.Brushes.White,
                FontSize         = 12,
                FontFamily       = new System.Windows.Media.FontFamily("Segoe UI"),
                FontWeight       = FontWeights.SemiBold,
                TextWrapping     = TextWrapping.Wrap,
                MaxWidth         = 100,
                Margin           = new Thickness(0, 0, 0, 4)
            };

            // Category
            TextBlock tbCat = new TextBlock
            {
                Text       = product.CategoryName ?? string.Empty,
                Foreground = AppColors.Muted,
                FontSize   = 10,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin     = new Thickness(0, 0, 0, 6)
            };

            // Price
            TextBlock tbPrice = new TextBlock
            {
                Text       = priceText,
                Foreground = AppColors.Success,
                FontSize   = 13,
                FontWeight = FontWeights.Bold,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin     = new Thickness(0, 0, 0, 8)
            };

            // Add button
            Button addBtn = new Button
            {
                Content         = "+ Add",
                Background      = AppColors.Accent,
                Foreground      = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0),
                Padding         = new Thickness(8, 4, 8, 4),
                FontSize        = 11,
                FontFamily      = new System.Windows.Media.FontFamily("Segoe UI"),
                Cursor          = System.Windows.Input.Cursors.Hand,
                Tag             = product
            };
            addBtn.Click += AddTileBtn_Click;

            StackPanel sp = new StackPanel();
            sp.Children.Add(tbName);
            sp.Children.Add(tbCat);
            sp.Children.Add(tbPrice);
            sp.Children.Add(addBtn);

            Border tile = new Border
            {
                Width           = 130,
                Height          = 130,
                Background      = AppColors.CardBg,
                CornerRadius    = new CornerRadius(8),
                BorderThickness = new Thickness(1),
                BorderBrush     = AppColors.CardBorder,
                Padding         = new Thickness(10),
                Margin          = new Thickness(0, 0, 8, 8),
                Child           = sp
            };
            return tile;
        }

        // ── Event: product tile "Add" button ─────────────────────────────
        private void AddTileBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            Product p = btn.Tag as Product;
            if (p == null) return;

            _vm.AddToCart(p);
            UpdateTotalsDisplay();

            if (_vm.HasError)
                ShowError(_vm.ErrorMessage);
        }

        // ── Event: search box ─────────────────────────────────────────────
        private void TbPosSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.ProductSearch = TbPosSearch.Text;
            RebuildProductTiles();
        }

        private void BtnPosRefresh_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Cart quantity buttons ─────────────────────────────────────────
        private void CartQtyBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn  = sender as Button;
            if (btn == null) return;
            POSCartItem item = btn.Tag as POSCartItem;
            if (item == null) return;
            _vm.DecrementCommand.Execute(item);
            UpdateTotalsDisplay();
        }

        private void CartQtyIncrBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn  = sender as Button;
            if (btn == null) return;
            POSCartItem item = btn.Tag as POSCartItem;
            if (item == null) return;
            _vm.IncrementCommand.Execute(item);
            UpdateTotalsDisplay();
        }

        private void CartRemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Button btn  = sender as Button;
            if (btn == null) return;
            POSCartItem item = btn.Tag as POSCartItem;
            if (item == null) return;
            _vm.RemoveItemCommand.Execute(item);
            UpdateTotalsDisplay();
        }

        // ── Cart control buttons ──────────────────────────────────────────
        private void BtnPosClear_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.CartItems.Count == 0) return;
            if (MessageBox.Show("Clear all cart items?", "Clear Cart",
                    MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _vm.ClearCartCommand.Execute(null);
                TbPosDiscount.Text = string.Empty;
                UpdateTotalsDisplay();
            }
        }

        private void BtnPosApplyDiscount_Click(object sender, RoutedEventArgs e)
        {
            _vm.DiscountInput = TbPosDiscount.Text.Trim();
            _vm.ApplyDiscountCommand.Execute(null);

            if (_vm.HasError)
                ShowError(_vm.ErrorMessage);
            else
                ShowError(null);
            UpdateTotalsDisplay();
        }

        private void BtnPosCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.CartItems.Count == 0) return;

            string msg = string.Format(
                "Items     : {0}\nSubtotal  : {1}\nDiscount  : {2}" +
                "\nVAT (12%) : {3}\n\nTOTAL DUE : {4}\n\nProceed with checkout?",
                _vm.TotalItems,
                _vm.SubtotalDisplay,
                _vm.DiscountDisplay,
                _vm.VATDisplay,
                _vm.GrandTotalDisplay);

            if (MessageBox.Show(msg, "Confirm Checkout",
                    MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes) return;

            try
            {
                Helpers.PrintHelper.PrintPOSReceipt(
                    new Models.POS_Session
                    {
                        CashierId     = App.CurrentUser != null ? App.CurrentUser.UserId : 0,
                        TotalSales    = _vm.GrandTotal,
                        PaymentMethod = Models.PaymentMethods.Cash,
                        StartTime     = System.DateTime.UtcNow,
                        EndTime       = System.DateTime.UtcNow
                    });

                MessageBox.Show(
                    "Sale recorded.\nTotal: " + _vm.GrandTotalDisplay,
                    "Done", MessageBoxButton.OK, MessageBoxImage.Information);

                _vm.ClearCartCommand.Execute(null);
                TbPosDiscount.Text = string.Empty;
                UpdateTotalsDisplay();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Failed to record transaction: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ── Totals display ────────────────────────────────────────────────
        private void UpdateTotalsDisplay()
        {
            TbPosSubtotal.Text  = _vm.SubtotalDisplay;
            TbPosDiscount2.Text = _vm.DiscountDisplay;
            TbPosVAT.Text       = _vm.VATDisplay;
            TbPosGrandTotal.Text = _vm.GrandTotalDisplay;
            BtnPosCheckout.IsEnabled = _vm.CartItems.Count > 0;
        }

        // ── Error helper ──────────────────────────────────────────────────
        private void ShowError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                PosErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbPosError.Text        = msg;
                PosErrorBar.Visibility = Visibility.Visible;
            }
        }
    }
}
