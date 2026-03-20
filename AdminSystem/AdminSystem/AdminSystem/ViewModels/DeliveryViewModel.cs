using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class DeliveryViewModel : BaseViewModel
    {
        private readonly IDeliveryService _deliveryService;

        public DeliveryViewModel(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
            Deliveries = new ObservableCollection<Delivery>();

            LoadCommand           = new RelayCommand(Load);
            MarkDeliveredCommand  = new RelayCommand(MarkDelivered,
                _ => SelectedDelivery != null);
            MarkFailedCommand     = new RelayCommand(MarkFailed,
                _ => SelectedDelivery != null);
            AssignTrackingCommand = new RelayCommand(AssignTracking,
                _ => SelectedDelivery != null
                     && !string.IsNullOrWhiteSpace(TrackingInput));
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<Delivery> Deliveries { get; }

        // ── Selected ────────────────────────────────────────────────────
        private Delivery _selectedDelivery;
        public Delivery SelectedDelivery
        {
            get { return _selectedDelivery; }
            set { SetField(ref _selectedDelivery, value, "SelectedDelivery"); }
        }

        private string _trackingInput;
        public string TrackingInput
        {
            get { return _trackingInput; }
            set { SetField(ref _trackingInput, value, "TrackingInput"); }
        }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadCommand           { get; }
        public RelayCommand MarkDeliveredCommand  { get; }
        public RelayCommand MarkFailedCommand     { get; }
        public RelayCommand AssignTrackingCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Deliveries.Clear();
                foreach (Delivery d in _deliveryService.GetActiveDeliveries())
                    Deliveries.Add(d);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void MarkDelivered(object param)
        {
            if (SelectedDelivery == null) return;
            ClearMessages();
            try
            {
                _deliveryService.MarkDelivered(SelectedDelivery.DeliveryId);
                ShowSuccess("Marked as Delivered.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void MarkFailed(object param)
        {
            if (SelectedDelivery == null) return;
            ClearMessages();
            try
            {
                _deliveryService.MarkFailed(SelectedDelivery.DeliveryId);
                ShowSuccess("Marked as Failed.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }

        private void AssignTracking(object param)
        {
            if (SelectedDelivery == null
                || string.IsNullOrWhiteSpace(TrackingInput)) return;
            ClearMessages();
            try
            {
                if (SelectedDelivery.Courier == Couriers.Lalamove)
                    _deliveryService.AssignLalamoveBooking(
                        SelectedDelivery.DeliveryId, TrackingInput.Trim());
                else if (SelectedDelivery.Courier == Couriers.LBC)
                    _deliveryService.AssignLBCTracking(
                        SelectedDelivery.DeliveryId, TrackingInput.Trim());
                else
                {
                    ShowError("Unknown courier on this delivery.");
                    return;
                }
                ShowSuccess("Tracking reference assigned.");
                TrackingInput = string.Empty;
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }
    }
}
