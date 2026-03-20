using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IOrderService     _orderService;
        private readonly IPaymentService   _paymentService;
        private readonly IInventoryService _inventoryService;
        private readonly IReportService    _reportService;

        public DashboardViewModel(
            IOrderService     orderService,
            IPaymentService   paymentService,
            IInventoryService inventoryService,
            IReportService    reportService)
        {
            _orderService     = orderService;
            _paymentService   = paymentService;
            _inventoryService = inventoryService;
            _reportService    = reportService;

            PendingPayments = new ObservableCollection<Payment>();
            RecentOrders    = new ObservableCollection<Order>();
            LowStockItems   = new ObservableCollection<InventoryLog>();

            RefreshCommand = new RelayCommand(LoadData);
        }

        // ── Stats ───────────────────────────────────────────────────────
        private int _activeOrderCount;
        public int ActiveOrderCount
        {
            get { return _activeOrderCount; }
            set { SetField(ref _activeOrderCount, value, "ActiveOrderCount"); }
        }

        private int _pendingPaymentCount;
        public int PendingPaymentCount
        {
            get { return _pendingPaymentCount; }
            set { SetField(ref _pendingPaymentCount, value, "PendingPaymentCount"); }
        }

        private int _lowStockCount;
        public int LowStockCount
        {
            get { return _lowStockCount; }
            set { SetField(ref _lowStockCount, value, "LowStockCount"); }
        }

        private decimal _todaySales;
        public decimal TodaySales
        {
            get { return _todaySales; }
            set { SetField(ref _todaySales, value, "TodaySales"); }
        }

        public string TodaySalesDisplay
        {
            get { return string.Format("\u20B1 {0:N2}", _todaySales); }
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<Payment>      PendingPayments { get; }
        public ObservableCollection<Order>        RecentOrders    { get; }
        public ObservableCollection<InventoryLog> LowStockItems   { get; }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand RefreshCommand { get; }

        // ── Load ────────────────────────────────────────────────────────
        public void LoadData()
        {
            IsLoading = true;
            ClearMessages();

            try
            {
                // Stats
                List<Order> active = _orderService.GetActiveOrders().ToList();
                ActiveOrderCount = active.Count;

                PendingPayments.Clear();
                foreach (Payment p in _paymentService.GetPendingVerification())
                    PendingPayments.Add(p);
                PendingPaymentCount = PendingPayments.Count;

                LowStockItems.Clear();
                foreach (InventoryLog l in _inventoryService.GetLowStockVariants())
                    LowStockItems.Add(l);
                LowStockCount = LowStockItems.Count;

                TodaySales = _reportService.GetTotalSalesToday();
                OnPropertyChanged("TodaySalesDisplay");

                // Recent orders (last 10 active)
                RecentOrders.Clear();
                int count = 0;
                foreach (Order o in active)
                {
                    if (count >= 10) break;
                    RecentOrders.Add(o);
                    count++;
                }
            }
            catch (System.Exception ex)
            {
                ShowError("Failed to load dashboard: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
