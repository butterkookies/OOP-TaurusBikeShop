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
            OrderStatuses.Processing,
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

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand RefreshCommand             { get; }
        public ICommand SelectStatusCommand        { get; }
        public ICommand SelectTypeCommand          { get; }
        public ICommand MarkReadyForPickupCommand  { get; }
        public ICommand ConfirmPickupCommand       { get; }
        public ICommand MarkOutForDeliveryCommand   { get; }
        public ICommand MarkDeliveredCommand       { get; }
        public ICommand CancelOrderCommand         { get; }
        public ICommand BulkUpdateStatusCommand    { get; }
        public ICommand BulkCancelCommand          { get; }
        public ICommand DeselectAllCommand         { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public OrderViewModel(IOrderService orderService, IDialogService dialog)
        {
            _orderService = orderService;
            _dialog       = dialog;

            RefreshCommand            = new RelayCommand(async () => await LoadAsync());
            SelectStatusCommand       = new RelayCommand<string>(s => SelectedStatusFilter = s ?? "All");
            SelectTypeCommand         = new RelayCommand<string>(t => SelectedTypeFilter = t ?? "All");
            MarkReadyForPickupCommand = new RelayCommand(async () => await MarkReadyForPickupAsync(),  () => CanMarkReadyForPickup);
            ConfirmPickupCommand      = new RelayCommand(async () => await ConfirmPickupAsync(),       () => CanConfirmPickup);
            MarkOutForDeliveryCommand = new RelayCommand(async () => await MarkOutForDeliveryAsync(),  () => CanMarkOutForDelivery);
            MarkDeliveredCommand      = new RelayCommand(async () => await MarkDeliveredAsync(),       () => CanMarkDelivered);
            CancelOrderCommand        = new RelayCommand(async () => await CancelOrderAsync(),         () => CanCancelOrder);
            BulkUpdateStatusCommand   = new RelayCommand<string>(async s => await BulkUpdateStatusAsync(s), _ => HasSelection);
            BulkCancelCommand         = new RelayCommand(async () => await BulkCancelAsync(),          () => HasSelection);
            DeselectAllCommand        = new RelayCommand(() => IsAllSelected = false,                  () => HasSelection);
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
                string? statusFilter = SelectedStatusFilter == "All" ? null : SelectedStatusFilter;
                string? typeFilter   = SelectedTypeFilter   == "All" ? null : SelectedTypeFilter;

                var orders = await _orderService.GetOrdersAsync(statusFilter, typeFilter);
                var counts = await _orderService.GetStatusCountsAsync();

                int? previousId  = _selectedRow?.Order.OrderId;

                Orders = new ObservableCollection<SelectableOrder>(
                    orders.Select(o => new SelectableOrder(o)));

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
        }

        // ── Status badges ─────────────────────────────────────────────────────

        private void BuildStatusBadges(Dictionary<string, int> counts)
        {
            int total = counts.Values.Sum();
            StatusBadges = new List<StatusBadge>
            {
                Badge("All",                        "All Orders",       total,                                         0x37,0x41,0x51, 0xD1,0xD5,0xDB),
                Badge(OrderStatuses.Pending,        "Pending",          counts.GetValueOrDefault(OrderStatuses.Pending),        0x29,0x21,0x00, 0xF5,0x9E,0x0B),
                Badge(OrderStatuses.Processing,     "Processing",       counts.GetValueOrDefault(OrderStatuses.Processing),     0x1F,0x12,0x00, 0xF9,0x73,0x16),
                Badge(OrderStatuses.ReadyForPickup, "Ready for Pickup", counts.GetValueOrDefault(OrderStatuses.ReadyForPickup), 0x0F,0x1E,0x3D, 0x60,0xA5,0xFA),
                Badge(OrderStatuses.PickedUp,       "Picked Up",        counts.GetValueOrDefault(OrderStatuses.PickedUp),       0x0A,0x1F,0x0E, 0x34,0xD3,0x99),
                Badge(OrderStatuses.OutForDelivery, "Out for Delivery", counts.GetValueOrDefault(OrderStatuses.OutForDelivery),  0x1A,0x0A,0x3D, 0xA7,0x8B,0xFA),
                Badge(OrderStatuses.Delivered,      "Delivered",        counts.GetValueOrDefault(OrderStatuses.Delivered),      0x0A,0x1F,0x0E, 0x34,0xD3,0x99),
                Badge(OrderStatuses.Cancelled,      "Cancelled",        counts.GetValueOrDefault(OrderStatuses.Cancelled),      0x1F,0x00,0x00, 0xF8,0x71,0x71),
            };
        }

        private static StatusBadge Badge(
            string label, string display, int count,
            byte bgR, byte bgG, byte bgB,
            byte fgR, byte fgG, byte fgB) => new()
        {
            Label        = label,
            DisplayLabel = display,
            Count        = count,
            Background   = new SolidColorBrush(Color.FromRgb(bgR, bgG, bgB)),
            Foreground   = new SolidColorBrush(Color.FromRgb(fgR, fgG, fgB)),
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
                    order.Items    = detail.Items;
                    order.Delivery = detail.Delivery;
                    order.Pickup   = detail.Pickup;
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

        private async Task MarkReadyForPickupAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.MarkReadyForPickupAsync(SelectedOrder.OrderId),
                $"Order {SelectedOrder.OrderNumber} is ready for pickup.");
        }

        private async Task ConfirmPickupAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.ConfirmPickupAsync(SelectedOrder.OrderId),
                $"Pickup confirmed for order {SelectedOrder.OrderNumber}.");
        }

        private async Task MarkOutForDeliveryAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.OutForDelivery),
                $"Order {SelectedOrder.OrderNumber} marked as Out for Delivery.");
        }

        private async Task MarkDeliveredAsync()
        {
            if (SelectedOrder == null) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.Delivered),
                $"Order {SelectedOrder.OrderNumber} marked as Delivered.");
        }

        private async Task CancelOrderAsync()
        {
            if (SelectedOrder == null) return;
            if (!_dialog.Confirm(
                    $"Cancel order {SelectedOrder.OrderNumber}? This cannot be undone.",
                    "Cancel Order")) return;
            await ExecuteOrderActionAsync(
                () => _orderService.UpdateOrderStatusAsync(SelectedOrder.OrderId, OrderStatuses.Cancelled),
                $"Order {SelectedOrder.OrderNumber} cancelled.");
        }

        // ── Bulk status actions ───────────────────────────────────────────────

        private async Task BulkUpdateStatusAsync(string? status)
        {
            if (string.IsNullOrEmpty(status)) return;
            var targets = Orders.Where(o => o.IsSelected).Select(o => o.Order).ToList();
            if (targets.Count == 0) return;

            // Pre-validate: filter to only orders where the transition is valid
            var valid   = targets.Where(o => OrderStatuses.IsValidTransition(o.OrderStatus, status)).ToList();
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
                o => _orderService.UpdateOrderStatusAsync(o.OrderId, status),
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
                o => _orderService.UpdateOrderStatusAsync(o.OrderId, OrderStatuses.Cancelled),
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
            OnPropertyChanged(nameof(CanMarkReadyForPickup));
            OnPropertyChanged(nameof(CanConfirmPickup));
            OnPropertyChanged(nameof(CanMarkOutForDelivery));
            OnPropertyChanged(nameof(CanMarkDelivered));
            OnPropertyChanged(nameof(CanCancelOrder));
        }
    }
}
