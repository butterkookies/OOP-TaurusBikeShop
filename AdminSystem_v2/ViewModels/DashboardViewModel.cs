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

        // ── Low Stock List ────────────────────────────────────────────────

        public ObservableCollection<LowStockVariant> LowStockItems { get; } = new();

        // ── Commands ──────────────────────────────────────────────────────

        public ICommand RefreshCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────

        public DashboardViewModel(IProductService productService,
                                  IInventoryService inventoryService)
        {
            _productService   = productService;
            _inventoryService = inventoryService;

            RefreshCommand = new RelayCommand(async () => await LoadAsync(), () => IsNotLoading);
        }

        // ── Data Loading ──────────────────────────────────────────────────

        public async Task LoadAsync()
        {
            IsLoading    = true;
            ErrorMessage = string.Empty;

            try
            {
                // Run both queries at the same time — no reason to wait for one before starting the other
                var totalTask    = _productService.GetTotalCountAsync();
                var lowStockTask = _inventoryService.GetLowStockVariantsAsync();

                await Task.WhenAll(totalTask, lowStockTask);

                TotalProducts = totalTask.Result;

                LowStockItems.Clear();
                foreach (var item in lowStockTask.Result)
                    LowStockItems.Add(item);

                LowStockCount = LowStockItems.Count;
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
