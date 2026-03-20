using System;
using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;
            TopOrders = new ObservableCollection<Order>();
            LowStock  = new ObservableCollection<Product>();
            Movements = new ObservableCollection<InventoryLog>();

            DateFrom = DateTime.Today.AddDays(-30);
            DateTo   = DateTime.Today;

            LoadCommand = new RelayCommand(Load);
        }

        // ── Date range ──────────────────────────────────────────────────
        private DateTime _dateFrom;
        public DateTime DateFrom
        {
            get { return _dateFrom; }
            set { SetField(ref _dateFrom, value, "DateFrom"); }
        }

        private DateTime _dateTo;
        public DateTime DateTo
        {
            get { return _dateTo; }
            set { SetField(ref _dateTo, value, "DateTo"); }
        }

        // ── Stats ───────────────────────────────────────────────────────
        private decimal _periodSales;
        public decimal PeriodSales
        {
            get { return _periodSales; }
            set
            {
                SetField(ref _periodSales, value, "PeriodSales");
                OnPropertyChanged("PeriodSalesDisplay");
            }
        }
        public string PeriodSalesDisplay
            => string.Format("\u20B1 {0:N2}", _periodSales);

        private decimal _todaySales;
        public decimal TodaySales
        {
            get { return _todaySales; }
            set
            {
                SetField(ref _todaySales, value, "TodaySales");
                OnPropertyChanged("TodaySalesDisplay");
            }
        }
        public string TodaySalesDisplay
            => string.Format("\u20B1 {0:N2}", _todaySales);

        private int _deliveredCount;
        public int DeliveredCount
        {
            get { return _deliveredCount; }
            set { SetField(ref _deliveredCount, value, "DeliveredCount"); }
        }

        private int _cancelledCount;
        public int CancelledCount
        {
            get { return _cancelledCount; }
            set { SetField(ref _cancelledCount, value, "CancelledCount"); }
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<Order>        TopOrders { get; }
        public ObservableCollection<Product>      LowStock  { get; }
        public ObservableCollection<InventoryLog> Movements { get; }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                TodaySales     = _reportService.GetTotalSalesToday();
                PeriodSales    = _reportService.GetTotalSalesForPeriod(DateFrom, DateTo);
                DeliveredCount = _reportService.GetOrderCountByStatus(
                    OrderStatuses.Delivered);
                CancelledCount = _reportService.GetOrderCountByStatus(
                    OrderStatuses.Cancelled);

                TopOrders.Clear();
                foreach (Order o in _reportService.GetTopOrdersByValue(10))
                    TopOrders.Add(o);

                LowStock.Clear();
                foreach (Product p in _reportService.GetLowStockProducts())
                    LowStock.Add(p);

                Movements.Clear();
                foreach (InventoryLog l in
                    _reportService.GetInventoryMovements(DateFrom, DateTo))
                    Movements.Add(l);
            }
            catch (Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }
    }
}
