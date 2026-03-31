using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AdminSystem.Helpers;
using AdminSystem.Models;
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

        // ── Public entry points ─────────────────────────────────────────────

        /// <summary>
        /// Called by MainWindow whenever this page is activated or manually refreshed.
        /// </summary>
        public void Refresh()
        {
            _vm.LoadData();
            UpdateStatCards();
            UpdateSubtitle();
            DgRecentOrders.ItemsSource = _vm.RecentOrders;
            BuildLowStockPanel();
            ShowOrHideError();
        }

        // ── Event handlers ──────────────────────────────────────────────────

        private void BtnDashRefresh_Click(object sender, RoutedEventArgs e)
            => Refresh();

        private void BtnOpenPOS_Click(object sender, RoutedEventArgs e)
            => NavigateTo(PageNames.POS);

        private void BtnGoInventory_Click(object sender, RoutedEventArgs e)
            => NavigateTo(PageNames.Inventory);

        // ── Helpers ─────────────────────────────────────────────────────────

        private void UpdateStatCards()
        {
            TbActiveOrders.Text    = _vm.ActiveOrderCount.ToString();
            TbPendingPayments.Text = _vm.PendingPaymentCount.ToString();
            TbLowStock.Text        = _vm.LowStockCount.ToString();
            TbTodaySales.Text      = _vm.TodaySalesDisplay;
        }

        private void UpdateSubtitle()
        {
            string adminName = App.CurrentUser?.FullName ?? "Admin";
            TbSubtitle.Text  = string.Format(
                "{0:dddd, MMMM d, yyyy}  ·  Welcome back, {1}",
                DateTime.Now,
                adminName);
        }

        private void BuildLowStockPanel()
        {
            // Clear existing rows (keep TbNoLowStock sentinel at index 0)
            while (SpLowStock.Children.Count > 1)
                SpLowStock.Children.RemoveAt(1);

            TbLowStockBadge.Text = _vm.LowStockCount.ToString();

            if (_vm.LowStockItems.Count == 0)
            {
                TbNoLowStock.Visibility = Visibility.Visible;
                return;
            }

            TbNoLowStock.Visibility = Visibility.Collapsed;

            foreach (InventoryLog item in _vm.LowStockItems)
                SpLowStock.Children.Add(BuildLowStockRow(item));
        }

        private static Border BuildLowStockRow(InventoryLog item)
        {
            // ── Warning dot ──
            Border dot = new Border
            {
                Width        = 6,
                Height       = 6,
                CornerRadius = new CornerRadius(3),
                Background   = new SolidColorBrush(Color.FromRgb(0xDC, 0x26, 0x26)),
                Margin       = new Thickness(0, 0, 8, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            // ── Variant label ──
            TextBlock label = new TextBlock
            {
                Text       = item.ProductVariantId.HasValue
                             ? string.Format("Variant #{0}", item.ProductVariantId)
                             : string.Format("Product #{0}", item.ProductId),
                Foreground = new SolidColorBrush(Color.FromRgb(0xC0, 0xC0, 0xC0)),
                FontSize   = 12,
                FontFamily = new FontFamily("Segoe UI"),
                VerticalAlignment = VerticalAlignment.Center
            };

            // ── Stock count pill ──
            TextBlock stockNum = new TextBlock
            {
                Text       = item.ChangeQuantity.ToString(),
                Foreground = new SolidColorBrush(Color.FromRgb(0xEF, 0x44, 0x44)),
                FontSize   = 11,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
                VerticalAlignment = VerticalAlignment.Center
            };

            Border stockPill = new Border
            {
                Background   = new SolidColorBrush(Color.FromRgb(0x2A, 0x0A, 0x0A)),
                CornerRadius = new CornerRadius(4),
                Padding      = new Thickness(7, 2, 7, 2),
                VerticalAlignment = VerticalAlignment.Center,
                Child = stockNum
            };

            // ── Row grid ──
            Grid rowGrid = new Grid();
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            Grid.SetColumn(dot,      0);
            Grid.SetColumn(label,    1);
            Grid.SetColumn(stockPill, 2);

            rowGrid.Children.Add(dot);
            rowGrid.Children.Add(label);
            rowGrid.Children.Add(stockPill);

            Border row = new Border
            {
                Padding         = new Thickness(16, 10, 16, 10),
                BorderThickness = new Thickness(0, 0, 0, 1),
                BorderBrush     = new SolidColorBrush(Color.FromRgb(0x28, 0x28, 0x28)),
                Child           = rowGrid
            };

            // Hover effect
            row.MouseEnter += (s, e) =>
                row.Background = new SolidColorBrush(Color.FromRgb(0x2A, 0x2A, 0x2A));
            row.MouseLeave += (s, e) =>
                row.Background = Brushes.Transparent;

            return row;
        }

        private void ShowOrHideError()
        {
            if (_vm.HasError)
            {
                TbDashError.Text       = _vm.ErrorMessage;
                BdrError.Visibility    = Visibility.Visible;
            }
            else
            {
                BdrError.Visibility = Visibility.Collapsed;
            }
        }

        private void NavigateTo(string pageName)
        {
            if (Window.GetWindow(this) is MainWindow mainWin)
                mainWin.NavigateTo(pageName);
        }
    }
}
