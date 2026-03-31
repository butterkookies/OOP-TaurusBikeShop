using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;
using AdminSystem.Services;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    public partial class MainWindow : Window
    {
        // ── Repositories (shared) ──────────────────────────────────────────
        private readonly OrderRepository     _orderRepo     = new OrderRepository();
        private readonly PaymentRepository   _paymentRepo   = new PaymentRepository();
        private readonly ProductRepository   _productRepo   = new ProductRepository();
        private readonly InventoryRepository _inventoryRepo = new InventoryRepository();
        private readonly UserRepository      _userRepo      = new UserRepository();

        // ── Services ──────────────────────────────────────────────────────
        private readonly IOrderService     _orderSvc;
        private readonly IPaymentService   _paymentSvc;
        private readonly IProductService   _productSvc;
        private readonly IInventoryService _inventorySvc;
        private readonly IDeliveryService  _deliverySvc;
        private readonly ISupportService   _supportSvc;
        private readonly IReportService    _reportSvc;

        // ── Cached UserControl views (lazy) ───────────────────────────────
        private DashboardView  _dashboardView;
        private OrdersView     _ordersView;
        private POSView        _posView;
        private ProductsView   _productsView;
        private InventoryView  _inventoryView;
        private DeliveryView   _deliveryView;
        private VouchersView   _vouchersView;
        private SupportView    _supportView;
        private UsersView      _usersView;
        private ReportsView    _reportsView;

        // ── Activity feed backing list ─────────────────────────────────────
        private readonly ObservableCollection<PendingPaymentItem> _pendingPayments
            = new ObservableCollection<PendingPaymentItem>();

        public MainWindow()
        {
            InitializeComponent();

            // Build services
            _orderSvc     = new OrderService(_orderRepo);
            _paymentSvc   = new PaymentService(_paymentRepo);
            _productSvc   = new ProductService(_productRepo);
            _inventorySvc = new InventoryService(_inventoryRepo, _productRepo);
            _deliverySvc  = new DeliveryService();
            _supportSvc   = new SupportService();
            _reportSvc    = new ReportService(_inventoryRepo, _productRepo);

            // Top-bar and sidebar user display
            if (App.CurrentUser != null)
            {
                TbCurrentUser.Text         = App.CurrentUser.FullName;
                AdminNameTextBlock.Text    = App.CurrentUser.FullName;
                AdminAvatarTextBlock.Text  = App.CurrentUser.FullName.Length > 0
                    ? App.CurrentUser.FullName[0].ToString().ToUpper()
                    : "A";
                AdminRoleTextBlock.Text    = App.CurrentUser.Role ?? "Administrator";
            }

            // Wire exit and refresh buttons
            ExitButton.Click          += (s, e) => Close();
            RefreshTopBarButton.Click += (s, e) => { LoadActivityFeed(); Navigate(PageNames.Dashboard); };

            // Wire nav buttons
            BtnNavDashboard.Click  += (s, e) => Navigate(PageNames.Dashboard);
            BtnNavOrders.Click     += (s, e) => Navigate(PageNames.Orders);
            BtnNavPOS.Click        += (s, e) => Navigate(PageNames.POS);
            BtnNavInventory.Click  += (s, e) => Navigate(PageNames.Inventory);
            BtnNavDelivery.Click   += (s, e) => Navigate(PageNames.Delivery);
            BtnNavReports.Click    += (s, e) => Navigate(PageNames.Reports);
            BtnNavSupport.Click    += (s, e) => Navigate(PageNames.Support);
            BtnNavProducts.Click   += (s, e) => Navigate(PageNames.Products);
            BtnNavVouchers.Click   += (s, e) => Navigate(PageNames.Vouchers);
            BtnNavUsers.Click      += (s, e) => Navigate(PageNames.Users);

            PaymentVerificationNavButton.Click += (s, e) => Navigate(PageNames.Orders);
            OrderHistoryNavButton.Click        += (s, e) => Navigate(PageNames.Orders);

            // Sidebar buttons that navigate from activity feed
            ReviewInventoryButton.Click += (s, e) => Navigate(PageNames.Inventory);
            ViewTicketButton.Click      += (s, e) => Navigate(PageNames.Support);

            // Wire activity feed list
            ActivityPendingPaymentsList.ItemsSource = _pendingPayments;

            LoadActivityFeed();
            Navigate(PageNames.Dashboard);
        }

        // ── Navigation ────────────────────────────────────────────────────
        public void NavigateTo(string page) => Navigate(page);

        private async void Navigate(string page)
        {
            switch (page)
            {
                case PageNames.Dashboard:
                    if (_dashboardView == null)
                        _dashboardView = new DashboardView(
                            new DashboardViewModel(_orderSvc, _paymentSvc,
                                                   _inventorySvc, _reportSvc));
                    ContentArea.Content = _dashboardView;
                    await _dashboardView.Refresh();
                    PageTitleTextBlock.Text     = "Dashboard";
                    PageBreadcrumbTextBlock.Text = "Admin → Dashboard";
                    break;

                case PageNames.Orders:
                    if (_ordersView == null)
                        _ordersView = new OrdersView(new OrderViewModel(_orderSvc));
                    ContentArea.Content = _ordersView;
                    _ordersView.Refresh();
                    PageTitleTextBlock.Text     = "Orders";
                    PageBreadcrumbTextBlock.Text = "Admin → Orders";
                    break;

                case PageNames.POS:
                    if (_posView == null)
                        _posView = new POSView(new POSViewModel(_productSvc));
                    ContentArea.Content = _posView;
                    _posView.Refresh();
                    PageTitleTextBlock.Text     = "Point of Sale";
                    PageBreadcrumbTextBlock.Text = "Admin → POS";
                    break;

                case PageNames.Products:
                    if (_productsView == null)
                        _productsView = new ProductsView(
                            new ProductViewModel(_productSvc));
                    ContentArea.Content = _productsView;
                    _productsView.Refresh();
                    PageTitleTextBlock.Text     = "Products";
                    PageBreadcrumbTextBlock.Text = "Admin → Products";
                    break;

                case PageNames.Inventory:
                    if (_inventoryView == null)
                        _inventoryView = new InventoryView(
                            new InventoryViewModel(_inventorySvc));
                    ContentArea.Content = _inventoryView;
                    _inventoryView.Refresh();
                    PageTitleTextBlock.Text     = "Inventory";
                    PageBreadcrumbTextBlock.Text = "Admin → Inventory";
                    break;

                case PageNames.Delivery:
                    if (_deliveryView == null)
                        _deliveryView = new DeliveryView(
                            new DeliveryViewModel(_deliverySvc));
                    ContentArea.Content = _deliveryView;
                    _deliveryView.Refresh();
                    PageTitleTextBlock.Text     = "Delivery";
                    PageBreadcrumbTextBlock.Text = "Admin → Delivery";
                    break;

                case PageNames.Vouchers:
                    if (_vouchersView == null)
                        _vouchersView = new VouchersView(new VoucherViewModel());
                    ContentArea.Content = _vouchersView;
                    _vouchersView.Refresh();
                    PageTitleTextBlock.Text     = "Vouchers";
                    PageBreadcrumbTextBlock.Text = "Admin → Vouchers";
                    break;

                case PageNames.Support:
                    if (_supportView == null)
                        _supportView = new SupportView(
                            new SupportViewModel(_supportSvc));
                    ContentArea.Content = _supportView;
                    _supportView.Refresh();
                    PageTitleTextBlock.Text     = "Support";
                    PageBreadcrumbTextBlock.Text = "Admin → Support";
                    break;

                case PageNames.Users:
                    if (_usersView == null)
                        _usersView = new UsersView(
                            new UsersViewModel(_userRepo));
                    ContentArea.Content = _usersView;
                    _usersView.Refresh();
                    PageTitleTextBlock.Text     = "Users";
                    PageBreadcrumbTextBlock.Text = "Admin → Users";
                    break;

                case PageNames.Reports:
                    if (_reportsView == null)
                        _reportsView = new ReportsView(
                            new ReportViewModel(_reportSvc));
                    ContentArea.Content = _reportsView;
                    _reportsView.Refresh();
                    PageTitleTextBlock.Text     = "Reports";
                    PageBreadcrumbTextBlock.Text = "Admin → Reports";
                    break;
            }
        }

        // ── Activity Feed ─────────────────────────────────────────────────
        private void LoadActivityFeed()
        {
            try
            {
                _pendingPayments.Clear();
                foreach (Payment p in _paymentSvc.GetPendingVerification())
                {
                    _pendingPayments.Add(new PendingPaymentItem
                    {
                        PaymentId      = p.PaymentId,
                        OrderRef       = "Order #" + p.OrderId,
                        Method         = p.PaymentMethod,
                        CreatedDisplay = p.CreatedAt.ToString("hh:mm tt"),
                        AmountDisplay  = string.Format("\u20B1 {0:N2}", p.Amount)
                    });
                }

                try
                {
                    PendingOrdersBadgeTextBlock.Text          = _orderSvc.GetActiveOrders().Count().ToString();
                    PaymentVerificationBadgeTextBlock.Text    = _paymentSvc.GetPendingVerification().Count().ToString();
                    SupportTicketsBadgeTextBlock.Text         = _supportSvc.GetTicketsByStatus("Open").Count().ToString();
                }
                catch (Exception badgeEx) { System.Diagnostics.Debug.WriteLine("[Badges] " + badgeEx.Message); }

                int low = 0;
                try { low = _inventorySvc.GetLowStockVariants().Count(); }
                catch (Exception lowEx) { System.Diagnostics.Debug.WriteLine("[LowStock] " + lowEx.Message); }

                ActivityLowStockBadge.Text  = low.ToString();
                ActivityLowStockDetail.Text = low > 0
                    ? low + " variant(s) below reorder threshold."
                    : "All stock levels healthy.";
                ActivityLowStockPanel.Visibility = low > 0
                    ? Visibility.Visible : Visibility.Collapsed;

                bool online = DatabaseHelper.TestConnection();
                ActivityDbStatus.Text = online ? "Database \u2022 Online" : "Database \u2022 Offline";
                ActivityDbDot.Fill = online ? AppColors.Success : AppColors.Accent;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[ActivityFeed] " + ex.Message);
            }
        }

        // ── Verify Payment (activity feed button) ─────────────────────────
        private void BtnVerifyPayment_Click(object sender, RoutedEventArgs e)
        {
            if (!(sender is System.Windows.Controls.Button btn) || btn.Tag == null) return;

            int paymentId;
            if (!int.TryParse(btn.Tag.ToString(), out paymentId)) return;

            Payment p = _paymentSvc.GetPaymentById(paymentId);
            if (p == null) return;

            if (MessageBox.Show(
                    string.Format("Approve payment of \u20B1{0:N2} for Order #{1}?",
                        p.Amount, p.OrderId),
                    "Approve Payment",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                == MessageBoxResult.Yes)
            {
                _paymentSvc.ApprovePayment(paymentId);
                LoadActivityFeed();
                MessageBox.Show("Payment approved.", "Done",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        // ── Sign-out ──────────────────────────────────────────────────────
        private void BtnNavSignOut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Sign out of Taurus Bike Shop Admin?",
                    "Sign Out",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question)
                == MessageBoxResult.Yes)
                NavigationHelper.SignOut(this);
        }

        // ── Window closing ────────────────────────────────────────────────
        private void Window_Closing(object sender,
            System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show(
                    "Exit Taurus Bike Shop Admin?", "Exit",
                    MessageBoxButton.YesNo, MessageBoxImage.Question)
                != MessageBoxResult.Yes)
                e.Cancel = true;
        }
    }

    // ── DTO for activity feed pending-payment items ───────────────────────
    public class PendingPaymentItem
    {
        public int    PaymentId      { get; set; }
        public string OrderRef       { get; set; }
        public string Method         { get; set; }
        public string CreatedDisplay { get; set; }
        public string AmountDisplay  { get; set; }
    }
}
