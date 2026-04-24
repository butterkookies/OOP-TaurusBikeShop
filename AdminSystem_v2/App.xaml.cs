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
            IPOSRepository       posRepo       = new POSRepository();
            IVoucherRepository   voucherRepo  = new VoucherRepository();
            IStorePaymentAccountRepository paymentAcctRepo = new StorePaymentAccountRepository();
            ISupportTicketRepository supportTicketRepo = new SupportTicketRepository();

            // Services

            IAuthService      authSvc      = new AuthService(userRepo);
            IProductService   productSvc   = new ProductService(productRepo);
            IInventoryService inventorySvc = new InventoryService(inventoryRepo);
            IOrderService     orderSvc     = new OrderService(orderRepo);
            IReportService    reportSvc    = new ReportService(reportRepo);
            IUserService      userSvc      = new UserService(userRepo);
            IDialogService    dialogSvc    = new DialogService();
            IPOSService       posSvc       = new POSService(posRepo);
            IVoucherService   voucherSvc   = new VoucherService(voucherRepo);
            IStorePaymentAccountService paymentAcctSvc = new StorePaymentAccountService(paymentAcctRepo);
            ISupportTicketService supportTicketSvc = new SupportTicketService(supportTicketRepo);
            IExcelExportService excelExportSvc = new ExcelExportService();
            IReceiptPrintService receiptPrintSvc = new ReceiptPrintService();

            ShowLogin(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc, dialogSvc, posSvc, voucherSvc, paymentAcctSvc, supportTicketSvc, excelExportSvc, receiptPrintSvc);
        }

        private void ShowLogin(IAuthService authSvc,
                               IProductService productSvc,
                               IInventoryService inventorySvc,
                               IOrderService orderSvc,
                               IReportService reportSvc,
                               IUserService userSvc,
                               IDialogService dialogSvc,
                               IPOSService posSvc,
                               IVoucherService voucherSvc,
                               IStorePaymentAccountService paymentAcctSvc,
                               ISupportTicketService supportTicketSvc,
                               IExcelExportService excelExportSvc,
                               IReceiptPrintService receiptPrintSvc)
        {
            var loginVm   = new LoginViewModel(authSvc, dialogSvc);
            var loginView = new LoginView(loginVm);

            loginVm.LoginSucceeded += () =>
            {
                ShowMain(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc, dialogSvc, posSvc, voucherSvc, paymentAcctSvc, supportTicketSvc, excelExportSvc, receiptPrintSvc);
                loginView.Close();
            };

            loginView.Show();
        }

        private void ShowMain(IAuthService authSvc,
                              IProductService productSvc,
                              IInventoryService inventorySvc,
                              IOrderService orderSvc,
                              IReportService reportSvc,
                              IUserService userSvc,
                              IDialogService dialogSvc,
                              IPOSService posSvc,
                              IVoucherService voucherSvc,
                              IStorePaymentAccountService paymentAcctSvc,
                              ISupportTicketService supportTicketSvc,
                              IExcelExportService excelExportSvc,
                              IReceiptPrintService receiptPrintSvc)
        {
            var dashboardVm = new DashboardViewModel(productSvc, inventorySvc, orderSvc);
            var productVm   = new ProductViewModel(productSvc);
            var orderVm     = new OrderViewModel(orderSvc, dialogSvc);
            var reportVm    = new ReportViewModel(reportSvc, excelExportSvc);
            var staffVm     = new StaffViewModel(userSvc, dialogSvc);
            var posVm       = new POSViewModel(posSvc, receiptPrintSvc);
            var voucherVm   = new VoucherViewModel(voucherSvc, dialogSvc);
            var paymentAcctVm = new StorePaymentAccountViewModel(paymentAcctSvc, dialogSvc);
            var supportTicketsVm = new SupportTicketsViewModel(supportTicketSvc, dialogSvc);

            var mainVm   = new MainWindowViewModel(authSvc, dashboardVm, productVm, orderVm, reportVm, staffVm, posVm, voucherVm, paymentAcctVm, supportTicketsVm, dialogSvc);
            var mainView = new MainShell(mainVm);

            mainVm.SignOutRequested += () =>
            {
                mainView.Close();
                ShowLogin(authSvc, productSvc, inventorySvc, orderSvc, reportSvc, userSvc, dialogSvc, posSvc, voucherSvc, paymentAcctSvc, supportTicketSvc, excelExportSvc, receiptPrintSvc);
            };

            mainView.Show();
        }
    }
}
