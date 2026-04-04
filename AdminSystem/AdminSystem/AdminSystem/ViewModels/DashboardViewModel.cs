using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
            LowStockItems   = new ObservableCollection<LowStockVariant>();

            RefreshCommand = new AsyncRelayCommand(LoadDataAsync);
        }

        // ── Stats ───────────────────────────────────────────────────────
        private int _activeOrderCount;
        public int ActiveOrderCount
        {
            get { return _activeOrderCount; }
            set { SetField(ref _activeOrderCount, value, nameof(ActiveOrderCount)); }
        }

        private int _pendingPaymentCount;
        public int PendingPaymentCount
        {
            get { return _pendingPaymentCount; }
            set { SetField(ref _pendingPaymentCount, value, nameof(PendingPaymentCount)); }
        }

        private int _lowStockCount;
        public int LowStockCount
        {
            get { return _lowStockCount; }
            set { SetField(ref _lowStockCount, value, nameof(LowStockCount)); }
        }

        private decimal _todaySales;
        public decimal TodaySales
        {
            get { return _todaySales; }
            set { SetField(ref _todaySales, value, nameof(TodaySales)); }
        }

        public string TodaySalesDisplay
        {
            get { return string.Format("\u20B1 {0:N2}", _todaySales); }
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<Payment>        PendingPayments { get; }
        public ObservableCollection<Order>          RecentOrders    { get; }
        public ObservableCollection<LowStockVariant> LowStockItems  { get; }

        // ── Commands ────────────────────────────────────────────────────
        public AsyncRelayCommand RefreshCommand { get; }

        // ── Load ────────────────────────────────────────────────────────
        public async Task LoadDataAsync()
        {
            IsLoading = true;
            ClearMessages();

            try
            {
                // Fetch all data concurrently
                Task<List<Order>>        activeTask   = _orderService.GetActiveOrdersAsync();
                Task<List<Payment>>      pendingTask  = _paymentService.GetPendingVerificationAsync();
                Task<List<LowStockVariant>> lowStockTask = _inventoryService.GetLowStockVariantsAsync();
                Task<decimal>            salesTask    = _reportService.GetTotalSalesTodayAsync();

                await Task.WhenAll(activeTask, pendingTask, lowStockTask, salesTask);

                List<Order>        active   = activeTask.Result;
                List<Payment>      pending  = pendingTask.Result;
                List<LowStockVariant> lowStock = lowStockTask.Result;
                decimal            sales    = salesTask.Result;

                // Stats
                ActiveOrderCount = active.Count;

                PendingPayments.Clear();
                foreach (Payment p in pending) PendingPayments.Add(p);
                PendingPaymentCount = PendingPayments.Count;

                LowStockItems.Clear();
                foreach (LowStockVariant l in lowStock) LowStockItems.Add(l);
                LowStockCount = LowStockItems.Count;

                TodaySales = sales;
                OnPropertyChanged(nameof(TodaySalesDisplay));

                // Recent orders (last 10 active)
                RecentOrders.Clear();
                foreach (Order o in active.Take(10))
                    RecentOrders.Add(o);
            }
            catch (Exception ex)
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
