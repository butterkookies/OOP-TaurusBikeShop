using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class OrderViewModel : BaseViewModel
    {
        private readonly IOrderService _orderService;

        public OrderViewModel(IOrderService orderService)
        {
            _orderService = orderService;
            Orders        = new ObservableCollection<Order>();

            LoadOrdersCommand    = new RelayCommand(LoadOrders);
            ApproveOrderCommand  = new RelayCommand(ApproveOrder,
                _ => SelectedOrder != null);
            CancelOrderCommand   = new RelayCommand(CancelOrder,
                _ => SelectedOrder != null
                     && SelectedOrder.OrderStatus != OrderStatuses.Delivered
                     && SelectedOrder.OrderStatus != OrderStatuses.Cancelled);
            UpdateStatusCommand  = new RelayCommand(UpdateStatus,
                _ => SelectedOrder != null
                     && !string.IsNullOrWhiteSpace(PendingStatus));
        }

        // ── Properties ─────────────────────────────────────────────────
        public ObservableCollection<Order> Orders { get; }

        private Order _selectedOrder;
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { SetField(ref _selectedOrder, value, "SelectedOrder"); }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set { SetField(ref _searchText, value, "SearchText"); }
        }

        private string _statusFilter;
        public string StatusFilter
        {
            get { return _statusFilter; }
            set
            {
                SetField(ref _statusFilter, value, "StatusFilter");
                LoadOrders();
            }
        }

        private string _pendingStatus;
        public string PendingStatus
        {
            get { return _pendingStatus; }
            set { SetField(ref _pendingStatus, value, "PendingStatus"); }
        }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadOrdersCommand   { get; }
        public RelayCommand ApproveOrderCommand { get; }
        public RelayCommand CancelOrderCommand  { get; }
        public RelayCommand UpdateStatusCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void LoadOrders()
        {
            IsLoading = true;
            ClearMessages();

            try
            {
                System.Collections.Generic.IEnumerable<Order> result =
                    string.IsNullOrWhiteSpace(StatusFilter)
                        ? _orderService.GetAllOrders()
                        : _orderService.GetOrdersByStatus(StatusFilter);

                Orders.Clear();
                foreach (Order o in result)
                    Orders.Add(o);
            }
            catch (System.Exception ex)
            {
                ShowError("Failed to load orders: " + ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApproveOrder(object param)
        {
            if (SelectedOrder == null) return;
            ClearMessages();
            try
            {
                _orderService.UpdateOrderStatus(
                    SelectedOrder.OrderId, OrderStatuses.Processing);
                ShowSuccess("Order moved to Processing.");
                LoadOrders();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void CancelOrder(object param)
        {
            if (SelectedOrder == null) return;
            ClearMessages();
            try
            {
                _orderService.CancelOrder(SelectedOrder.OrderId);
                ShowSuccess("Order cancelled.");
                LoadOrders();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void UpdateStatus(object param)
        {
            if (SelectedOrder == null || string.IsNullOrWhiteSpace(PendingStatus))
                return;
            ClearMessages();
            try
            {
                _orderService.UpdateOrderStatus(SelectedOrder.OrderId, PendingStatus);
                ShowSuccess("Status updated to " + PendingStatus + ".");
                PendingStatus = string.Empty;
                LoadOrders();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }
    }
}
