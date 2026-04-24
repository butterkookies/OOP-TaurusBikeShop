using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly IAuthService       _authService;
        private readonly IDialogService     _dialog;
        private readonly DashboardViewModel _dashboardVm;
        private readonly ProductViewModel   _productVm;
        private readonly OrderViewModel     _orderVm;
        private readonly ReportViewModel    _reportVm;
        private readonly StaffViewModel     _staffVm;
        private readonly POSViewModel       _posVm;
        private readonly VoucherViewModel   _voucherVm;
        private readonly StorePaymentAccountViewModel _paymentAccountsVm;
        private readonly SupportTicketsViewModel _supportTicketsVm;

        // ── Session ───────────────────────────────────────────────────────

        public string UserFullName    { get; }
        public string UserInitials    { get; }
        public string UserRole        { get; }
        public bool   IsAdmin         => UserRole == RoleNames.Admin;
        public bool   IsAdminOrManager => UserRole == RoleNames.Admin || UserRole == RoleNames.Manager;

        // ── Navigation State ──────────────────────────────────────────────

        private object _currentViewModel = null!;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            private set => SetProperty(ref _currentViewModel, value);
        }

        private string _activePage = string.Empty;
        public string ActivePage
        {
            get => _activePage;
            private set => SetProperty(ref _activePage, value);
        }

        public string PageTitle      => ActivePage;
        public string PageBreadcrumb => $"Admin \u2192 {ActivePage}";

        // ── Commands ──────────────────────────────────────────────────────

        public ICommand NavigateCommand { get; }
        public ICommand SignOutCommand  { get; }
        public ICommand ExitCommand     { get; }

        // ── Events ────────────────────────────────────────────────────────

        public event Action? SignOutRequested;

        // ── Constructor ───────────────────────────────────────────────────

        public MainWindowViewModel(
            IAuthService       authService,
            DashboardViewModel dashboardVm,
            ProductViewModel   productVm,
            OrderViewModel     orderVm,
            ReportViewModel    reportVm,
            StaffViewModel     staffVm,
            POSViewModel       posVm,
            VoucherViewModel   voucherVm,
            StorePaymentAccountViewModel paymentAccountsVm,
            SupportTicketsViewModel supportTicketsVm,
            IDialogService     dialog)
        {
            _authService       = authService;
            _dialog            = dialog;
            _dashboardVm       = dashboardVm;
            _productVm         = productVm;
            _orderVm           = orderVm;
            _reportVm          = reportVm;
            _staffVm           = staffVm;
            _posVm             = posVm;
            _voucherVm         = voucherVm;
            _paymentAccountsVm = paymentAccountsVm;
            _supportTicketsVm  = supportTicketsVm;

            User? user   = App.CurrentUser;
            UserFullName = user?.FullName ?? "Administrator";
            UserInitials = user?.Initials ?? "A";
            UserRole     = user?.Role     ?? "Admin";

            NavigateCommand = new RelayCommand<string>(Navigate);
            SignOutCommand  = new RelayCommand(ExecuteSignOut);
            ExitCommand     = new RelayCommand(ExecuteExit);

            Navigate(PageNames.Dashboard);
        }

        // ── Navigation ────────────────────────────────────────────────────

        private void Navigate(string? page)
        {
            if (string.IsNullOrEmpty(page)) return;

            // Block non-Admin users from restricted pages
            if (page == PageNames.Staff          && !IsAdmin) return;
            if (page == PageNames.PaymentAccounts && !IsAdmin) return;

            CurrentViewModel = page switch
            {
                PageNames.Dashboard       => _dashboardVm,
                PageNames.Products        => _productVm,
                PageNames.Orders          => _orderVm,
                PageNames.Reports         => _reportVm,
                PageNames.Staff           => _staffVm,
                PageNames.POS             => _posVm,
                PageNames.Vouchers        => _voucherVm,
                PageNames.PaymentAccounts => _paymentAccountsVm,
                PageNames.SupportTickets  => _supportTicketsVm,
                _                         => _dashboardVm
            };

            ActivePage = page;
            OnPropertyChanged(nameof(PageTitle));
            OnPropertyChanged(nameof(PageBreadcrumb));

            // Trigger data load when the user navigates to a page
            if (page == PageNames.Dashboard)
                _ = _dashboardVm.LoadAsync();
            else if (page == PageNames.Products)
                _ = _productVm.LoadAsync();
            else if (page == PageNames.Orders)
                _ = _orderVm.LoadAsync();
            else if (page == PageNames.Reports)
                _ = _reportVm.LoadAsync();
            else if (page == PageNames.Staff)
                _ = _staffVm.LoadAsync();
            else if (page == PageNames.POS)
                _ = _posVm.LoadAsync();
            else if (page == PageNames.Vouchers)
                _ = _voucherVm.LoadAsync();
            else if (page == PageNames.PaymentAccounts)
                _ = _paymentAccountsVm.LoadAsync();
            else if (page == PageNames.SupportTickets)
                _ = _supportTicketsVm.LoadTicketsAsync();
        }

        // ── Command Handlers ──────────────────────────────────────────────

        private void ExecuteSignOut()
        {
            if (_dialog.Confirm("Sign out of Taurus Bike Shop Admin?", "Sign Out"))
            {
                _authService.Logout();
                SignOutRequested?.Invoke();
            }
        }

        private void ExecuteExit()
        {
            if (_dialog.Confirm("Exit Taurus Bike Shop Admin?", "Exit"))
            {
                _authService.Logout(); // clears App.CurrentUser so Window_Closing skips its dialog
                System.Windows.Application.Current.Shutdown();
            }
        }
    }
}
