using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService  _orderService;
        private readonly IDialogService _dialog;

        // ── Pagination ─────────────────────────────────────────────────────────

        private List<SelectableOrder> _allOrders = new();
        private const int DefaultPageSize = 50;

        private int _currentPage = 1;
        public  int CurrentPage
        {
            get => _currentPage;
            private set => SetProperty(ref _currentPage, value);
        }

        private int _totalPages = 1;
        public  int TotalPages
        {
            get => _totalPages;
            private set => SetProperty(ref _totalPages, value);
        }

        public string PageInfo => $"Page {CurrentPage} of {TotalPages}";

        public bool CanGoNext     => CurrentPage < TotalPages;
        public bool CanGoPrevious => CurrentPage > 1;

        // ── Order list ────────────────────────────────────────────────────────

        private ObservableCollection<SelectableOrder> _orders = new();
        public  ObservableCollection<SelectableOrder> Orders
        {
            get => _orders;
            private set
            {
                UnwireSelectionTracking(_orders);
                SetProperty(ref _orders, value);
                WireSelectionTracking(value);
                RefreshSelectionState();
            }
        }

        // ── Status badges ─────────────────────────────────────────────────────

        private List<StatusBadge> _statusBadges = new();
        public  List<StatusBadge> StatusBadges
        {
            get => _statusBadges;
            private set => SetProperty(ref _statusBadges, value);
        }

        // ── Status filter ─────────────────────────────────────────────────────

        /// <summary>All available status filter values, including "All".</summary>
        public IReadOnlyList<string> StatusFilters { get; } = new List<string>
        {
            "All",
            OrderStatuses.Pending,
            OrderStatuses.PaymentVerification,
            OrderStatuses.Processing,
            OrderStatuses.OnHold,
            OrderStatuses.ReadyForPickup,
            OrderStatuses.PickedUp,
            OrderStatuses.OutForDelivery,
            OrderStatuses.Delivered,
            OrderStatuses.Cancelled,
        };

        private string _selectedStatusFilter = "All";
        public  string SelectedStatusFilter
        {
            get => _selectedStatusFilter;
            private set
            {
                if (SetProperty(ref _selectedStatusFilter, value))
                    _ = LoadAsync();
            }
        }

        // ── Order type filter (Delivery / Pickup / All) ───────────────────────

        /// <summary>All available order-type filter values.</summary>
        public IReadOnlyList<string> TypeFilters { get; } = new List<string>
        {
            "All", OrderTypes.Delivery, OrderTypes.Pickup
        };

        private string _selectedTypeFilter = "All";
        public  string SelectedTypeFilter
        {
            get => _selectedTypeFilter;
            private set
            {
                if (SetProperty(ref _selectedTypeFilter, value))
                    _ = LoadAsync();
            }
        }

        // ── Selected order (row click → detail panel) ─────────────────────────

        private SelectableOrder? _selectedRow;

        /// <summary>
        /// The DataGrid's currently-highlighted row.
        /// Setting this triggers detail loading and shows the right-hand panel.
        /// </summary>
        public SelectableOrder? SelectedRow
        {
            get => _selectedRow;
            set
            {
                if (SetProperty(ref _selectedRow, value))
                    _ = OnOrderSelectedAsync(value?.Order);
            }
        }

        private Order? _selectedOrder;

        /// <summary>
        /// The fully-loaded <see cref="Order"/> shown in the detail panel.
        /// Cycled through null after each async load so WPF re-evaluates all
        /// child bindings (Items, Delivery, Pickup) on the new object reference.
        /// </summary>
        public Order? SelectedOrder
        {
            get => _selectedOrder;
            private set => SetProperty(ref _selectedOrder, value);
        }

        private bool _isDetailVisible;
        public  bool IsDetailVisible
        {
            get => _isDetailVisible;
            private set => SetProperty(ref _isDetailVisible, value);
        }

        // ── Bulk selection ────────────────────────────────────────────────────

        private bool _suppressSelectionRefresh;

        /// <summary>Number of rows currently checked for bulk operations.</summary>
        public int  SelectionCount => Orders.Count(o => o.IsSelected);

        /// <summary>True when at least one row is checked.</summary>
        public bool HasSelection   => SelectionCount > 0;

        /// <summary>
        /// True when every row is checked; False otherwise.
        /// Setting this property checks or unchecks all rows at once.
        /// </summary>
        public bool IsAllSelected
        {
            get => Orders.Count > 0 && Orders.All(o => o.IsSelected);
            set
            {
                _suppressSelectionRefresh = true;
                foreach (var o in Orders)
                    o.IsSelected = value;
                _suppressSelectionRefresh = false;
                RefreshSelectionState();
            }
        }

        // ── Single-order action availability ─────────────────────────────────
        //    Only the immediate next valid status is enabled.
        //    Terminal states disable all actions.

        public bool CanMarkReadyForPickup =>
            SelectedOrder is { DeliveryType: OrderTypes.Pickup } so
            && !OrderStatuses.TerminalStatuses.Contains(so.OrderStatus)
            && OrderStatuses.IsValidTransition(so.OrderStatus, OrderStatuses.ReadyForPickup);

        public bool CanConfirmPickup =>
            SelectedOrder is { DeliveryType: OrderTypes.Pickup } so
            && OrderStatuses.IsValidTransition(so.OrderStatus, OrderStatuses.PickedUp);

        public bool CanMarkOutForDelivery =>
            SelectedOrder is { DeliveryType: OrderTypes.Delivery } so
            && !OrderStatuses.TerminalStatuses.Contains(so.OrderStatus)
            && OrderStatuses.IsValidTransition(so.OrderStatus, OrderStatuses.OutForDelivery);

        public bool CanMarkDelivered =>
            SelectedOrder is { DeliveryType: OrderTypes.Delivery } so
            && OrderStatuses.IsValidTransition(so.OrderStatus, OrderStatuses.Delivered);

        public bool CanCancelOrder =>
            SelectedOrder != null
            && !OrderStatuses.TerminalStatuses.Contains(SelectedOrder.OrderStatus)
            && OrderStatuses.IsValidTransition(SelectedOrder.OrderStatus, OrderStatuses.Cancelled);

        public bool CanApprovePayment =>
            SelectedOrder?.OrderStatus == OrderStatuses.PaymentVerification
            && !string.IsNullOrEmpty(SelectedOrder.PaymentProofUrl);

        public bool CanHoldPayment =>
            SelectedOrder?.OrderStatus == OrderStatuses.PaymentVerification
            && !string.IsNullOrEmpty(SelectedOrder.PaymentProofUrl);

        public bool CanMarkProcessing =>
            SelectedOrder != null
            && SelectedOrder.OrderStatus != OrderStatuses.PaymentVerification
            && !OrderStatuses.TerminalStatuses.Contains(SelectedOrder.OrderStatus)
            && OrderStatuses.IsValidTransition(SelectedOrder.OrderStatus, OrderStatuses.Processing);

        // ── Bulk Status Dropdown ──────────────────────────────────────────────
        
        public IEnumerable<string> BulkStatuses
        {
            get
            {
                var selected = Orders.Where(o => o.IsSelected).Select(o => o.Order).ToList();
                if (selected.Count == 0) return Array.Empty<string>();

                var allTargets = new[] 
                {
                    OrderStatuses.Processing,
                    OrderStatuses.OnHold,
                    OrderStatuses.ReadyForPickup,
                    OrderStatuses.PickedUp,
                    OrderStatuses.OutForDelivery,
                    OrderStatuses.Delivered,
                    OrderStatuses.Cancelled
                };

                var list = new List<string>();
                foreach (string status in allTargets)
                {
                    if (selected.All(o => OrderStatuses.IsValidTransition(o.OrderStatus, status, o.DeliveryType)))
                    {
                        list.Add(status);
                    }
                }

                return list;
            }
        }

        private string? _selectedBulkStatus;
        public string? SelectedBulkStatus
        {
            get => _selectedBulkStatus;
            set => SetProperty(ref _selectedBulkStatus, value);
        }

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand RefreshCommand             { get; }
        public ICommand SelectStatusCommand        { get; }
        public ICommand SelectTypeCommand          { get; }
        public ICommand MarkProcessingCommand      { get; }
        public ICommand MarkReadyForPickupCommand  { get; }
        public ICommand ConfirmPickupCommand       { get; }
        public ICommand MarkOutForDeliveryCommand   { get; }  // kept for bulk ops
        public ICommand ShipWithLalamoveCommand     { get; }
        public ICommand ShipWithLBCCommand          { get; }
        public ICommand MarkDeliveredCommand       { get; }
        public ICommand CancelOrderCommand         { get; }
        public ICommand ApprovePaymentCommand      { get; }
        public ICommand HoldPaymentCommand         { get; }
        public ICommand BulkUpdateStatusCommand    { get; }
        public ICommand ApplyBulkUpdateCommand     { get; }
        public ICommand BulkCancelCommand          { get; }
        public ICommand DeselectAllCommand         { get; }
        public ICommand CloseDetailCommand         { get; }
        public ICommand OpenPaymentProofCommand    { get; }
        public ICommand NextPageCommand            { get; }
        public ICommand PreviousPageCommand        { get; }
        public ICommand FirstPageCommand           { get; }
        public ICommand LastPageCommand            { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public OrderViewModel(IOrderService orderService, IDialogService dialog)
        {
            _orderService = orderService;
            _dialog       = dialog;

            RefreshCommand            = new RelayCommand(async () => await LoadAsync());
            SelectStatusCommand       = new RelayCommand<string>(s => SelectedStatusFilter = s ?? "All");
            SelectTypeCommand         = new RelayCommand<string>(t => SelectedTypeFilter = t ?? "All");
            MarkProcessingCommand     = new RelayCommand(async () => await MarkProcessingAsync(),      () => CanMarkProcessing);
            MarkReadyForPickupCommand = new RelayCommand(async () => await MarkReadyForPickupAsync(),  () => CanMarkReadyForPickup);
            ConfirmPickupCommand      = new RelayCommand(async () => await ConfirmPickupAsync(),       () => CanConfirmPickup);
            MarkOutForDeliveryCommand = new RelayCommand(async () => await MarkOutForDeliveryAsync(),  () => CanMarkOutForDelivery);
            ShipWithLalamoveCommand   = new RelayCommand(async () => await ShipOrderAsync("Lalamove"), () => CanMarkOutForDelivery);
            ShipWithLBCCommand        = new RelayCommand(async () => await ShipOrderAsync("LBC"),      () => CanMarkOutForDelivery);
            MarkDeliveredCommand      = new RelayCommand(async () => await MarkDeliveredAsync(),       () => CanMarkDelivered);
            CancelOrderCommand        = new RelayCommand(async () => await CancelOrderAsync(),         () => CanCancelOrder);
            ApprovePaymentCommand     = new RelayCommand(async () => await ApprovePaymentAsync(),      () => CanApprovePayment);
            HoldPaymentCommand        = new RelayCommand(async () => await HoldPaymentAsync(),         () => CanHoldPayment);
            BulkUpdateStatusCommand   = new RelayCommand<string>(async s => await BulkUpdateStatusAsync(s), _ => HasSelection);
            ApplyBulkUpdateCommand    = new RelayCommand(async () => await BulkUpdateStatusAsync(SelectedBulkStatus), () => HasSelection && !string.IsNullOrEmpty(SelectedBulkStatus));
            BulkCancelCommand         = new RelayCommand(async () => await BulkCancelAsync(),          () => HasSelection);
            DeselectAllCommand        = new RelayCommand(() => IsAllSelected = false,                  () => HasSelection);
            CloseDetailCommand        = new RelayCommand(() =>
            {
                SelectedOrder    = null;
                _selectedRow     = null;
                OnPropertyChanged(nameof(SelectedRow));
                IsDetailVisible  = false;
            });

            OpenPaymentProofCommand   = new RelayCommand<string>(url => 
            {
                if (!string.IsNullOrWhiteSpace(url))
                {
                    try
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = url,
                            UseShellExecute = true
                        });
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[Orders] Open proof failed: {ex}");
                    }
                }
            });

            NextPageCommand     = new RelayCommand(() => { CurrentPage++; ApplyPagination(); },     () => CanGoNext);
            PreviousPageCommand = new RelayCommand(() => { CurrentPage--; ApplyPagination(); },     () => CanGoPrevious);
            FirstPageCommand    = new RelayCommand(() => { CurrentPage = 1; ApplyPagination(); },   () => CanGoPrevious);
            LastPageCommand     = new RelayCommand(() => { CurrentPage = TotalPages; ApplyPagination(); }, () => CanGoNext);
        }

        // ── Load ──────────────────────────────────────────────────────────────

        /// <summary>
        /// Loads the filtered order list and refreshes status-badge counts.
        /// Called on navigation, filter change, and after every status-update action.
        /// </summary>
        public async Task LoadAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                // ★ Auto-cancel expired pending orders before loading the list.
                // This ensures the admin always sees accurate statuses, even if
                // the WebApplication's background job hasn't run.
                try
                {
                    int cancelled = await _orderService.AutoCancelExpiredPendingOrdersAsync();
                    if (cancelled > 0)
                        System.Diagnostics.Debug.WriteLine(
                            $"[Orders] Auto-cancelled {cancelled} expired pending order(s).");
                }
                catch (Exception ex)
                {
                    // Best-effort — don't block the order list from loading
                    System.Diagnostics.Debug.WriteLine($"[Orders] Auto-cancel failed: {ex.Message}");
                }

                string? statusFilter = SelectedStatusFilter == "All" ? null : SelectedStatusFilter;
                string? typeFilter   = SelectedTypeFilter   == "All" ? null : SelectedTypeFilter;

                var orders = await _orderService.GetOrdersAsync(statusFilter, typeFilter);
                var counts = await _orderService.GetStatusCountsAsync();

                int? previousId  = _selectedRow?.Order.OrderId;

                // Store the full master list for pagination
                _allOrders = orders.Select(o => new SelectableOrder(o)).ToList();
                TotalPages = Math.Max(1, (int)Math.Ceiling(_allOrders.Count / (double)DefaultPageSize));
                CurrentPage = 1;
                ApplyPagination();

                BuildStatusBadges(counts);

                // Restore the previously-selected order if it still appears in the new list
                if (previousId.HasValue)
                {
                    var restored = Orders.FirstOrDefault(r => r.Order.OrderId == previousId.Value);
                    if (restored != null)
                    {
                        _selectedRow = restored;
                        OnPropertyChanged(nameof(SelectedRow));
                        await OnOrderSelectedAsync(restored.Order);
                    }
                    else
                    {
                        _selectedRow  = null;
                        SelectedOrder = null;
                        OnPropertyChanged(nameof(SelectedRow));
                        IsDetailVisible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError("Failed to load orders. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Orders] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>Slices the master list to update the visible page of orders.</summary>
        private void ApplyPagination()
        {
            var page = _allOrders
                .Skip((CurrentPage - 1) * DefaultPageSize)
                .Take(DefaultPageSize);
            Orders = new ObservableCollection<SelectableOrder>(page);

            OnPropertyChanged(nameof(PageInfo));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(CanGoPrevious));
        }

        // ── Bulk selection tracking ───────────────────────────────────────────

        private void WireSelectionTracking(ObservableCollection<SelectableOrder> orders)
        {
            orders.CollectionChanged += OnOrdersCollectionChanged;
            foreach (var o in orders)
                o.PropertyChanged += OnOrderSelectionChanged;
        }

        private void UnwireSelectionTracking(ObservableCollection<SelectableOrder> orders)
        {
            orders.CollectionChanged -= OnOrdersCollectionChanged;
            foreach (var o in orders)
                o.PropertyChanged -= OnOrderSelectionChanged;
        }

        private void OnOrdersCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (SelectableOrder o in e.NewItems) o.PropertyChanged += OnOrderSelectionChanged;
            if (e.OldItems != null)
                foreach (SelectableOrder o in e.OldItems) o.PropertyChanged -= OnOrderSelectionChanged;
            RefreshSelectionState();
        }

        private void OnOrderSelectionChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectableOrder.IsSelected) && !_suppressSelectionRefresh)
                RefreshSelectionState();
        }

        private void RefreshSelectionState()
        {
            OnPropertyChanged(nameof(SelectionCount));
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(IsAllSelected));
            OnPropertyChanged(nameof(BulkStatuses));

            if (SelectedBulkStatus != null && !BulkStatuses.Contains(SelectedBulkStatus))
            {
                SelectedBulkStatus = null;
            }
        }

        // ── Status badges ─────────────────────────────────────────────────────

        private void BuildStatusBadges(Dictionary<string, int> counts)
        {
            int total = counts.Values.Sum();
            StatusBadges = new List<StatusBadge>
            {
                // All Orders: Neutral (#9CA3AF)
                Badge("All", "All Orders", total, 0x9C, 0xA3, 0xAF),
                
                // Attention: Soft Orange (#F6AD55)
                Badge(OrderStatuses.Pending, "Pending", counts.GetValueOrDefault(OrderStatuses.Pending), 0xF6, 0xAD, 0x55),
                Badge(OrderStatuses.PaymentVerification, "Payment Verification", counts.GetValueOrDefault(OrderStatuses.PaymentVerification), 0xF6, 0xAD, 0x55),
                Badge(OrderStatuses.OnHold, "On Hold", counts.GetValueOrDefault(OrderStatuses.OnHold), 0xF6, 0xAD, 0x55),
                
                // In Progress: Light Blue (#63B3ED)
                Badge(OrderStatuses.Processing, "Processing", counts.GetValueOrDefault(OrderStatuses.Processing), 0x63, 0xB3, 0xED),
                Badge(OrderStatuses.ReadyForPickup, "Ready for Pickup", counts.GetValueOrDefault(OrderStatuses.ReadyForPickup), 0x63, 0xB3, 0xED),
                Badge(OrderStatuses.OutForDelivery, "Out for Delivery", counts.GetValueOrDefault(OrderStatuses.OutForDelivery), 0x63, 0xB3, 0xED),
                
                // Success: Soft Green (#68D391)
                Badge(OrderStatuses.PickedUp, "Picked Up", counts.GetValueOrDefault(OrderStatuses.PickedUp), 0x68, 0xD3, 0x91),
                Badge(OrderStatuses.Delivered, "Delivered", counts.GetValueOrDefault(OrderStatuses.Delivered), 0x68, 0xD3, 0x91),
                
                // Danger: Soft Red (#FC8181)
                Badge(OrderStatuses.Cancelled, "Cancelled", counts.GetValueOrDefault(OrderStatuses.Cancelled), 0xFC, 0x81, 0x81),
            };
        }

        private static StatusBadge Badge(
            string label, string display, int count,
            byte accR, byte accG, byte accB) => new()
        {
            Label        = label,
            DisplayLabel = display,
            Count        = count,
            AccentColor  = new SolidColorBrush(Color.FromRgb(accR, accG, accB))
        };

        // ── Order detail loading ──────────────────────────────────────────────

        private async Task OnOrderSelectedAsync(Order? order)
        {
            if (order == null)
            {
                SelectedOrder   = null;
                IsDetailVisible = false;
                NotifyActionAvailability();
                return;
            }

            IsLoading = true;
            try
            {
                var detail = await _orderService.GetOrderDetailAsync(order.OrderId);
                if (detail != null)
                {
                    order.Items              = detail.Items;
                    order.Delivery           = detail.Delivery;
                    order.Pickup             = detail.Pickup;
                    order.PaymentProofUrl    = detail.PaymentProofUrl;
                    order.PaymentProofMethod = detail.PaymentProofMethod;
                    order.PaymentStatus      = detail.PaymentStatus;
                    order.Address            = detail.Address;
                }

                // Cycle through null so WPF sees a genuine reference change and
                // re-evaluates every child binding (Items, Delivery, Pickup).
                SelectedOrder   = null;
                SelectedOrder   = order;
                IsDetailVisible = true;
                NotifyActionAvailability();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load order detail.");
                System.Diagnostics.Debug.WriteLine($"[Orders] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Single-order status actions ───────────────────────────────────────

        private async Task MarkProcessingAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.Processing, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} is now Processing.");
        }

        private async Task MarkReadyForPickupAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.MarkReadyForPickupAsync(SelectedOrder.OrderId, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} is ready for pickup.");
        }

        private async Task ConfirmPickupAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.ConfirmPickupAsync(SelectedOrder.OrderId, SelectedOrder.OrderStatus),
                $"Pickup confirmed for order {SelectedOrder.OrderNumber}.");
        }

        private async Task MarkOutForDeliveryAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.OutForDelivery, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} marked as Out for Delivery.");
        }

        private async Task ShipOrderAsync(string courier)
        {
            if (SelectedOrder == null) return;
            string label = courier == "Lalamove" ? "Lalamove" : "LBC Express";
            if (!_dialog.Confirm(
                    $"Ship order {SelectedOrder.OrderNumber} via {label}?\n" +
                    $"A simulated tracking reference will be generated and the customer will be notified.",
                    $"Ship via {label}")) return;
            await ExecuteOrderActionAsync(
                () => _orderService.ShipOrderAsync(SelectedOrder.OrderId, courier, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} shipped via {label}. Customer notified.");
        }

        private async Task MarkDeliveredAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.Delivered, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} marked as Delivered.");
        }

        private async Task ApprovePaymentAsync()
        {
            if (SelectedOrder == null) return;
            if (!_dialog.Confirm(
                    $"Approve payment for order {SelectedOrder.OrderNumber}? This will move the order to Processing.",
                    "Approve Payment")) return;
            await ExecuteOrderActionAsync(
                () => _orderService.ApprovePaymentAsync(SelectedOrder.OrderId, SelectedOrder.OrderStatus),
                $"Payment approved. Order {SelectedOrder.OrderNumber} is now Processing.");
        }

        private async Task HoldPaymentAsync()
        {
            if (SelectedOrder == null) return;
            if (!_dialog.Confirm(
                    $"Place order {SelectedOrder.OrderNumber} on hold? The customer will be notified that a payment-proof discrepancy was found.",
                    "Hold Payment")) return;
            await ExecuteOrderActionAsync(
                () => _orderService.HoldPaymentAsync(SelectedOrder.OrderId, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} placed on hold pending payment-proof resolution.");
        }

        private async Task CancelOrderAsync()
        {
            if (SelectedOrder == null) return;
            if (!_dialog.Confirm(
                    $"Cancel order {SelectedOrder.OrderNumber}? This cannot be undone.",
                    "Cancel Order")) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.Cancelled, SelectedOrder.OrderStatus),
                $"Order {SelectedOrder.OrderNumber} cancelled.");
        }

        // ── Bulk status actions ───────────────────────────────────────────────

        private async Task BulkUpdateStatusAsync(string? status)
        {
            if (string.IsNullOrEmpty(status)) return;
            var targets = Orders.Where(o => o.IsSelected).Select(o => o.Order).ToList();
            if (targets.Count == 0) return;

            // Pre-validate: filter to only orders where the transition is valid including delivery type rules
            var valid   = targets.Where(o => OrderStatuses.IsValidTransition(o.OrderStatus, status, o.DeliveryType)).ToList();
            int skipped = targets.Count - valid.Count;

            if (valid.Count == 0)
            {
                ShowError($"None of the {targets.Count} selected order(s) can transition to '{status}'. " +
                          "Reverting status is not allowed.");
                return;
            }

            string message = skipped > 0
                ? $"Update {valid.Count} order(s) to '{status}'? ({skipped} order(s) will be skipped — invalid transition.)"
                : $"Update {valid.Count} order(s) to '{status}'?";

            if (!_dialog.Confirm(message, "Bulk Update Status")) return;

            await ExecuteBulkActionAsync(
                o => _orderService.UpdateOrderStatusAsync(o.OrderId, status, o.OrderStatus),
                valid,
                skipped > 0
                    ? $"{valid.Count} order(s) updated to '{status}'. {skipped} skipped (invalid transition)."
                    : $"{valid.Count} order(s) updated to '{status}'.");
        }

        private async Task BulkCancelAsync()
        {
            var targets = Orders.Where(o => o.IsSelected).Select(o => o.Order).ToList();
            if (targets.Count == 0) return;

            // Pre-validate: only orders that can be cancelled
            var valid   = targets.Where(o => OrderStatuses.IsValidTransition(o.OrderStatus, OrderStatuses.Cancelled)).ToList();
            int skipped = targets.Count - valid.Count;

            if (valid.Count == 0)
            {
                ShowError($"None of the {targets.Count} selected order(s) can be cancelled. " +
                          "Orders in terminal states cannot be modified.");
                return;
            }

            string message = skipped > 0
                ? $"Cancel {valid.Count} order(s)? This cannot be undone. ({skipped} order(s) will be skipped — already in terminal state.)"
                : $"Cancel {valid.Count} order(s)? This cannot be undone.";

            if (!_dialog.Confirm(message, "Bulk Cancel Orders")) return;

            await ExecuteBulkActionAsync(
                o => _orderService.UpdateOrderStatusAsync(o.OrderId, OrderStatuses.Cancelled, o.OrderStatus),
                valid,
                skipped > 0
                    ? $"{valid.Count} order(s) cancelled. {skipped} skipped (terminal state)."
                    : $"{valid.Count} order(s) cancelled.");
        }

        // ── Shared action helpers ─────────────────────────────────────────────

        private async Task ExecuteOrderActionAsync(Func<Task> action, string successMessage)
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                await action();
                ShowSuccess(successMessage);
                await LoadAsync();
            }
            catch (InvalidStatusTransitionException ex)
            {
                ShowError(ex.Message);
                System.Diagnostics.Debug.WriteLine($"[Orders] Rejected transition: {ex}");
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ExecuteBulkActionAsync(
            Func<Order, Task> action, IList<Order> targets, string successMessage)
        {
            IsLoading = true;
            ClearMessages();
            int failed = 0;
            try
            {
                foreach (var order in targets)
                {
                    try   { await action(order); }
                    catch (InvalidStatusTransitionException)
                    {
                        // Already validated client-side; race condition with another admin
                        failed++;
                    }
                    catch { failed++; }
                }

                if (failed > 0)
                    ShowError($"Action failed for {failed} order(s). Others may have succeeded.");
                else
                    ShowSuccess(successMessage);

                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Bulk action failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NotifyActionAvailability()
        {
            OnPropertyChanged(nameof(CanMarkProcessing));
            OnPropertyChanged(nameof(CanMarkReadyForPickup));
            OnPropertyChanged(nameof(CanConfirmPickup));
            OnPropertyChanged(nameof(CanMarkOutForDelivery));
            OnPropertyChanged(nameof(CanMarkDelivered));
            OnPropertyChanged(nameof(CanCancelOrder));
            OnPropertyChanged(nameof(CanApprovePayment));
            OnPropertyChanged(nameof(CanHoldPayment));
            // ShipWithLalamoveCommand / ShipWithLBCCommand share CanMarkOutForDelivery;
            // CommandManager.RequerySuggested (used by RelayCommand) re-evaluates them automatically.
        }
    }
}
