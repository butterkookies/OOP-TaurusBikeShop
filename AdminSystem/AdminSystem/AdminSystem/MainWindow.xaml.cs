// ═══════════════════════════════════════════════════════════════════════
//  AdminDashboardWindow.xaml.cs
//  Taurus Bike Shop — Admin POS System
//
//  CHANGES (latest):
//  • Receipt / Invoice preview dialog
//    - Replaces the plain MessageBox confirm+success flow in BtnProcessPayment_Click
//    - Shows styled dark receipt with itemised lines, coupon row, totals,
//      payment method, customer, transaction ID, timestamp
//    - Two buttons: "Confirm & Process" clears cart; "Cancel" returns to POS
//  • Order Detail flyout panel (Pending Orders page)
//    - Clicking "View" on any order row opens a modal detail window
//    - Shows order header, customer info, placeholder item lines, status
//      badges, and Approve / Reject / Assign Logistics action buttons
//    - Wired via DgOrders_ViewOrder_Click; XAML "View" button updated with
//      Tag="{Binding OrderId}" and Click handler
//  • Custom Item entry in Walk-In POS
//    - "+ Custom" button added below the product grid in XAML
//    - Opens ShowCustomItemDialog: name text box + price text box + qty
//      spinner — same dark style as the existing quantity dialog
//    - Item merges into cart the same way catalogue items do
//  • All previous logic preserved
//  • C# 7.3 compatible
// ═══════════════════════════════════════════════════════════════════════
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaurusBikeShop
{
    // ════════════════════════════════════════════════════════════════════
    //  POS CART ITEM — lightweight model
    // ════════════════════════════════════════════════════════════════════
    internal class CartItem
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal => UnitPrice * Quantity;
    }

    public partial class AdminDashboardWindow : Window
    {
        private Button _activeNavBtn;
        private FrameworkElement _activePage;
        private Button _activeAnalyticsPeriodBtn;
        private bool _closeConfirmed = false;

        // ── POS cart state ────────────────────────────────────────────
        private readonly List<CartItem> _cartItems = new List<CartItem>();

        // ── Coupon store  (code → discount amount) ────────────────────
        //    Populated when admin creates a coupon; persists for the session.
        private readonly Dictionary<string, decimal> _coupons
            = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);

        // ── Currently applied coupon ──────────────────────────────────
        private string _appliedCouponCode = null;
        private decimal _appliedCouponAmount = 0m;

        // ── Admin password (hardcoded for now — swap for DB check later) ──
        private const string AdminPassword = "admin123";

        // ════════════════════════════════════════════════════════════
        //  ANALYTICS PLACEHOLDER DATA
        // ════════════════════════════════════════════════════════════
        private struct AnalyticsPeriodData
        {
            public string PeriodLabel;
            public string DateRange;
            public string Revenue;
            public string RevenueTrend;
            public string Orders;
            public string OrdersTrend;
            public string AvgOrder;
            public string AvgOrderTrend;
            public bool AvgOrderDown;
            public string Customers;
            public string CustomersTrend;
            public string WalkIn;
            public string WalkInSub;
            public string Online;
            public string OnlineSub;
            public string Refunds;
            public string RefundsSub;
            public string Cancelled;
            public string CancelledSub;
            public string ChartSubHint;
        }

        private static readonly AnalyticsPeriodData TodayData = new AnalyticsPeriodData
        {
            PeriodLabel = "Today",
            DateRange = " · Mar 10, 2026",
            Revenue = "₱ 42,850",
            RevenueTrend = "↑ 12.4% vs yesterday",
            Orders = "18",
            OrdersTrend = "↑ 3 more than yesterday",
            AvgOrder = "₱ 2,381",
            AvgOrderTrend = "↑ 4% vs yesterday",
            AvgOrderDown = false,
            Customers = "7",
            CustomersTrend = "↑ 2 more than yesterday",
            WalkIn = "₱ 15,200",
            WalkInSub = "8 transactions",
            Online = "₱ 27,650",
            OnlineSub = "10 transactions",
            Refunds = "₱ 0",
            RefundsSub = "0 refunds",
            Cancelled = "1",
            CancelledSub = "— same as yesterday",
            ChartSubHint = "Data source: Today"
        };
        private static readonly AnalyticsPeriodData WeekData = new AnalyticsPeriodData
        {
            PeriodLabel = "This Week",
            DateRange = " · Mar 4 – Mar 10, 2026",
            Revenue = "₱ 38,450",
            RevenueTrend = "↑ 9% vs last week",
            Orders = "24",
            OrdersTrend = "↑ 3 more than last week",
            AvgOrder = "₱ 1,602",
            AvgOrderTrend = "↓ 1% vs last week",
            AvgOrderDown = true,
            Customers = "9",
            CustomersTrend = "↑ 2 more than last week",
            WalkIn = "₱ 14,100",
            WalkInSub = "9 transactions",
            Online = "₱ 24,350",
            OnlineSub = "15 transactions",
            Refunds = "₱ 1,200",
            RefundsSub = "1 refund",
            Cancelled = "3",
            CancelledSub = "↓ 1 vs last week",
            ChartSubHint = "Data source: This Week"
        };
        private static readonly AnalyticsPeriodData MonthData = new AnalyticsPeriodData
        {
            PeriodLabel = "This Month",
            DateRange = " · Mar 1 – Mar 10, 2026",
            Revenue = "₱ 142,800",
            RevenueTrend = "↑ 12% vs last month",
            Orders = "87",
            OrdersTrend = "↑ 8 more than last month",
            AvgOrder = "₱ 1,641",
            AvgOrderTrend = "↓ 2% vs last month",
            AvgOrderDown = true,
            Customers = "34",
            CustomersTrend = "↑ 6 more than last month",
            WalkIn = "₱ 38,400",
            WalkInSub = "24 transactions",
            Online = "₱ 104,400",
            OnlineSub = "63 transactions",
            Refunds = "₱ 4,200",
            RefundsSub = "3 refunds",
            Cancelled = "7",
            CancelledSub = "↓ 2 vs last month",
            ChartSubHint = "Data source: This Month"
        };
        private static readonly AnalyticsPeriodData YearData = new AnalyticsPeriodData
        {
            PeriodLabel = "This Year",
            DateRange = " · Jan 1 – Mar 10, 2026",
            Revenue = "₱ 1,874,200",
            RevenueTrend = "↑ 22% vs last year",
            Orders = "1,043",
            OrdersTrend = "↑ 187 more than last year",
            AvgOrder = "₱ 1,797",
            AvgOrderTrend = "↑ 5% vs last year",
            AvgOrderDown = false,
            Customers = "312",
            CustomersTrend = "↑ 54 more than last year",
            WalkIn = "₱ 520,100",
            WalkInSub = "312 transactions",
            Online = "₱ 1,354,100",
            OnlineSub = "731 transactions",
            Refunds = "₱ 48,600",
            RefundsSub = "31 refunds",
            Cancelled = "67",
            CancelledSub = "↓ 12 vs last year",
            ChartSubHint = "Data source: This Year"
        };

        // ════════════════════════════════════════════════════════════
        //  CONSTRUCTOR
        // ════════════════════════════════════════════════════════════
        public AdminDashboardWindow()
        {
            InitializeComponent();

            // ── Sidebar navigation ──────────────────────────────────
            DashboardNavButton.Click += NavButton_Click;
            PendingOrdersNavButton.Click += NavButton_Click;
            PaymentVerificationNavButton.Click += NavButton_Click;
            LogisticsNavButton.Click += NavButton_Click;
            WalkInPosNavButton.Click += NavButton_Click;
            InventoryNavButton.Click += NavButton_Click;
            OrderHistoryNavButton.Click += NavButton_Click;
            AnalyticsNavButton.Click += NavButton_Click;
            SupportTicketsNavButton.Click += NavButton_Click;

            // ── Dashboard module tiles ──────────────────────────────
            OnlineOrdersModuleButton.Click += NavButton_Click;
            PaymentVerificationModuleButton.Click += NavButton_Click;
            LogisticsModuleButton.Click += NavButton_Click;
            WalkInPosModuleButton.Click += NavButton_Click;
            InventoryModuleButton.Click += NavButton_Click;
            OrderHistoryModuleButton.Click += NavButton_Click;
            AnalyticsModuleButton.Click += NavButton_Click;
            SupportTicketsModuleButton.Click += NavButton_Click;

            // ── Top-bar ─────────────────────────────────────────────
            ViewAllOrdersButton.Click += NavButton_Click;
            RefreshTopBarButton.Click += RefreshTopBarButton_Click;

            // ── Activity feed quick-nav ─────────────────────────────
            VerifyPaymentItem1Button.Click += NavButton_Click;
            VerifyPaymentItem2Button.Click += NavButton_Click;
            VerifyPaymentItem3Button.Click += NavButton_Click;
            ReviewInventoryButton.Click += NavButton_Click;
            ViewTicketButton.Click += NavButton_Click;

            // ── Logout & Exit ───────────────────────────────────────
            LogoutButton.Click += LogoutButton_Click;
            ExitButton.Click += ExitButton_Click;

            // ── Pending Orders ──────────────────────────────────────
            PO_ResetFilters.Click += PO_ResetFilters_Click;
            PO_BulkApprove.Click += PO_BulkApprove_Click;
            PO_BulkReject.Click += PO_BulkReject_Click;
            PO_BulkLogistics.Click += PO_BulkLogistics_Click;
            PO_BulkExport.Click += PO_BulkExport_Click;
            PO_ClearSelection.Click += PO_ClearSelection_Click;
            PO_PrevPage.Click += PO_PrevPage_Click;
            PO_NextPage.Click += PO_NextPage_Click;

            // ── Analytics period buttons ────────────────────────────
            Analytics_BtnToday.Click += AnalyticsPeriod_Click;
            Analytics_BtnWeek.Click += AnalyticsPeriod_Click;
            Analytics_BtnMonth.Click += AnalyticsPeriod_Click;
            Analytics_BtnYear.Click += AnalyticsPeriod_Click;

            // ── Walk-In POS product buttons ─────────────────────────
            BtnAddBike.Click += PosAddItem_Click;
            BtnAddGear.Click += PosAddItem_Click;
            BtnAddBrake.Click += PosAddItem_Click;
            BtnAddHelmet.Click += PosAddItem_Click;
            BtnAddRepair.Click += PosAddItem_Click;
            BtnAddLight.Click += PosAddItem_Click;

            // ── Walk-In POS cart actions ────────────────────────────
            BtnProcessPayment.Click += BtnProcessPayment_Click;
            BtnClearCart.Click += BtnClearCart_Click;
            BtnApplyCoupon.Click += BtnApplyCoupon_Click;
            BtnCreateCoupon.Click += BtnCreateCoupon_Click;
            BtnRemoveCoupon.Click += BtnRemoveCoupon_Click;
            BtnAddCustomItem.Click += BtnAddCustomItem_Click;

            // ── Live clock ──────────────────────────────────────────
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // ── Initial state ───────────────────────────────────────
            _activeNavBtn = DashboardNavButton;
            _activePage = PageDashboard;
            _activeAnalyticsPeriodBtn = Analytics_BtnMonth;
            ApplyAnalyticsData(MonthData);
        }

        // ════════════════════════════════════════════════════════════
        //  PRODUCT CATALOGUE  — name + price, keyed by button name
        // ════════════════════════════════════════════════════════════
        private static readonly Dictionary<string, (string Name, decimal Price)> _productCatalogue
            = new Dictionary<string, (string, decimal)>
            {
                { "BtnAddBike",   ("Road Bike Frame",   4500m) },
                { "BtnAddGear",   ("Gear Set 21-Speed", 1200m) },
                { "BtnAddBrake",  ("Brake Kit",          850m) },
                { "BtnAddHelmet", ("Helmet Pro",        1800m) },
                { "BtnAddRepair", ("Repair Kit",         450m) },
                { "BtnAddLight",  ("Bike Light Set",     680m) },
            };

        // ════════════════════════════════════════════════════════════
        //  POS — ADD ITEM (opens quantity dialog)
        // ════════════════════════════════════════════════════════════
        private void PosAddItem_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;

            if (!_productCatalogue.TryGetValue(btn.Name, out var product)) return;

            int qty = ShowQuantityDialog(product.Name, product.Price);
            if (qty <= 0) return;

            // Merge with existing cart entry if same product already added
            CartItem existing = _cartItems.Find(c => c.Name == product.Name);
            if (existing != null)
                existing.Quantity += qty;
            else
                _cartItems.Add(new CartItem { Name = product.Name, UnitPrice = product.Price, Quantity = qty });

            RefreshCartUI();
        }

        // ════════════════════════════════════════════════════════════
        //  QUANTITY DIALOG  — lightweight inline WPF dialog
        // ════════════════════════════════════════════════════════════
        private int ShowQuantityDialog(string productName, decimal unitPrice)
        {
            var dialog = new Window
            {
                Title = "Add to Cart",
                Width = 320,
                Height = 255,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x24)),
                WindowStyle = WindowStyle.ToolWindow,
            };

            var panel = new StackPanel { Margin = new Thickness(20) };

            panel.Children.Add(new TextBlock
            {
                Text = productName,
                Foreground = Brushes.WhiteSmoke,
                FontSize = 14,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 4),
            });
            panel.Children.Add(new TextBlock
            {
                Text = string.Format("₱ {0:N2} each", unitPrice),
                Foreground = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                FontSize = 12,
                Margin = new Thickness(0, 0, 0, 14),
            });

            // Qty row: [−] [TextBox] [+]
            var qtyRow = new StackPanel { Orientation = Orientation.Horizontal };

            var btnMinus = new Button
            {
                Content = "−",
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 18,
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            var qtyBox = new TextBox
            {
                Text = "1",
                Width = 60,
                Height = 36,
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                Foreground = Brushes.WhiteSmoke,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 14,
                Margin = new Thickness(4, 0, 4, 0),
            };
            var btnPlus = new Button
            {
                Content = "+",
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 18,
                Cursor = System.Windows.Input.Cursors.Hand,
            };

            var totalLabel = new TextBlock
            {
                Text = string.Format("Total: \u20b1 {0:N2}", unitPrice),
                Foreground = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin = new Thickness(0, 12, 0, 0),
            };

            Action updateTotal = () =>
            {
                if (int.TryParse(qtyBox.Text, out int qty) && qty > 0)
                    totalLabel.Text = string.Format("Total: \u20b1 {0:N2}", unitPrice * qty);
            };

            btnMinus.Click += (s, ev) =>
            {
                if (int.TryParse(qtyBox.Text, out int v) && v > 1) qtyBox.Text = (v - 1).ToString();
                updateTotal();
            };
            btnPlus.Click += (s, ev) =>
            {
                if (int.TryParse(qtyBox.Text, out int v)) qtyBox.Text = (v + 1).ToString();
                updateTotal();
            };
            qtyBox.TextChanged += (s, ev) => updateTotal();

            qtyRow.Children.Add(btnMinus);
            qtyRow.Children.Add(qtyBox);
            qtyRow.Children.Add(btnPlus);
            panel.Children.Add(qtyRow);
            panel.Children.Add(totalLabel);

            int result = 0;
            var btnConfirm = new Button
            {
                Content = "Add to Cart",
                Height = 36,
                Margin = new Thickness(0, 12, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnConfirm.Click += (s, ev) =>
            {
                if (int.TryParse(qtyBox.Text, out int v) && v > 0)
                    result = v;
                dialog.Close();
            };
            panel.Children.Add(btnConfirm);

            dialog.Content = panel;
            dialog.ShowDialog();
            return result;
        }

        // ════════════════════════════════════════════════════════════
        //  CART UI REFRESH  — rebuilds PosCartList from _cartItems
        // ════════════════════════════════════════════════════════════
        private void RefreshCartUI()
        {
            PosCartList.Children.Clear();

            if (_cartItems.Count == 0)
            {
                PosCartList.Children.Add(new TextBlock
                {
                    Text = "Cart is empty",
                    Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 24, 0, 0),
                    FontFamily = new FontFamily("Segoe UI"),
                    FontSize = 12,
                });
                UpdateCartTotals();
                return;
            }

            foreach (CartItem item in _cartItems)
            {
                CartItem captured = item;

                var card = new Border
                {
                    Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                    CornerRadius = new CornerRadius(5),
                    Padding = new Thickness(10, 8, 10, 8),
                    Margin = new Thickness(0, 0, 0, 6),
                    BorderThickness = new Thickness(1),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                };

                var cardStack = new StackPanel();

                // ── Top row: name + remove button ───────────────────
                var topRow = new Grid();
                topRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                topRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                var nameBlock = new TextBlock
                {
                    Text = item.Name,
                    Foreground = Brushes.WhiteSmoke,
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("Segoe UI"),
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                };
                Grid.SetColumn(nameBlock, 0);

                var removeBtn = new Button
                {
                    Content = "✕",
                    Width = 20,
                    Height = 20,
                    Background = Brushes.Transparent,
                    Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)),
                    BorderThickness = new Thickness(0),
                    FontSize = 10,
                    Cursor = System.Windows.Input.Cursors.Hand,
                    Padding = new Thickness(0),
                    VerticalContentAlignment = VerticalAlignment.Center,
                    HorizontalContentAlignment = HorizontalAlignment.Center,
                };
                removeBtn.Click += (s, ev) =>
                {
                    _cartItems.Remove(captured);
                    RefreshCartUI();
                };
                Grid.SetColumn(removeBtn, 1);

                topRow.Children.Add(nameBlock);
                topRow.Children.Add(removeBtn);
                cardStack.Children.Add(topRow);

                // ── Bottom row: qty controls + line total ────────────
                var bottomRow = new Grid { Margin = new Thickness(0, 6, 0, 0) };
                bottomRow.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                bottomRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                var qtyPanel = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center };

                var qtyMinus = BuildQtyButton("−");
                var qtyLabel = new TextBlock
                {
                    Text = item.Quantity.ToString(),
                    Foreground = Brushes.WhiteSmoke,
                    FontSize = 12,
                    FontFamily = new FontFamily("Segoe UI"),
                    Width = 28,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                var qtyPlus = BuildQtyButton("+");

                qtyMinus.Click += (s, ev) =>
                {
                    if (captured.Quantity > 1) { captured.Quantity--; RefreshCartUI(); }
                    else { _cartItems.Remove(captured); RefreshCartUI(); }
                };
                qtyPlus.Click += (s, ev) =>
                {
                    captured.Quantity++;
                    RefreshCartUI();
                };

                qtyPanel.Children.Add(qtyMinus);
                qtyPanel.Children.Add(qtyLabel);
                qtyPanel.Children.Add(qtyPlus);
                Grid.SetColumn(qtyPanel, 0);

                var lineTotal = new TextBlock
                {
                    Text = string.Format("₱ {0:N2}", item.LineTotal),
                    Foreground = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("Segoe UI"),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetColumn(lineTotal, 1);

                bottomRow.Children.Add(qtyPanel);
                bottomRow.Children.Add(lineTotal);
                cardStack.Children.Add(bottomRow);

                card.Child = cardStack;
                PosCartList.Children.Add(card);
            }

            UpdateCartTotals();
        }

        // ── Builds a small [−] or [+] qty button ────────────────────
        private static Button BuildQtyButton(string label)
        {
            return new Button
            {
                Content = label,
                Width = 24,
                Height = 24,
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                Foreground = Brushes.WhiteSmoke,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                FontSize = 13,
                Cursor = System.Windows.Input.Cursors.Hand,
                Padding = new Thickness(0),
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
        }

        // ── Recalculate and display totals ───────────────────────────
        private void UpdateCartTotals()
        {
            decimal subtotal = 0m;
            foreach (CartItem c in _cartItems) subtotal += c.LineTotal;

            decimal discount = Math.Min(_appliedCouponAmount, subtotal);
            decimal total = subtotal - discount;

            PosSubtotalText.Text = string.Format("₱ {0:N2}", subtotal);
            PosDiscountText.Text = discount > 0
                ? string.Format("— ₱ {0:N2}", discount)
                : "— ₱ 0.00";
            PosTotalText.Text = string.Format("₱ {0:N2}", total);
        }

        // ════════════════════════════════════════════════════════════
        //  POS — CREATE COUPON  (admin-password-gated)
        // ════════════════════════════════════════════════════════════
        private void BtnCreateCoupon_Click(object sender, RoutedEventArgs e)
        {
            // ── Use SizeToContent so the window always fits all controls ──
            var dialog = new Window
            {
                Title = "Create Coupon",
                Width = 360,
                SizeToContent = SizeToContent.Height,   // height auto-expands for error label
                MinHeight = 100,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x24)),
                WindowStyle = WindowStyle.ToolWindow,
            };

            // ── Outer ScrollViewer guards against tiny screens ───────────
            var scroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            var panel = new StackPanel { Margin = new Thickness(22, 20, 22, 22) };

            // Title
            panel.Children.Add(new TextBlock
            {
                Text = "Create New Coupon",
                Foreground = Brushes.WhiteSmoke,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin = new Thickness(0, 0, 0, 18),
            });

            // ── Coupon Code ──────────────────────────────────────────────
            panel.Children.Add(MakeDialogLabel("Coupon Code"));
            var codeBox = MakeDialogTextBox("e.g. SAVE100");
            panel.Children.Add(codeBox);

            // ── Discount Amount ──────────────────────────────────────────
            panel.Children.Add(MakeDialogLabel("Discount Amount (₱)", topMargin: 12));
            var amountBox = MakeDialogTextBox("e.g. 500");
            panel.Children.Add(amountBox);

            // ── Admin Password ───────────────────────────────────────────
            panel.Children.Add(MakeDialogLabel("Admin Password", topMargin: 12));
            var passBox = new PasswordBox
            {
                Height = 34,
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                Foreground = Brushes.WhiteSmoke,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                BorderThickness = new Thickness(1),
                FontSize = 13,
                Padding = new Thickness(8, 0, 8, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
            };
            panel.Children.Add(passBox);

            // ── Error banner (hidden until a validation fails) ───────────
            var errorBanner = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x10, 0x10)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(10, 7, 10, 7),
                Margin = new Thickness(0, 12, 0, 0),
                Visibility = Visibility.Collapsed,
            };
            var errorText = new TextBlock
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0x66, 0x66)),
                FontSize = 12,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap,
            };
            errorBanner.Child = errorText;
            panel.Children.Add(errorBanner);

            // Helper to show the error banner
            Action<string> showError = (msg) =>
            {
                errorText.Text = "⚠  " + msg;
                errorBanner.Visibility = Visibility.Visible;
            };

            // ── Create Coupon button ─────────────────────────────────────
            var btnCreate = new Button
            {
                Content = "Create Coupon",
                Height = 38,
                Margin = new Thickness(0, 14, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            btnCreate.Click += (s, ev) =>
            {
                // Hide any previous error first
                errorBanner.Visibility = Visibility.Collapsed;

                string code = codeBox.Text.Trim().ToUpper();
                string rawAmt = amountBox.Text.Trim();
                string pass = passBox.Password;

                // ── Validate ─────────────────────────────────────────────
                if (string.IsNullOrEmpty(code))
                {
                    showError("Please enter a coupon code.");
                    codeBox.Focus();
                    return;
                }

                if (_coupons.ContainsKey(code))
                {
                    showError(string.Format("Coupon \"{0}\" already exists.", code));
                    codeBox.SelectAll();
                    codeBox.Focus();
                    return;
                }

                if (!decimal.TryParse(rawAmt, out decimal amt) || amt <= 0)
                {
                    showError("Enter a valid discount amount greater than 0.");
                    amountBox.SelectAll();
                    amountBox.Focus();
                    return;
                }

                if (pass != AdminPassword)
                {
                    showError("Incorrect admin password. Please try again.");
                    passBox.Clear();
                    passBox.Focus();
                    return;
                }

                // ── All good — save and close ────────────────────────────
                _coupons[code] = amt;
                dialog.Close();

                MessageBox.Show(
                    string.Format("Coupon \"{0}\" created successfully!\nDiscount: ₱ {1:N2}", code, amt),
                    "Coupon Created",
                    MessageBoxButton.OK,
                    MessageBoxImage.None);
            };

            panel.Children.Add(btnCreate);

            scroll.Content = panel;
            dialog.Content = scroll;
            dialog.ShowDialog();
        }

        // ── Small helpers for building dialog controls ───────────────
        private static TextBlock MakeDialogLabel(string text, double topMargin = 0)
        {
            return new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)),
                FontSize = 11,
                FontFamily = new System.Windows.Media.FontFamily("Segoe UI"),
                Margin = new Thickness(0, topMargin, 0, 4),
            };
        }

        private static TextBox MakeDialogTextBox(string placeholder)
        {
            return new TextBox
            {
                Height = 34,
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                Foreground = Brushes.WhiteSmoke,
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                BorderThickness = new Thickness(1),
                FontSize = 13,
                Padding = new Thickness(8, 0, 8, 0),
                VerticalContentAlignment = VerticalAlignment.Center,
                Tag = placeholder,
            };
        }

        private static void ShowStatus(TextBlock label, string message)
        {
            label.Text = message;
            label.Visibility = Visibility.Visible;
        }

        // ════════════════════════════════════════════════════════════
        //  POS — APPLY COUPON
        // ════════════════════════════════════════════════════════════
        private void BtnApplyCoupon_Click(object sender, RoutedEventArgs e)
        {
            string code = PosCouponBox.Text.Trim().ToUpper();

            if (string.IsNullOrEmpty(code))
            {
                MessageBox.Show("Please enter a coupon code.", "No Code",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!_coupons.TryGetValue(code, out decimal discount))
            {
                MessageBox.Show(
                    string.Format("Coupon \"{0}\" not found.", code),
                    "Invalid Coupon",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            _appliedCouponCode = code;
            _appliedCouponAmount = discount;

            PosCouponBadge.Visibility = Visibility.Visible;
            PosCouponBadgeLabel.Text = string.Format("🏷 {0} — ₱ {1:N2} off", code, discount);
            PosCouponBox.Text = string.Empty;

            UpdateCartTotals();
        }

        // ════════════════════════════════════════════════════════════
        //  POS — REMOVE COUPON
        // ════════════════════════════════════════════════════════════
        private void BtnRemoveCoupon_Click(object sender, RoutedEventArgs e)
        {
            _appliedCouponCode = null;
            _appliedCouponAmount = 0m;
            PosCouponBadge.Visibility = Visibility.Collapsed;
            UpdateCartTotals();
        }

        // ════════════════════════════════════════════════════════════
        //  POS — PROCESS PAYMENT  (opens receipt preview dialog)
        // ════════════════════════════════════════════════════════════
        private void BtnProcessPayment_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0)
            {
                MessageBox.Show("Cart is empty. Please add items before processing payment.",
                    "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            decimal subtotal = 0m;
            foreach (CartItem c in _cartItems) subtotal += c.LineTotal;
            decimal discount = Math.Min(_appliedCouponAmount, subtotal);
            decimal total = subtotal - discount;

            string method = (PosPaymentCombo.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Cash";
            string customer = string.IsNullOrWhiteSpace(PosCustomerNameBox.Text)
                ? "Walk-In Customer"
                : PosCustomerNameBox.Text.Trim();
            string txnId = "TXN-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            string txnTime = DateTime.Now.ToString("MMM dd, yyyy  hh:mm tt");

            // ── Build receipt dialog ─────────────────────────────────
            var dlg = new Window
            {
                Title = "Receipt Preview",
                Width = 400,
                SizeToContent = SizeToContent.Height,
                MaxHeight = 680,
                MinHeight = 200,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                WindowStyle = WindowStyle.ToolWindow,
            };

            var outerScroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            var root = new StackPanel { Margin = new Thickness(0) };

            // ── Receipt header band ──────────────────────────────────
            var headerBand = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                Padding = new Thickness(24, 18, 24, 18),
            };
            var headerStack = new StackPanel { HorizontalAlignment = HorizontalAlignment.Center };
            headerStack.Children.Add(new TextBlock
            {
                Text = "🐂  TAURUS BIKE SHOP",
                Foreground = Brushes.White,
                FontSize = 15,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI"),
                HorizontalAlignment = HorizontalAlignment.Center,
            });
            headerStack.Children.Add(new TextBlock
            {
                Text = "OFFICIAL RECEIPT",
                Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0xCC, 0xCC)),
                FontSize = 10,
                FontFamily = new FontFamily("Segoe UI"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 3, 0, 0),
                LetterSpacing = 2,
            });
            headerBand.Child = headerStack;
            root.Children.Add(headerBand);

            // ── Receipt body ─────────────────────────────────────────
            var body = new StackPanel { Margin = new Thickness(24, 16, 24, 8) };

            // Meta row
            var metaGrid = new Grid { Margin = new Thickness(0, 0, 0, 14) };
            metaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            metaGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var metaLeft = new StackPanel();
            metaLeft.Children.Add(MakeReceiptMeta("Transaction ID", txnId));
            metaLeft.Children.Add(MakeReceiptMeta("Date & Time", txnTime));
            Grid.SetColumn(metaLeft, 0);

            var metaRight = new StackPanel { HorizontalAlignment = HorizontalAlignment.Right };
            metaRight.Children.Add(MakeReceiptMeta("Customer", customer, rightAlign: true));
            metaRight.Children.Add(MakeReceiptMeta("Payment", method, rightAlign: true));
            Grid.SetColumn(metaRight, 1);

            metaGrid.Children.Add(metaLeft);
            metaGrid.Children.Add(metaRight);
            body.Children.Add(metaGrid);

            // Divider
            body.Children.Add(MakeReceiptDivider());

            // Column headers
            var colHeader = new Grid { Margin = new Thickness(0, 8, 0, 6) };
            colHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            colHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            colHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
            colHeader.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

            void AddColHead(string text, int col, TextAlignment align = TextAlignment.Left)
            {
                var tb = new TextBlock
                {
                    Text = text,
                    Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88)),
                    FontSize = 10,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("Segoe UI"),
                    TextAlignment = align,
                };
                Grid.SetColumn(tb, col);
                colHeader.Children.Add(tb);
            }
            AddColHead("ITEM", 0);
            AddColHead("QTY", 2, TextAlignment.Center);
            AddColHead("AMOUNT", 3, TextAlignment.Right);
            body.Children.Add(colHeader);

            body.Children.Add(MakeReceiptDivider(dashed: true));

            // Item rows
            foreach (CartItem item in _cartItems)
            {
                var row = new Grid { Margin = new Thickness(0, 4, 0, 4) };
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(70) });
                row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(80) });

                var nameBlock = new StackPanel();
                nameBlock.Children.Add(new TextBlock
                {
                    Text = item.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0)),
                    FontSize = 12,
                    FontFamily = new FontFamily("Segoe UI"),
                    TextWrapping = TextWrapping.Wrap,
                });
                nameBlock.Children.Add(new TextBlock
                {
                    Text = string.Format("₱ {0:N2} each", item.UnitPrice),
                    Foreground = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77)),
                    FontSize = 10,
                    FontFamily = new FontFamily("Segoe UI"),
                    Margin = new Thickness(0, 1, 0, 0),
                });
                Grid.SetColumn(nameBlock, 0);

                var qtyBlock = new TextBlock
                {
                    Text = item.Quantity.ToString(),
                    Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)),
                    FontSize = 12,
                    FontFamily = new FontFamily("Segoe UI"),
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetColumn(qtyBlock, 2);

                var totalBlock = new TextBlock
                {
                    Text = string.Format("₱ {0:N2}", item.LineTotal),
                    Foreground = new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0)),
                    FontSize = 12,
                    FontWeight = FontWeights.SemiBold,
                    FontFamily = new FontFamily("Segoe UI"),
                    TextAlignment = TextAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Center,
                };
                Grid.SetColumn(totalBlock, 3);

                row.Children.Add(nameBlock);
                row.Children.Add(qtyBlock);
                row.Children.Add(totalBlock);
                body.Children.Add(row);
            }

            body.Children.Add(MakeReceiptDivider());

            // Subtotal row
            body.Children.Add(MakeReceiptTotalRow("Subtotal", string.Format("₱ {0:N2}", subtotal), muted: true));

            // Coupon row (only if applied)
            if (discount > 0)
            {
                body.Children.Add(MakeReceiptTotalRow(
                    string.Format("Coupon  ({0})", _appliedCouponCode),
                    string.Format("— ₱ {0:N2}", discount),
                    muted: true,
                    green: true));
            }

            body.Children.Add(MakeReceiptDivider());

            // Grand total
            body.Children.Add(MakeReceiptTotalRow(
                "TOTAL",
                string.Format("₱ {0:N2}", total),
                large: true));

            body.Children.Add(new TextBlock
            {
                Text = "Thank you for shopping at Taurus Bike Shop!",
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                FontSize = 10,
                FontFamily = new FontFamily("Segoe UI"),
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 14, 0, 6),
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
            });

            root.Children.Add(body);

            // ── Action buttons ───────────────────────────────────────
            var btnRow = new Grid
            {
                Margin = new Thickness(24, 0, 24, 20),
            };
            btnRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            btnRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(8) });
            btnRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            bool confirmed = false;

            var btnCancel = new Button
            {
                Content = "Cancel",
                Height = 38,
                Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                FontSize = 13,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnCancel.Click += (s, ev) => dlg.Close();
            Grid.SetColumn(btnCancel, 0);

            var btnConfirm = new Button
            {
                Content = "✔  Confirm & Process",
                Height = 38,
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x80, 0x40)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnConfirm.Click += (s, ev) =>
            {
                confirmed = true;
                dlg.Close();
            };
            Grid.SetColumn(btnConfirm, 2);

            btnRow.Children.Add(btnCancel);
            btnRow.Children.Add(btnConfirm);
            root.Children.Add(btnRow);

            outerScroll.Content = root;
            dlg.Content = outerScroll;
            dlg.ShowDialog();

            // ── If confirmed, clear cart ─────────────────────────────
            if (confirmed)
            {
                _cartItems.Clear();
                _appliedCouponCode = null;
                _appliedCouponAmount = 0m;
                PosCouponBadge.Visibility = Visibility.Collapsed;
                PosCustomerNameBox.Text = string.Empty;
                RefreshCartUI();
            }
        }

        // ── Receipt helper: meta label+value pair ────────────────────
        private static StackPanel MakeReceiptMeta(string label, string value, bool rightAlign = false)
        {
            var align = rightAlign ? HorizontalAlignment.Right : HorizontalAlignment.Left;
            var ta = rightAlign ? TextAlignment.Right : TextAlignment.Left;
            var sp = new StackPanel { HorizontalAlignment = align, Margin = new Thickness(0, 0, 0, 4) };
            sp.Children.Add(new TextBlock
            {
                Text = label.ToUpper(),
                Foreground = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77)),
                FontSize = 9,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
                TextAlignment = ta,
            });
            sp.Children.Add(new TextBlock
            {
                Text = value,
                Foreground = new SolidColorBrush(Color.FromRgb(0xCC, 0xCC, 0xCC)),
                FontSize = 11,
                FontFamily = new FontFamily("Segoe UI"),
                TextAlignment = ta,
                TextWrapping = TextWrapping.Wrap,
            });
            return sp;
        }

        // ── Receipt helper: horizontal divider ───────────────────────
        private static Border MakeReceiptDivider(bool dashed = false)
        {
            return new Border
            {
                Height = 1,
                Background = new SolidColorBrush(Color.FromRgb(0x2E, 0x2E, 0x2E)),
                Margin = new Thickness(0, 4, 0, 4),
                Opacity = dashed ? 0.5 : 1.0,
            };
        }

        // ── Receipt helper: total / subtotal row ─────────────────────
        private static Grid MakeReceiptTotalRow(
            string label, string value,
            bool muted = false, bool green = false, bool large = false)
        {
            var row = new Grid { Margin = new Thickness(0, 3, 0, 3) };
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            row.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var fgColor = green ? Color.FromRgb(0x4C, 0xAF, 0x50)
                        : muted ? Color.FromRgb(0x88, 0x88, 0x88)
                        : Color.FromRgb(0xF0, 0xF0, 0xF0);

            double fs = large ? 15 : 12;
            var fw = large ? FontWeights.Bold : FontWeights.Normal;

            var lblBlock = new TextBlock
            {
                Text = label,
                Foreground = new SolidColorBrush(fgColor),
                FontSize = fs,
                FontWeight = fw,
                FontFamily = new FontFamily("Segoe UI"),
            };
            Grid.SetColumn(lblBlock, 0);

            var valBlock = new TextBlock
            {
                Text = value,
                Foreground = new SolidColorBrush(fgColor),
                FontSize = fs,
                FontWeight = fw,
                FontFamily = new FontFamily("Segoe UI"),
                TextAlignment = TextAlignment.Right,
            };
            Grid.SetColumn(valBlock, 1);

            row.Children.Add(lblBlock);
            row.Children.Add(valBlock);
            return row;
        }

        // ════════════════════════════════════════════════════════════
        //  POS — CLEAR CART
        // ════════════════════════════════════════════════════════════
        private void BtnClearCart_Click(object sender, RoutedEventArgs e)
        {
            if (_cartItems.Count == 0) return;

            var result = MessageBox.Show("Clear all items from the cart?", "Clear Cart",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _cartItems.Clear();
                RefreshCartUI();
            }
        }

        // ════════════════════════════════════════════════════════════
        //  POS — CUSTOM ITEM ENTRY
        // ════════════════════════════════════════════════════════════
        private void BtnAddCustomItem_Click(object sender, RoutedEventArgs e)
        {
            var result = ShowCustomItemDialog(out string itemName, out decimal unitPrice, out int qty);
            if (!result) return;

            CartItem existing = _cartItems.Find(c => c.Name == itemName);
            if (existing != null)
                existing.Quantity += qty;
            else
                _cartItems.Add(new CartItem { Name = itemName, UnitPrice = unitPrice, Quantity = qty });

            RefreshCartUI();
        }

        private bool ShowCustomItemDialog(out string itemName, out decimal unitPrice, out int qty)
        {
            itemName = string.Empty;
            unitPrice = 0m;
            qty = 0;

            var dlg = new Window
            {
                Title = "Add Custom Item",
                Width = 340,
                SizeToContent = SizeToContent.Height,
                MinHeight = 100,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x24)),
                WindowStyle = WindowStyle.ToolWindow,
            };

            var scroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            var panel = new StackPanel { Margin = new Thickness(22, 20, 22, 22) };

            panel.Children.Add(new TextBlock
            {
                Text = "Custom Item",
                Foreground = Brushes.WhiteSmoke,
                FontSize = 15,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
                Margin = new Thickness(0, 0, 0, 4),
            });
            panel.Children.Add(new TextBlock
            {
                Text = "Add a product or service not in the catalogue.",
                Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88)),
                FontSize = 11,
                FontFamily = new FontFamily("Segoe UI"),
                Margin = new Thickness(0, 0, 0, 18),
                TextWrapping = TextWrapping.Wrap,
            });

            // Item name
            panel.Children.Add(MakeDialogLabel("Item Name / Description"));
            var nameBox = MakeDialogTextBox("e.g. Custom Repair Service");
            panel.Children.Add(nameBox);

            // Unit price
            panel.Children.Add(MakeDialogLabel("Unit Price (₱)", topMargin: 12));
            var priceBox = MakeDialogTextBox("e.g. 750");
            panel.Children.Add(priceBox);

            // Quantity row
            panel.Children.Add(MakeDialogLabel("Quantity", topMargin: 12));

            var qtyRow = new StackPanel { Orientation = Orientation.Horizontal };
            var btnMinus = new Button
            {
                Content = "−",
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 18,
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            var qtyBox = new TextBox
            {
                Text = "1",
                Width = 60,
                Height = 36,
                TextAlignment = TextAlignment.Center,
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                Foreground = Brushes.WhiteSmoke,
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                VerticalContentAlignment = VerticalAlignment.Center,
                FontSize = 14,
                Margin = new Thickness(4, 0, 4, 0),
            };
            var btnPlus = new Button
            {
                Content = "+",
                Width = 36,
                Height = 36,
                Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 18,
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnMinus.Click += (s, ev) =>
            {
                if (int.TryParse(qtyBox.Text, out int v) && v > 1) qtyBox.Text = (v - 1).ToString();
            };
            btnPlus.Click += (s, ev) =>
            {
                if (int.TryParse(qtyBox.Text, out int v)) qtyBox.Text = (v + 1).ToString();
            };
            qtyRow.Children.Add(btnMinus);
            qtyRow.Children.Add(qtyBox);
            qtyRow.Children.Add(btnPlus);
            panel.Children.Add(qtyRow);

            // Error banner
            var errorBanner = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x10, 0x10)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(4),
                Padding = new Thickness(10, 7, 10, 7),
                Margin = new Thickness(0, 12, 0, 0),
                Visibility = Visibility.Collapsed,
            };
            var errorText = new TextBlock
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0xFF, 0x66, 0x66)),
                FontSize = 12,
                FontFamily = new FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap,
            };
            errorBanner.Child = errorText;
            panel.Children.Add(errorBanner);

            Action<string> showError = (msg) =>
            {
                errorText.Text = "⚠  " + msg;
                errorBanner.Visibility = Visibility.Visible;
            };

            bool confirmed = false;

            var btnAdd = new Button
            {
                Content = "Add to Cart",
                Height = 38,
                Margin = new Thickness(0, 14, 0, 0),
                Background = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                Foreground = Brushes.White,
                BorderThickness = new Thickness(0),
                FontSize = 13,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            btnAdd.Click += (s, ev) =>
            {
                errorBanner.Visibility = Visibility.Collapsed;

                string name = nameBox.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    showError("Please enter an item name.");
                    nameBox.Focus();
                    return;
                }
                if (!decimal.TryParse(priceBox.Text.Trim(), out decimal price) || price <= 0)
                {
                    showError("Enter a valid unit price greater than 0.");
                    priceBox.SelectAll();
                    priceBox.Focus();
                    return;
                }
                if (!int.TryParse(qtyBox.Text, out int q) || q <= 0)
                {
                    showError("Quantity must be at least 1.");
                    qtyBox.SelectAll();
                    qtyBox.Focus();
                    return;
                }

                itemName = name;
                unitPrice = price;
                qty = q;
                confirmed = true;
                dlg.Close();
            };

            panel.Children.Add(btnAdd);
            scroll.Content = panel;
            dlg.Content = scroll;
            dlg.ShowDialog();
            return confirmed;
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — View Order detail flyout
        // ════════════════════════════════════════════════════════════
        //  Called from the "View" button in the DgOrders DataGrid.
        //  The button in XAML should have:
        //    Click="DgOrders_ViewOrder_Click"  Tag="{Binding OrderId}"
        // ════════════════════════════════════════════════════════════
        public void DgOrders_ViewOrder_Click(object sender, RoutedEventArgs e)
        {
            string orderId = (sender as Button)?.Tag?.ToString() ?? "ORD-UNKNOWN";
            ShowOrderDetailDialog(orderId);
        }

        private void ShowOrderDetailDialog(string orderId)
        {
            // ── Placeholder data — swap these fields for real bindings later ──
            string customer = "Juan dela Cruz";
            string email = "juan@example.com";
            string phone = "+63 917 123 4567";
            string address = "123 Rizal St., Brgy. Poblacion, Laoag City, Ilocos Norte";
            string orderDate = "Mar 10, 2026  09:14 AM";
            string payStatus = "Pending";
            string orderStatus = "Pending";
            string method = "GCash";
            decimal subtotal = 6750m;
            decimal shipping = 250m;
            decimal grandTotal = 7000m;

            var dlg = new Window
            {
                Title = string.Format("Order Details  —  {0}", orderId),
                Width = 540,
                SizeToContent = SizeToContent.Height,
                MaxHeight = 700,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                Background = new SolidColorBrush(Color.FromRgb(0x1A, 0x1A, 0x1A)),
                WindowStyle = WindowStyle.ToolWindow,
            };

            var outerScroll = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            var root = new StackPanel { Margin = new Thickness(0) };

            // ── Header band ──────────────────────────────────────────
            var hdr = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x24, 0x24, 0x24)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                BorderThickness = new Thickness(0, 0, 0, 1),
                Padding = new Thickness(22, 16, 22, 16),
            };
            var hdrGrid = new Grid();
            hdrGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            hdrGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

            var hdrLeft = new StackPanel();
            hdrLeft.Children.Add(new TextBlock
            {
                Text = orderId,
                Foreground = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                FontSize = 17,
                FontWeight = FontWeights.Bold,
                FontFamily = new FontFamily("Segoe UI"),
            });
            hdrLeft.Children.Add(new TextBlock
            {
                Text = "Placed: " + orderDate,
                Foreground = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77)),
                FontSize = 11,
                FontFamily = new FontFamily("Segoe UI"),
                Margin = new Thickness(0, 2, 0, 0),
            });
            Grid.SetColumn(hdrLeft, 0);

            var hdrRight = new StackPanel { HorizontalAlignment = HorizontalAlignment.Right };
            hdrRight.Children.Add(MakeStatusBadge(payStatus, isPayment: true));
            hdrRight.Children.Add(MakeStatusBadge(orderStatus, isPayment: false));
            Grid.SetColumn(hdrRight, 1);

            hdrGrid.Children.Add(hdrLeft);
            hdrGrid.Children.Add(hdrRight);
            hdr.Child = hdrGrid;
            root.Children.Add(hdr);

            var body = new StackPanel { Margin = new Thickness(22, 16, 22, 16) };

            // ── Customer info ────────────────────────────────────────
            body.Children.Add(MakeSectionLabel("CUSTOMER"));
            var custCard = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(0, 6, 0, 16),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
            };
            var custGrid = new Grid();
            custGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            custGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            var custLeft = new StackPanel();
            custLeft.Children.Add(new TextBlock { Text = customer, Foreground = new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0)), FontSize = 13, FontWeight = FontWeights.SemiBold, FontFamily = new FontFamily("Segoe UI") });
            custLeft.Children.Add(new TextBlock { Text = email, Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88)), FontSize = 11, FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, 2, 0, 0) });
            custLeft.Children.Add(new TextBlock { Text = phone, Foreground = new SolidColorBrush(Color.FromRgb(0x88, 0x88, 0x88)), FontSize = 11, FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, 2, 0, 0) });
            Grid.SetColumn(custLeft, 0);

            var custRight = new StackPanel { HorizontalAlignment = HorizontalAlignment.Right };
            custRight.Children.Add(new TextBlock { Text = "Delivery Address", Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)), FontSize = 9, FontWeight = FontWeights.SemiBold, FontFamily = new FontFamily("Segoe UI") });
            custRight.Children.Add(new TextBlock { Text = address, Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)), FontSize = 11, FontFamily = new FontFamily("Segoe UI"), Margin = new Thickness(0, 3, 0, 0), TextWrapping = TextWrapping.Wrap, TextAlignment = TextAlignment.Right, MaxWidth = 200 });
            Grid.SetColumn(custRight, 1);

            custGrid.Children.Add(custLeft);
            custGrid.Children.Add(custRight);
            custCard.Child = custGrid;
            body.Children.Add(custCard);

            // ── Order items ──────────────────────────────────────────
            body.Children.Add(MakeSectionLabel("ORDER ITEMS"));
            var itemsCard = new Border
            {
                Background = new SolidColorBrush(Color.FromRgb(0x2C, 0x2C, 0x2C)),
                CornerRadius = new CornerRadius(5),
                Padding = new Thickness(14, 10, 14, 10),
                Margin = new Thickness(0, 6, 0, 16),
                BorderThickness = new Thickness(1),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
            };
            var itemsStack = new StackPanel();

            // Placeholder lines — replace with real order.Items enumeration later
            var placeholderItems = new[]
            {
                ("Road Bike Frame",   1, 4500m),
                ("Gear Set 21-Speed", 1, 1200m),
                ("Repair Kit",        2, 450m),
            };
            foreach (var (name, q, price) in placeholderItems)
            {
                var itemRow = new Grid { Margin = new Thickness(0, 4, 0, 4) };
                itemRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                itemRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(40) });
                itemRow.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(90) });

                var iName = new TextBlock { Text = name, Foreground = new SolidColorBrush(Color.FromRgb(0xE0, 0xE0, 0xE0)), FontSize = 12, FontFamily = new FontFamily("Segoe UI") };
                var iQty = new TextBlock { Text = "×" + q, Foreground = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77)), FontSize = 12, FontFamily = new FontFamily("Segoe UI"), TextAlignment = TextAlignment.Center, VerticalAlignment = VerticalAlignment.Center };
                var iTot = new TextBlock { Text = string.Format("₱ {0:N2}", price * q), Foreground = new SolidColorBrush(Color.FromRgb(0xF0, 0xF0, 0xF0)), FontSize = 12, FontWeight = FontWeights.SemiBold, FontFamily = new FontFamily("Segoe UI"), TextAlignment = TextAlignment.Right, VerticalAlignment = VerticalAlignment.Center };
                Grid.SetColumn(iName, 0);
                Grid.SetColumn(iQty, 1);
                Grid.SetColumn(iTot, 2);
                itemRow.Children.Add(iName);
                itemRow.Children.Add(iQty);
                itemRow.Children.Add(iTot);
                itemsStack.Children.Add(itemRow);
            }

            // Totals
            itemsStack.Children.Add(new Border { Height = 1, Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)), Margin = new Thickness(0, 8, 0, 8) });
            itemsStack.Children.Add(MakeReceiptTotalRow("Subtotal", string.Format("₱ {0:N2}", subtotal), muted: true));
            itemsStack.Children.Add(MakeReceiptTotalRow("Shipping", string.Format("₱ {0:N2}", shipping), muted: true));
            itemsStack.Children.Add(new Border { Height = 1, Background = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)), Margin = new Thickness(0, 6, 0, 6) });
            itemsStack.Children.Add(MakeReceiptTotalRow("TOTAL", string.Format("₱ {0:N2}", grandTotal), large: true));
            itemsStack.Children.Add(MakeReceiptMeta("Payment Method", method));

            itemsCard.Child = itemsStack;
            body.Children.Add(itemsCard);

            // ── Action buttons ───────────────────────────────────────
            body.Children.Add(MakeSectionLabel("ACTIONS"));
            var actRow = new WrapPanel { Margin = new Thickness(0, 8, 0, 0) };

            var btnApprove = new Button
            {
                Content = "✔  Approve Order",
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Margin = new Thickness(0, 0, 8, 0),
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x4C, 0xAF, 0x50)),
                BorderThickness = new Thickness(1),
                FontSize = 12,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnApprove.Click += (s, ev) => dlg.Close();

            var btnReject = new Button
            {
                Content = "✖  Reject",
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Margin = new Thickness(0, 0, 8, 0),
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0xCC, 0x00, 0x00)),
                BorderThickness = new Thickness(1),
                FontSize = 12,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnReject.Click += (s, ev) => dlg.Close();

            var btnLogistics = new Button
            {
                Content = "🚚  Assign Logistics",
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Margin = new Thickness(0, 0, 8, 0),
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.FromRgb(0xA0, 0xA0, 0xA0)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                BorderThickness = new Thickness(1),
                FontSize = 12,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnLogistics.Click += (s, ev) => dlg.Close();

            var btnClose = new Button
            {
                Content = "Close",
                Height = 34,
                Padding = new Thickness(14, 0, 14, 0),
                Background = Brushes.Transparent,
                Foreground = new SolidColorBrush(Color.FromRgb(0x77, 0x77, 0x77)),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A)),
                BorderThickness = new Thickness(1),
                FontSize = 12,
                FontFamily = new FontFamily("Segoe UI"),
                Cursor = System.Windows.Input.Cursors.Hand,
            };
            btnClose.Click += (s, ev) => dlg.Close();

            actRow.Children.Add(btnApprove);
            actRow.Children.Add(btnReject);
            actRow.Children.Add(btnLogistics);
            actRow.Children.Add(btnClose);
            body.Children.Add(actRow);

            root.Children.Add(body);
            outerScroll.Content = root;
            dlg.Content = outerScroll;
            dlg.ShowDialog();
        }

        // ── Order detail helper: section label ───────────────────────
        private static TextBlock MakeSectionLabel(string text)
        {
            return new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(0x66, 0x66, 0x66)),
                FontSize = 10,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
            };
        }

        // ── Order detail helper: coloured status badge ───────────────
        private static Border MakeStatusBadge(string status, bool isPayment)
        {
            Color bg, fg;
            if (isPayment)
            {
                switch (status)
                {
                    case "Paid": bg = Color.FromRgb(0x1A, 0x30, 0x20); fg = Color.FromRgb(0x4C, 0xAF, 0x50); break;
                    case "Failed": bg = Color.FromRgb(0x2C, 0x1A, 0x1A); fg = Color.FromRgb(0xCC, 0x00, 0x00); break;
                    case "Refunded": bg = Color.FromRgb(0x0D, 0x21, 0x37); fg = Color.FromRgb(0x21, 0x96, 0xF3); break;
                    default: bg = Color.FromRgb(0x2C, 0x20, 0x00); fg = Color.FromRgb(0xFF, 0xC1, 0x07); break;
                }
            }
            else
            {
                switch (status)
                {
                    case "Processing": bg = Color.FromRgb(0x0D, 0x21, 0x37); fg = Color.FromRgb(0x21, 0x96, 0xF3); break;
                    case "Shipped": bg = Color.FromRgb(0x22, 0x10, 0x3A); fg = Color.FromRgb(0x9C, 0x27, 0xB0); break;
                    case "Completed": bg = Color.FromRgb(0x1A, 0x30, 0x20); fg = Color.FromRgb(0x4C, 0xAF, 0x50); break;
                    case "Cancelled": bg = Color.FromRgb(0x2C, 0x1A, 0x1A); fg = Color.FromRgb(0xCC, 0x00, 0x00); break;
                    default: bg = Color.FromRgb(0x33, 0x33, 0x33); fg = Color.FromRgb(0xA0, 0xA0, 0xA0); break;
                }
            }

            var badge = new Border
            {
                Background = new SolidColorBrush(bg),
                BorderBrush = new SolidColorBrush(fg),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10, 3, 10, 3),
                Margin = new Thickness(0, 0, 0, 4),
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            badge.Child = new TextBlock
            {
                Text = status,
                Foreground = new SolidColorBrush(fg),
                FontSize = 11,
                FontWeight = FontWeights.SemiBold,
                FontFamily = new FontFamily("Segoe UI"),
            };
            return badge;
        }


        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (_closeConfirmed) return;
            var result = MessageBox.Show(
                "Are you sure you want to exit Taurus Bike Shop?",
                "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) e.Cancel = true;
        }

        // ════════════════════════════════════════════════════════════
        //  LIVE CLOCK
        // ════════════════════════════════════════════════════════════
        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentDateTimeTextBlock.Text = DateTime.Now.ToString("ddd, dd MMM yyyy — hh:mm:ss tt");
        }

        // ════════════════════════════════════════════════════════════
        //  EXIT & LOGOUT
        // ════════════════════════════════════════════════════════════
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to exit Taurus Bike Shop?",
                "Confirm Exit", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { _closeConfirmed = true; Application.Current.Shutdown(); }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?",
                "Confirm Logout", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes) { _closeConfirmed = true; Close(); }
        }

        // ════════════════════════════════════════════════════════════
        //  TOP-BAR REFRESH
        // ════════════════════════════════════════════════════════════
        private void RefreshTopBarButton_Click(object sender, RoutedEventArgs e) { }

        // ════════════════════════════════════════════════════════════
        //  MASTER NAVIGATION
        // ════════════════════════════════════════════════════════════
        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn == null) return;
            NavigateTo(btn.Tag != null ? btn.Tag.ToString() : "Dashboard", btn);
        }

        private void NavigateTo(string tag, Button sourceButton)
        {
            string pageName = TagToPageName(tag);
            FrameworkElement targetPage = FindName(pageName) as FrameworkElement;
            if (targetPage == null) return;

            if (_activePage != null && !ReferenceEquals(_activePage, targetPage))
                _activePage.Visibility = Visibility.Collapsed;

            targetPage.Visibility = Visibility.Visible;
            _activePage = targetPage;

            string title = TagToTitle(tag);
            PageTitleTextBlock.Text = title;
            PageBreadcrumbTextBlock.Text = "Admin \u2192 " + title;

            Button sidebar = TagToSidebarButton(tag);
            if (sidebar != null)
            {
                if (_activeNavBtn != null && !ReferenceEquals(_activeNavBtn, sidebar))
                    _activeNavBtn.Style = (Style)FindResource("NavButtonStyle");
                sidebar.Style = (Style)FindResource("NavButtonActiveStyle");
                _activeNavBtn = sidebar;
            }
        }

        private string TagToPageName(string tag)
        {
            switch (tag)
            {
                case "Dashboard": return "PageDashboard";
                case "PendingOrders": return "PagePendingOrders";
                case "PaymentVerification": return "PagePaymentVerification";
                case "Logistics": return "PageLogistics";
                case "WalkInPOS": return "PageWalkInPOS";
                case "Inventory": return "PageInventory";
                case "OrderHistory": return "PageOrderHistory";
                case "Analytics": return "PageAnalytics";
                case "Support": return "PageSupport";
                default: return "PageDashboard";
            }
        }

        private string TagToTitle(string tag)
        {
            switch (tag)
            {
                case "Dashboard": return "Admin Dashboard";
                case "PendingOrders": return "Pending Orders";
                case "PaymentVerification": return "Payment Verification";
                case "Logistics": return "Logistics Management";
                case "WalkInPOS": return "Walk-In POS";
                case "Inventory": return "Inventory Management";
                case "OrderHistory": return "Order History";
                case "Analytics": return "Analytics";
                case "Support": return "Support Tickets";
                default: return tag;
            }
        }

        private Button TagToSidebarButton(string tag)
        {
            switch (tag)
            {
                case "Dashboard": return DashboardNavButton;
                case "PendingOrders": return PendingOrdersNavButton;
                case "PaymentVerification": return PaymentVerificationNavButton;
                case "Logistics": return LogisticsNavButton;
                case "WalkInPOS": return WalkInPosNavButton;
                case "Inventory": return InventoryNavButton;
                case "OrderHistory": return OrderHistoryNavButton;
                case "Analytics": return AnalyticsNavButton;
                case "Support": return SupportTicketsNavButton;
                default: return null;
            }
        }

        // ════════════════════════════════════════════════════════════
        //  ANALYTICS — PERIOD SWITCHER
        // ════════════════════════════════════════════════════════════
        private void AnalyticsPeriod_Click(object sender, RoutedEventArgs e)
        {
            Button clicked = sender as Button;
            if (clicked == null || ReferenceEquals(clicked, _activeAnalyticsPeriodBtn)) return;

            if (_activeAnalyticsPeriodBtn != null)
                _activeAnalyticsPeriodBtn.Style = (Style)FindResource("GhostButtonStyle");
            clicked.Style = (Style)FindResource("PrimaryButtonStyle");
            _activeAnalyticsPeriodBtn = clicked;

            string tag = clicked.Tag != null ? clicked.Tag.ToString() : "";
            switch (tag)
            {
                case "Today": ApplyAnalyticsData(TodayData); break;
                case "Week": ApplyAnalyticsData(WeekData); break;
                case "Month": ApplyAnalyticsData(MonthData); break;
                case "Year": ApplyAnalyticsData(YearData); break;
            }
        }

        private void ApplyAnalyticsData(AnalyticsPeriodData d)
        {
            Analytics_ActivePeriodLabel.Text = d.PeriodLabel;
            Analytics_DateRangeLabel.Text = d.DateRange;
            Analytics_ChartPeriodLabel.Text = d.PeriodLabel;
            Analytics_ChartSubHint.Text = d.ChartSubHint;
            Analytics_Revenue.Text = d.Revenue;
            Analytics_RevenueTrend.Text = d.RevenueTrend;
            Analytics_Orders.Text = d.Orders;
            Analytics_OrdersTrend.Text = d.OrdersTrend;
            Analytics_AvgOrder.Text = d.AvgOrder;
            Analytics_AvgOrderTrend.Text = d.AvgOrderTrend;
            Analytics_AvgOrderTrend.Foreground = d.AvgOrderDown
                ? (SolidColorBrush)FindResource("AccentRed")
                : (SolidColorBrush)FindResource("StatusGreen");
            Analytics_Customers.Text = d.Customers;
            Analytics_CustomersTrend.Text = d.CustomersTrend;
            Analytics_WalkIn.Text = d.WalkIn;
            Analytics_WalkInSub.Text = d.WalkInSub;
            Analytics_Online.Text = d.Online;
            Analytics_OnlineSub.Text = d.OnlineSub;
            Analytics_Refunds.Text = d.Refunds;
            Analytics_RefundsSub.Text = d.RefundsSub;
            Analytics_Cancelled.Text = d.Cancelled;
            Analytics_CancelledSub.Text = d.CancelledSub;
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — DataGrid SelectionChanged
        // ════════════════════════════════════════════════════════════
        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int count = DgOrders.SelectedItems.Count;
            PO_BulkBar.Visibility = count > 0 ? Visibility.Visible : Visibility.Collapsed;
            PO_SelectedCountLabel.Text = count == 1 ? "1 order selected" : count + " orders selected";
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — Filters
        // ════════════════════════════════════════════════════════════
        private void PO_ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            PO_SearchBox.Text = string.Empty;
            PO_PaymentFilter.SelectedIndex = 0;
            PO_OrderStatusFilter.SelectedIndex = 0;
            PO_DateFrom.SelectedDate = null;
            PO_DateTo.SelectedDate = null;
            PO_HighValueToggle.IsChecked = false;
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — Bulk actions
        // ════════════════════════════════════════════════════════════
        private void PO_BulkApprove_Click(object sender, RoutedEventArgs e)
        {
            int count = DgOrders.SelectedItems.Count;
            if (count == 0) return;
            if (MessageBox.Show(string.Format("Approve {0} selected order(s)?", count),
                    "Confirm Bulk Approve", MessageBoxButton.YesNo, MessageBoxImage.Question)
                == MessageBoxResult.Yes) DgOrders.UnselectAll();
        }

        private void PO_BulkReject_Click(object sender, RoutedEventArgs e)
        {
            int count = DgOrders.SelectedItems.Count;
            if (count == 0) return;
            if (MessageBox.Show(string.Format("Reject {0} selected order(s)?", count),
                    "Confirm Bulk Reject", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes) DgOrders.UnselectAll();
        }

        private void PO_BulkLogistics_Click(object sender, RoutedEventArgs e) { }
        private void PO_BulkExport_Click(object sender, RoutedEventArgs e) { }

        private void PO_ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            DgOrders.UnselectAll();
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — Pagination
        // ════════════════════════════════════════════════════════════
        private void PO_PrevPage_Click(object sender, RoutedEventArgs e) { }
        private void PO_NextPage_Click(object sender, RoutedEventArgs e) { }
    }
}