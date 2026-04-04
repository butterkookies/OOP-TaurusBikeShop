using System.Windows;
using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;
using AdminSystem_v2.Services;
using AdminSystem_v2.ViewModels;
using AdminSystem_v2.Views;
using MainShell = AdminSystem_v2.Views.MainWindow;

namespace AdminSystem_v2
{
    public partial class App : Application
    {
        public static User? CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Repositories
            IUserRepository      userRepo      = new UserRepository();
            IProductRepository   productRepo   = new ProductRepository();
            IInventoryRepository inventoryRepo = new InventoryRepository();
            IOrderRepository     orderRepo     = new OrderRepository();
            IReportRepository    reportRepo    = new ReportRepository();

            // Services
            IAuthService      authSvc      = new AuthService(userRepo);
            IProductService   productSvc   = new ProductService(productRepo);
            IInventoryService inventorySvc = new InventoryService(inventoryRepo);
            IOrderService     orderSvc     = new OrderService(orderRepo);
            IReportService    reportSvc    = new ReportService(reportRepo);
            IUserService      userSvc      = new UserService(userRepo);

            ShowLogin(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc);
        }

        private void ShowLogin(IAuthService authSvc,
                               IProductService productSvc,
                               IInventoryService inventorySvc,
                               IOrderService orderSvc,
                               IReportService reportSvc,
                               IUserService userSvc)
        {
            var loginVm   = new LoginViewModel(authSvc);
            var loginView = new LoginView(loginVm);

            loginVm.LoginSucceeded += () =>
            {
                loginView.Close();
                ShowMain(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc);
            };

            loginView.Show();
        }

        private void ShowMain(IAuthService authSvc,
                              IProductService productSvc,
                              IInventoryService inventorySvc,
                              IOrderService orderSvc,
                              IReportService reportSvc,
                              IUserService userSvc)
        {
            var dashboardVm = new DashboardViewModel(productSvc, inventorySvc);
            var productVm   = new ProductViewModel(productSvc);
            var orderVm     = new OrderViewModel(orderSvc);
            var reportVm    = new ReportViewModel(reportSvc);
            var staffVm     = new StaffViewModel(userSvc);

            var mainVm   = new MainWindowViewModel(authSvc, dashboardVm, productVm, orderVm, reportVm, staffVm);
            var mainView = new MainShell(mainVm);

            mainVm.SignOutRequested += () =>
            {
                mainView.Close();
                ShowLogin(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc);
            };

            mainView.Show();
        }
    }
}
