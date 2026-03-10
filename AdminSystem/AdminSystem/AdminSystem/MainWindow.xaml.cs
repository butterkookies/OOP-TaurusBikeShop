// ═══════════════════════════════════════════════════════════════════════
//  AdminDashboardWindow.xaml.cs
//  Taurus Bike Shop — Admin POS System
//
//  • Navigation controller (page switching via Visibility)
//  • Live clock (DispatcherTimer)
//  • Full PendingOrders page logic
//  • Analytics period switcher (Today / This Week / This Month / This Year)
//  • All module-tile buttons wired
//  • C# 7.3 compatible
// ═══════════════════════════════════════════════════════════════════════
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TaurusBikeShop
{
    public partial class AdminDashboardWindow : Window
    {
        private Button _activeNavBtn;
        private FrameworkElement _activePage;
        private Button _activeAnalyticsPeriodBtn;

        // ════════════════════════════════════════════════════════════
        //  ANALYTICS PLACEHOLDER DATA  — swap for real DB calls later
        // ════════════════════════════════════════════════════════════
        private struct AnalyticsPeriodData
        {
            public string PeriodLabel;      // shown in "Viewing:" label
            public string DateRange;        // e.g. "Mar 4 – Mar 10, 2026"

            // Primary KPIs
            public string Revenue;
            public string RevenueTrend;
            public string Orders;
            public string OrdersTrend;
            public string AvgOrder;
            public string AvgOrderTrend;
            public bool AvgOrderDown;
            public string Customers;
            public string CustomersTrend;

            // Secondary KPIs
            public string WalkIn;
            public string WalkInSub;
            public string Online;
            public string OnlineSub;
            public string Refunds;
            public string RefundsSub;
            public string Cancelled;
            public string CancelledSub;

            // Chart hint
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

            // ── Sidebar navigation ───────────────────────────────────
            DashboardNavButton.Click += NavButton_Click;
            PendingOrdersNavButton.Click += NavButton_Click;
            PaymentVerificationNavButton.Click += NavButton_Click;
            LogisticsNavButton.Click += NavButton_Click;
            WalkInPosNavButton.Click += NavButton_Click;
            InventoryNavButton.Click += NavButton_Click;
            OrderHistoryNavButton.Click += NavButton_Click;
            AnalyticsNavButton.Click += NavButton_Click;
            SupportTicketsNavButton.Click += NavButton_Click;

            // ── Dashboard module tiles ───────────────────────────────
            OnlineOrdersModuleButton.Click += NavButton_Click;
            PaymentVerificationModuleButton.Click += NavButton_Click;
            LogisticsModuleButton.Click += NavButton_Click;
            WalkInPosModuleButton.Click += NavButton_Click;
            InventoryModuleButton.Click += NavButton_Click;
            OrderHistoryModuleButton.Click += NavButton_Click;
            AnalyticsModuleButton.Click += NavButton_Click;
            SupportTicketsModuleButton.Click += NavButton_Click;

            // ── Top-bar ──────────────────────────────────────────────
            ViewAllOrdersButton.Click += NavButton_Click;
            RefreshTopBarButton.Click += RefreshTopBarButton_Click;

            // ── Activity feed quick-nav ──────────────────────────────
            VerifyPaymentItem1Button.Click += NavButton_Click;
            VerifyPaymentItem2Button.Click += NavButton_Click;
            VerifyPaymentItem3Button.Click += NavButton_Click;
            ReviewInventoryButton.Click += NavButton_Click;
            ViewTicketButton.Click += NavButton_Click;

            // ── Logout ───────────────────────────────────────────────
            LogoutButton.Click += LogoutButton_Click;

            // ── Pending Orders ───────────────────────────────────────
            PO_ResetFilters.Click += PO_ResetFilters_Click;
            PO_BulkApprove.Click += PO_BulkApprove_Click;
            PO_BulkReject.Click += PO_BulkReject_Click;
            PO_BulkLogistics.Click += PO_BulkLogistics_Click;
            PO_BulkExport.Click += PO_BulkExport_Click;
            PO_ClearSelection.Click += PO_ClearSelection_Click;
            PO_PrevPage.Click += PO_PrevPage_Click;
            PO_NextPage.Click += PO_NextPage_Click;

            // ── Analytics period buttons ─────────────────────────────
            Analytics_BtnToday.Click += AnalyticsPeriod_Click;
            Analytics_BtnWeek.Click += AnalyticsPeriod_Click;
            Analytics_BtnMonth.Click += AnalyticsPeriod_Click;
            Analytics_BtnYear.Click += AnalyticsPeriod_Click;

            // ── Live clock ───────────────────────────────────────────
            var timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            // ── Initial state ────────────────────────────────────────
            _activeNavBtn = DashboardNavButton;
            _activePage = PageDashboard;
            _activeAnalyticsPeriodBtn = Analytics_BtnMonth;
            ApplyAnalyticsData(MonthData);
        }

        // ════════════════════════════════════════════════════════════
        //  LIVE CLOCK
        // ════════════════════════════════════════════════════════════
        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentDateTimeTextBlock.Text =
                DateTime.Now.ToString("ddd, dd MMM yyyy — hh:mm:ss tt");
        }

        // ════════════════════════════════════════════════════════════
        //  LOGOUT
        // ════════════════════════════════════════════════════════════
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to log out?", "Confirm Logout",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                Close();
        }

        // ════════════════════════════════════════════════════════════
        //  TOP-BAR REFRESH
        // ════════════════════════════════════════════════════════════
        private void RefreshTopBarButton_Click(object sender, RoutedEventArgs e)
        {
            // TODO: trigger VM refresh for active page
        }

        // ════════════════════════════════════════════════════════════
        //  MASTER NAVIGATION HANDLER
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
            if (clicked == null || ReferenceEquals(clicked, _activeAnalyticsPeriodBtn))
                return;

            // Swap button styles
            if (_activeAnalyticsPeriodBtn != null)
                _activeAnalyticsPeriodBtn.Style = (Style)FindResource("GhostButtonStyle");
            clicked.Style = (Style)FindResource("PrimaryButtonStyle");
            _activeAnalyticsPeriodBtn = clicked;

            // Load matching data
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
            // Period labels
            Analytics_ActivePeriodLabel.Text = d.PeriodLabel;
            Analytics_DateRangeLabel.Text = d.DateRange;
            Analytics_ChartPeriodLabel.Text = d.PeriodLabel;
            Analytics_ChartSubHint.Text = d.ChartSubHint;

            // Primary KPI row
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

            // Secondary KPI row
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
            PO_SelectedCountLabel.Text = count == 1
                ? "1 order selected"
                : count + " orders selected";
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
                == MessageBoxResult.Yes)
                DgOrders.UnselectAll();
        }

        private void PO_BulkReject_Click(object sender, RoutedEventArgs e)
        {
            int count = DgOrders.SelectedItems.Count;
            if (count == 0) return;
            if (MessageBox.Show(string.Format("Reject {0} selected order(s)?", count),
                    "Confirm Bulk Reject", MessageBoxButton.YesNo, MessageBoxImage.Warning)
                == MessageBoxResult.Yes)
                DgOrders.UnselectAll();
        }

        private void PO_BulkLogistics_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ViewModel.BulkAssignLogisticsCommand
        }

        private void PO_BulkExport_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ViewModel.BulkExportCommand
        }

        private void PO_ClearSelection_Click(object sender, RoutedEventArgs e)
        {
            DgOrders.UnselectAll();
        }

        // ════════════════════════════════════════════════════════════
        //  PENDING ORDERS — Pagination
        // ════════════════════════════════════════════════════════════
        private void PO_PrevPage_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ViewModel.PreviousPageCommand
        }

        private void PO_NextPage_Click(object sender, RoutedEventArgs e)
        {
            // TODO: ViewModel.NextPageCommand
        }

        private void PendingOrdersNavButton_Click(object sender, RoutedEventArgs e)
        {
            // Handled by NavButton_Click wiring above
        }
    }
}
