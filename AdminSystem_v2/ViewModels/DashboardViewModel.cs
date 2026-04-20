using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class DashboardViewModel : BaseViewModel
    {
        private readonly IProductService   _productService;
        private readonly IInventoryService _inventoryService;
        private readonly IOrderService     _orderService;

        // ── Stat Cards ────────────────────────────────────────────────────

        private int _totalProducts;
        public int TotalProducts
        {
            get => _totalProducts;
            private set => SetProperty(ref _totalProducts, value);
        }

        private int _lowStockCount;
        public int LowStockCount
        {
            get => _lowStockCount;
            private set => SetProperty(ref _lowStockCount, value);
        }

        private int _totalOrdersToday;
        public int TotalOrdersToday
        {
            get => _totalOrdersToday;
            private set => SetProperty(ref _totalOrdersToday, value);
        }

        private int _totalOrdersWeek;
        public int TotalOrdersWeek
        {
            get => _totalOrdersWeek;
            private set => SetProperty(ref _totalOrdersWeek, value);
        }

        // ── Low Stock List ────────────────────────────────────────────────

        public ObservableCollection<LowStockVariant> LowStockItems { get; } = new();

        // ── Orders Bar Chart (last 7 days) ────────────────────────────────

        public ObservableCollection<OrderBarItem> OrderBars { get; } = new();

        // ── Commands ──────────────────────────────────────────────────────

        public ICommand RefreshCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────

        public DashboardViewModel(IProductService   productService,
                                  IInventoryService inventoryService,
                                  IOrderService     orderService)
        {
            _productService   = productService;
            _inventoryService = inventoryService;
            _orderService     = orderService;

            RefreshCommand = new RelayCommand(async () => await LoadAsync(), () => IsNotLoading);
        }

        // ── Data Loading ──────────────────────────────────────────────────

        public async Task LoadAsync()
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            try
            {
                var totalTask    = _productService.GetTotalCountAsync();
                var lowStockTask = _inventoryService.GetLowStockVariantsAsync();
                var ordersTask   = _orderService.GetOrdersAsync();

                await Task.WhenAll(totalTask, lowStockTask, ordersTask);

                TotalProducts = totalTask.Result;

                LowStockItems.Clear();
                foreach (var item in lowStockTask.Result)
                    LowStockItems.Add(item);
                LowStockCount = LowStockItems.Count;

                // ── Build 7-day bar chart ──────────────────────────────────
                var today  = DateTime.Today;
                var orders = ordersTask.Result.ToList();

                var bars = new List<OrderBarItem>();
                for (int i = 6; i >= 0; i--)
                {
                    var day   = today.AddDays(-i);
                    var count = orders.Count(o => o.CreatedAt.Date == day.Date);
                    bars.Add(new OrderBarItem
                    {
                        Label = i == 0 ? "Today" : day.ToString("ddd"),
                        Count = count
                    });
                }

                // Normalise heights (0–1)
                int maxCount = bars.Max(b => b.Count);
                foreach (var bar in bars)
                    bar.HeightRatio = maxCount > 0 ? (double)bar.Count / maxCount : 0;

                OrderBars.Clear();
                foreach (var bar in bars)
                    OrderBars.Add(bar);

                TotalOrdersToday = bars.Last().Count;
                TotalOrdersWeek  = bars.Sum(b => b.Count);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load dashboard data. Check your database connection.";
                System.Diagnostics.Debug.WriteLine($"[Dashboard] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
