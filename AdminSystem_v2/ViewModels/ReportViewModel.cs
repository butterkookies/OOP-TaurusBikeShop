using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace AdminSystem_v2.ViewModels
{
    public class ReportViewModel : BaseViewModel
    {
        private readonly IReportService _reportService;

        // ── Active section tab ────────────────────────────────────────────────

        private string _activeTab = "Sales";
        public  string ActiveTab
        {
            get => _activeTab;
            private set => SetProperty(ref _activeTab, value);
        }

        public bool IsSalesTab     => ActiveTab == "Sales";
        public bool IsInventoryTab => ActiveTab == "Inventory";

        // ── Source type ───────────────────────────────────────────────────────

        private string _sourceType = "Online";   // "Online" | "WalkIn" | "Combined"
        public  string SourceType
        {
            get => _sourceType;
            private set
            {
                if (!SetProperty(ref _sourceType, value)) return;
                OnPropertyChanged(nameof(IsOnlineSource));
                OnPropertyChanged(nameof(IsWalkInSource));
                OnPropertyChanged(nameof(IsCombinedSource));
                OnPropertyChanged(nameof(SourceLabel));
                OnPropertyChanged(nameof(OrdersCardLabel));
                _ = LoadSalesAsync();
            }
        }

        public bool IsOnlineSource   => SourceType == "Online";
        public bool IsWalkInSource   => SourceType == "WalkIn";
        public bool IsCombinedSource => SourceType == "Combined";

        public string SourceLabel => SourceType switch
        {
            "WalkIn"   => "Walk-in (POS)",
            "Combined" => "Combined",
            _          => "Online"
        };

        public string OrdersCardLabel => SourceType switch
        {
            "WalkIn"   => "Walk-in (POS) transactions",
            "Combined" => "Online + Walk-in transactions",
            _          => "Online orders (non-cancelled)"
        };

        private bool? WalkInFlag => SourceType switch
        {
            "Online" => false,
            "WalkIn" => true,
            _        => (bool?)null
        };

        // ── Period ────────────────────────────────────────────────────────────

        private string _period = "Daily";   // "Daily" | "Weekly" | "Monthly" | "Yearly" | "Custom"
        public  string Period
        {
            get => _period;
            private set
            {
                if (!SetProperty(ref _period, value)) return;
                OnPropertyChanged(nameof(IsDaily));
                OnPropertyChanged(nameof(IsWeekly));
                OnPropertyChanged(nameof(IsMonthly));
                OnPropertyChanged(nameof(IsYearly));
                OnPropertyChanged(nameof(IsCustomPeriod));
                OnPropertyChanged(nameof(BreakdownLabel));
                if (value != "Custom") ApplyPeriodDates(value);
            }
        }

        public bool IsDaily        => Period == "Daily";
        public bool IsWeekly       => Period == "Weekly";
        public bool IsMonthly      => Period == "Monthly";
        public bool IsYearly       => Period == "Yearly";
        public bool IsCustomPeriod => Period == "Custom";

        public string BreakdownLabel => Period switch
        {
            "Weekly"  => "WEEKLY BREAKDOWN",
            "Monthly" => "MONTHLY BREAKDOWN",
            "Yearly"  => "YEARLY BREAKDOWN",
            _         => "DAILY BREAKDOWN"
        };

        private string GroupBy => Period switch
        {
            "Weekly"  => "Week",
            "Monthly" => "Month",
            "Yearly"  => "Year",
            _         => "Day"
        };

        private void ApplyPeriodDates(string period)
        {
            (FromDate, ToDate) = period switch
            {
                "Weekly"  => (DateTime.Today.AddDays(-7 * 11), DateTime.Today),
                "Monthly" => (new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-11), DateTime.Today),
                "Yearly"  => (new DateTime(DateTime.Today.Year - 4, 1, 1), DateTime.Today),
                _         => (DateTime.Today.AddDays(-13), DateTime.Today)   // Daily
            };
            _ = LoadSalesAsync();
        }

        // ── Date range ────────────────────────────────────────────────────────

        private DateTime _fromDate = DateTime.Today.AddDays(-13);
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

        // ── Chart ─────────────────────────────────────────────────────────────

        private PlotModel _chartModel = new();
        public  PlotModel ChartModel
        {
            get => _chartModel;
            private set => SetProperty(ref _chartModel, value);
        }

        // ── Inventory data ────────────────────────────────────────────────────

        private ObservableCollection<InventoryReportItem> _inventoryItems = new();
        public  ObservableCollection<InventoryReportItem> InventoryItems
        {
            get => _inventoryItems;
            private set => SetProperty(ref _inventoryItems, value);
        }

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

        public ICommand SetSourceOnlineCommand   { get; }
        public ICommand SetSourceWalkInCommand   { get; }
        public ICommand SetSourceCombinedCommand { get; }

        public ICommand SetPeriodDailyCommand   { get; }
        public ICommand SetPeriodWeeklyCommand  { get; }
        public ICommand SetPeriodMonthlyCommand { get; }
        public ICommand SetPeriodYearlyCommand  { get; }
        public ICommand SetPeriodCustomCommand  { get; }

        public ICommand RunSalesReportCommand   { get; }
        public ICommand RefreshInventoryCommand { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public ReportViewModel(IReportService reportService)
        {
            _reportService = reportService;

            ShowSalesTabCommand     = new RelayCommand(() => SwitchTab("Sales"));
            ShowInventoryTabCommand = new RelayCommand(() => SwitchTab("Inventory"));

            SetSourceOnlineCommand   = new RelayCommand(() => SourceType = "Online");
            SetSourceWalkInCommand   = new RelayCommand(() => SourceType = "WalkIn");
            SetSourceCombinedCommand = new RelayCommand(() => SourceType = "Combined");

            SetPeriodDailyCommand   = new RelayCommand(() => Period = "Daily");
            SetPeriodWeeklyCommand  = new RelayCommand(() => Period = "Weekly");
            SetPeriodMonthlyCommand = new RelayCommand(() => Period = "Monthly");
            SetPeriodYearlyCommand  = new RelayCommand(() => Period = "Yearly");
            SetPeriodCustomCommand  = new RelayCommand(() => { Period = "Custom"; });

            RunSalesReportCommand   = new RelayCommand(async () => await LoadSalesAsync());
            RefreshInventoryCommand = new RelayCommand(async () => await LoadInventoryAsync());
        }

        // ── Navigation entry point ────────────────────────────────────────────

        public async Task LoadAsync()
        {
            if (ActiveTab == "Sales")
                await LoadSalesAsync();
            else
                await LoadInventoryAsync();
        }

        // ── Tab switching ─────────────────────────────────────────────────────

        private void SwitchTab(string tab)
        {
            ActiveTab = tab;
            OnPropertyChanged(nameof(IsSalesTab));
            OnPropertyChanged(nameof(IsInventoryTab));

            if (tab == "Inventory" && !_inventoryLoaded)
                _ = LoadInventoryAsync();
        }

        // ── Sales loading ─────────────────────────────────────────────────────

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
                DateTime exclusiveTo = ToDate.Date.AddDays(1);
                bool? walkIn = WalkInFlag;
                string groupBy = GroupBy;

                // Run all queries in parallel
                var summaryTask = _reportService.GetSalesSummaryAsync(FromDate.Date, exclusiveTo, walkIn);
                var dailyTask   = _reportService.GetDailySalesAsync(FromDate.Date, exclusiveTo, walkIn);
                var topTask     = _reportService.GetTopProductsAsync(FromDate.Date, exclusiveTo, 10, walkIn);
                var chartOnlineTask = _reportService.GetChartDataAsync(FromDate.Date, exclusiveTo, groupBy, false);
                var chartWalkInTask = _reportService.GetChartDataAsync(FromDate.Date, exclusiveTo, groupBy, true);

                await Task.WhenAll(summaryTask, dailyTask, topTask, chartOnlineTask, chartWalkInTask);

                SalesSummary = summaryTask.Result;
                DailySales   = new ObservableCollection<DailySales>(dailyTask.Result);
                TopProducts  = new ObservableCollection<TopProduct>(topTask.Result);

                ChartModel = BuildChartModel(
                    chartOnlineTask.Result.ToList(),
                    chartWalkInTask.Result.ToList(),
                    walkIn,
                    groupBy);
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

        // ── Inventory loading ─────────────────────────────────────────────────

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

        // ── Chart builder ─────────────────────────────────────────────────────

        private PlotModel BuildChartModel(
            List<DailySales> onlineData,
            List<DailySales> walkInData,
            bool? sourceFilter,
            string groupBy)
        {
            var bgColor     = OxyColor.Parse("#1F2937");
            var gridColor   = OxyColor.Parse("#374151");
            var textColor   = OxyColor.Parse("#9CA3AF");
            var onlineColor = OxyColor.Parse("#60A5FA");   // blue
            var walkInColor = OxyColor.Parse("#34D399");   // green

            var model = new PlotModel
            {
                Background          = bgColor,
                PlotAreaBackground  = bgColor,
                TextColor           = textColor,
                PlotAreaBorderColor = gridColor,
            };

            model.Legends.Add(new OxyPlot.Legends.Legend
            {
                LegendPosition        = OxyPlot.Legends.LegendPosition.TopRight,
                LegendPlacement       = OxyPlot.Legends.LegendPlacement.Inside,
                LegendBackground      = OxyColor.Parse("#111827"),
                LegendTextColor       = textColor,
                LegendBorderThickness = 0,
            });

            var dateFormat = groupBy switch
            {
                "Month" => "MMM yy",
                "Year"  => "yyyy",
                _       => "MMM d"
            };

            var xAxis = new DateTimeAxis
            {
                Position         = AxisPosition.Bottom,
                StringFormat     = dateFormat,
                TextColor        = textColor,
                TicklineColor    = gridColor,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = gridColor,
                MinorGridlineStyle = LineStyle.None,
                AxislineColor    = gridColor,
            };
            model.Axes.Add(xAxis);

            var yAxis = new LinearAxis
            {
                Position         = AxisPosition.Left,
                StringFormat     = "₱{0:N0}",
                TextColor        = textColor,
                TicklineColor    = gridColor,
                MajorGridlineStyle = LineStyle.Solid,
                MajorGridlineColor = gridColor,
                MinorGridlineStyle = LineStyle.None,
                AxislineColor    = gridColor,
                Minimum          = 0,
            };
            model.Axes.Add(yAxis);

            // For "Combined" show two lines; otherwise one line for the chosen source
            if (sourceFilter == null)
            {
                AddLineSeries(model, onlineData, "Online", onlineColor);
                AddLineSeries(model, walkInData, "Walk-in", walkInColor);
            }
            else if (sourceFilter == false)
            {
                AddLineSeries(model, onlineData, "Online", onlineColor);
            }
            else
            {
                AddLineSeries(model, walkInData, "Walk-in", walkInColor);
            }

            return model;
        }

        private static void AddLineSeries(PlotModel model, List<DailySales> data, string title, OxyColor color)
        {
            var series = new LineSeries
            {
                Title           = title,
                Color           = color,
                StrokeThickness = 2,
                MarkerType      = MarkerType.Circle,
                MarkerSize      = 3,
                MarkerFill      = color,
                TrackerFormatString = "{0}\n{1}: {2:MMM d, yyyy}\nRevenue: ₱{4:N2}",
            };

            foreach (var d in data)
                series.Points.Add(new DataPoint(DateTimeAxis.ToDouble(d.SaleDate), (double)d.Revenue));

            model.Series.Add(series);
        }
    }
}
