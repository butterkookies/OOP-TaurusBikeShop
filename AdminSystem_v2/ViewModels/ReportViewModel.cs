using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;

        // ── Active tab ────────────────────────────────────────────────────────

        private string _activeTab = "Sales";   // "Sales" | "Inventory"
        public  string ActiveTab
        {
            get => _activeTab;
            private set => SetProperty(ref _activeTab, value);
        }

        public bool IsSalesTab     => ActiveTab == "Sales";
        public bool IsInventoryTab => ActiveTab == "Inventory";

        // ── Sales date range ──────────────────────────────────────────────────

        private DateTime _fromDate = DateTime.Today.AddDays(-29);
        public  DateTime FromDate
        {
            get => _fromDate;
            set => SetProperty(ref _fromDate, value);
        }

        private DateTime _toDate = DateTime.Today;
        public  DateTime ToDate
        {
            get => _toDate;
            set => SetProperty(ref _toDate, value);
        }

        // ── Sales data ────────────────────────────────────────────────────────

        private SalesSummary? _salesSummary;
        public  SalesSummary? SalesSummary
        {
            get => _salesSummary;
            private set => SetProperty(ref _salesSummary, value);
        }

        private ObservableCollection<DailySales> _dailySales = new();
        public  ObservableCollection<DailySales> DailySales
        {
            get => _dailySales;
            private set => SetProperty(ref _dailySales, value);
        }

        private ObservableCollection<TopProduct> _topProducts = new();
        public  ObservableCollection<TopProduct> TopProducts
        {
            get => _topProducts;
            private set => SetProperty(ref _topProducts, value);
        }

        private bool _salesLoaded;

        // ── Inventory data ────────────────────────────────────────────────────

        private ObservableCollection<InventoryReportItem> _inventoryItems = new();
        public  ObservableCollection<InventoryReportItem> InventoryItems
        {
            get => _inventoryItems;
            private set => SetProperty(ref _inventoryItems, value);
        }

        // ── Inventory summary (computed from the list) ────────────────────────

        private int     _invTotal;
        private int     _invInStock;
        private int     _invLowStock;
        private int     _invOutOfStock;
        private decimal _invTotalValue;

        public int     InvTotal      { get => _invTotal;      private set => SetProperty(ref _invTotal,      value); }
        public int     InvInStock    { get => _invInStock;    private set => SetProperty(ref _invInStock,    value); }
        public int     InvLowStock   { get => _invLowStock;   private set => SetProperty(ref _invLowStock,   value); }
        public int     InvOutOfStock { get => _invOutOfStock; private set => SetProperty(ref _invOutOfStock, value); }
        public decimal InvTotalValue { get => _invTotalValue; private set => SetProperty(ref _invTotalValue, value); }

        private bool _inventoryLoaded;

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand ShowSalesTabCommand     { get; }
        public ICommand ShowInventoryTabCommand { get; }
        public ICommand RunSalesReportCommand   { get; }
        public ICommand RefreshInventoryCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;

            ShowSalesTabCommand     = new RelayCommand(() => SwitchTab("Sales"));
            ShowInventoryTabCommand = new RelayCommand(() => SwitchTab("Inventory"));
            RunSalesReportCommand   = new RelayCommand(async () => await LoadSalesAsync());
            RefreshInventoryCommand = new RelayCommand(async () => await LoadInventoryAsync());
        }

        // ── Called by MainWindowViewModel on navigation ───────────────────────

        public async Task LoadAsync()
        {
            // Load whichever tab is currently active
            if (ActiveTab == "Sales" && !_salesLoaded)
                await LoadSalesAsync();
            else if (ActiveTab == "Inventory" && !_inventoryLoaded)
                await LoadInventoryAsync();
        }

        // ── Tab switching ─────────────────────────────────────────────────────

        private void SwitchTab(string tab)
        {
            ActiveTab = tab;
            OnPropertyChanged(nameof(IsSalesTab));
            OnPropertyChanged(nameof(IsInventoryTab));

            if (tab == "Sales" && !_salesLoaded)
                _ = LoadSalesAsync();
            else if (tab == "Inventory" && !_inventoryLoaded)
                _ = LoadInventoryAsync();
        }

        // ── Data loading ──────────────────────────────────────────────────────

        private async Task LoadSalesAsync()
        {
            if (ToDate < FromDate)
            {
                ShowError("'To' date must be on or after 'From' date.");
                return;
            }

            IsLoading = true;
            ClearMessages();
            try
            {
                // Exclusive upper bound: include the full ToDate day
                DateTime exclusiveTo = ToDate.Date.AddDays(1);

                var summaryTask = _reportService.GetSalesSummaryAsync(FromDate.Date, exclusiveTo);
                var dailyTask   = _reportService.GetDailySalesAsync(FromDate.Date, exclusiveTo);
                var topTask     = _reportService.GetTopProductsAsync(FromDate.Date, exclusiveTo);

                await Task.WhenAll(summaryTask, dailyTask, topTask);

                SalesSummary = summaryTask.Result;
                DailySales   = new ObservableCollection<DailySales>(dailyTask.Result);
                TopProducts  = new ObservableCollection<TopProduct>(topTask.Result);

                _salesLoaded = true;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load sales data. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Reports/Sales] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadInventoryAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                var items = (await _reportService.GetInventoryReportAsync()).ToList();

                InventoryItems = new ObservableCollection<InventoryReportItem>(items);

                InvTotal      = items.Count;
                InvInStock    = items.Count(i => i.StockStatus == StockStatuses.InStock);
                InvLowStock   = items.Count(i => i.StockStatus == StockStatuses.LowStock);
                InvOutOfStock = items.Count(i => i.StockStatus == StockStatuses.OutOfStock);
                InvTotalValue = items.Sum(i => i.StockValue);

                _inventoryLoaded = true;
            }
            catch (Exception ex)
            {
                ShowError("Failed to load inventory data. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Reports/Inventory] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
