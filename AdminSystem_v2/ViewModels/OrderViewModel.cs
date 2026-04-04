using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;

        // ── Order list ────────────────────────────────────────────────────────

        private ObservableCollection<Order> _orders = new();
        public  ObservableCollection<Order> Orders
        {
            get => _orders;
            private set => SetProperty(ref _orders, value);
        }

        // ── Status filter ─────────────────────────────────────────────────────

        /// <summary>All filter chips shown at the top of the list.</summary>
        public List<string> StatusFilters { get; } = new()
        {
            "All",
            OrderStatuses.Pending,
            OrderStatuses.Processing,
            OrderStatuses.ReadyForPickup,
            OrderStatuses.PickedUp,
            OrderStatuses.Shipped,
            OrderStatuses.Delivered,
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

        // ── Selected order ────────────────────────────────────────────────────

        private Order? _selectedOrder;
        public  Order? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                if (SetProperty(ref _selectedOrder, value))
                    OnOrderSelected(value);
            }
        }

        private bool _isDetailVisible;
        public  bool IsDetailVisible
        {
            get => _isDetailVisible;
            private set => SetProperty(ref _isDetailVisible, value);
        }

        // ── Action availability ───────────────────────────────────────────────

        public bool CanMarkReadyForPickup =>
            _selectedOrder is { DeliveryType: "Pickup" }
            && _selectedOrder.OrderStatus is OrderStatuses.Pending or OrderStatuses.Processing;

        public bool CanConfirmPickup =>
            _selectedOrder is { DeliveryType: "Pickup", OrderStatus: OrderStatuses.ReadyForPickup };

        public bool CanMarkShipped =>
            _selectedOrder is { DeliveryType: "Delivery" }
            && _selectedOrder.OrderStatus is OrderStatuses.Pending or OrderStatuses.Processing;

        public bool CanMarkDelivered =>
            _selectedOrder is { DeliveryType: "Delivery", OrderStatus: OrderStatuses.Shipped };

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand RefreshCommand          { get; }
        public ICommand SelectStatusCommand     { get; }
        public ICommand MarkReadyForPickupCommand { get; }
        public ICommand ConfirmPickupCommand    { get; }
        public ICommand MarkShippedCommand      { get; }
        public ICommand MarkDeliveredCommand    { get; }
        public ICommand CancelOrderCommand      { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;

            RefreshCommand            = new RelayCommand(async () => await LoadAsync());
            SelectStatusCommand       = new RelayCommand<string>(s => SelectedStatusFilter = s ?? "All");
            MarkReadyForPickupCommand = new RelayCommand(async () => await MarkReadyForPickupAsync(), () => CanMarkReadyForPickup);
            ConfirmPickupCommand      = new RelayCommand(async () => await ConfirmPickupAsync(),    () => CanConfirmPickup);
            MarkShippedCommand        = new RelayCommand(async () => await MarkShippedAsync(),      () => CanMarkShipped);
            MarkDeliveredCommand      = new RelayCommand(async () => await MarkDeliveredAsync(),    () => CanMarkDelivered);
            CancelOrderCommand        = new RelayCommand(async () => await CancelOrderAsync(),
                                            () => _selectedOrder?.OrderStatus
                                                    is OrderStatuses.Pending or OrderStatuses.Processing);
        }

        // ── Called by MainWindowViewModel on navigation ───────────────────────

        public async Task LoadAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                var orders = await _orderService.GetOrdersAsync(
                    SelectedStatusFilter == "All" ? null : SelectedStatusFilter);

                RunOnUI(() =>
                {
                    Orders = new ObservableCollection<Order>(orders);

                    // Re-select the same order if it's still in the list after refresh
                    if (_selectedOrder != null)
                    {
                        var still = Orders.FirstOrDefault(o => o.OrderId == _selectedOrder.OrderId);
                        if (still != null) _ = OnOrderSelected(still);
                        else { SelectedOrder = null; IsDetailVisible = false; }
                    }
                });
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

        // ── Selection ─────────────────────────────────────────────────────────

        private async Task OnOrderSelected(Order? order)
        {
            _selectedOrder = order;
            NotifyActionAvailability();

            if (order == null)
            {
                IsDetailVisible = false;
                return;
            }

            // Load full detail (items + delivery/pickup) in the background
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

                RunOnUI(() =>
                {
                    IsDetailVisible = true;
                    NotifyActionAvailability();
                    OnPropertyChanged(nameof(SelectedOrder));
                });
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

        // ── Status-change actions ─────────────────────────────────────────────

        private async Task MarkReadyForPickupAsync()
        {
            if (_selectedOrder == null) return;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _orderService.MarkReadyForPickupAsync(_selectedOrder.OrderId);
                ShowSuccess($"Order {_selectedOrder.OrderNumber} is ready for pickup.");
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        private async Task ConfirmPickupAsync()
        {
            if (_selectedOrder == null) return;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _orderService.ConfirmPickupAsync(_selectedOrder.OrderId);
                ShowSuccess($"Pickup confirmed for order {_selectedOrder.OrderNumber}.");
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        private async Task MarkShippedAsync()
        {
            if (_selectedOrder == null) return;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _orderService.UpdateOrderStatusAsync(_selectedOrder.OrderId, OrderStatuses.Shipped);
                ShowSuccess($"Order {_selectedOrder.OrderNumber} marked as Shipped.");
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        private async Task MarkDeliveredAsync()
        {
            if (_selectedOrder == null) return;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _orderService.UpdateOrderStatusAsync(_selectedOrder.OrderId, OrderStatuses.Delivered);
                ShowSuccess($"Order {_selectedOrder.OrderNumber} marked as Delivered.");
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        private async Task CancelOrderAsync()
        {
            if (_selectedOrder == null) return;

            var confirm = System.Windows.MessageBox.Show(
                $"Cancel order {_selectedOrder.OrderNumber}? This cannot be undone.",
                "Cancel Order",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning);

            if (confirm != System.Windows.MessageBoxResult.Yes) return;

            IsLoading = true;
            ClearMessages();
            try
            {
                await _orderService.UpdateOrderStatusAsync(_selectedOrder.OrderId, OrderStatuses.Cancelled);
                ShowSuccess($"Order {_selectedOrder.OrderNumber} cancelled.");
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Action failed: {ex.Message}");
            }
            finally { IsLoading = false; }
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private void NotifyActionAvailability()
        {
            OnPropertyChanged(nameof(CanMarkReadyForPickup));
            OnPropertyChanged(nameof(CanConfirmPickup));
            OnPropertyChanged(nameof(CanMarkShipped));
            OnPropertyChanged(nameof(CanMarkDelivered));
        }
    }
}
