using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Services;

namespace AdminSystem.ViewModels
{
    public class InventoryViewModel : BaseViewModel
    {
        private readonly IInventoryService _inventoryService;

        public InventoryViewModel(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            Logs     = new ObservableCollection<InventoryLog>();
            LowStock = new ObservableCollection<InventoryLog>();

            LoadCommand   = new RelayCommand(Load);
            AdjustCommand = new RelayCommand(AdjustStock,
                _ => AdjustVariantId > 0 && AdjustQuantity != 0
                     && !string.IsNullOrWhiteSpace(AdjustChangeType));
        }

        // ── Collections ─────────────────────────────────────────────────
        public ObservableCollection<InventoryLog> Logs     { get; }
        public ObservableCollection<InventoryLog> LowStock { get; }

        // ── Adjustment form ─────────────────────────────────────────────
        private int _adjustVariantId;
        public int AdjustVariantId
        {
            get { return _adjustVariantId; }
            set { SetField(ref _adjustVariantId, value, "AdjustVariantId"); }
        }

        private int _adjustQuantity;
        public int AdjustQuantity
        {
            get { return _adjustQuantity; }
            set { SetField(ref _adjustQuantity, value, "AdjustQuantity"); }
        }

        private string _adjustChangeType;
        public string AdjustChangeType
        {
            get { return _adjustChangeType; }
            set { SetField(ref _adjustChangeType, value, "AdjustChangeType"); }
        }

        private string _adjustNotes;
        public string AdjustNotes
        {
            get { return _adjustNotes; }
            set { SetField(ref _adjustNotes, value, "AdjustNotes"); }
        }

        // ── Commands ────────────────────────────────────────────────────
        public RelayCommand LoadCommand   { get; }
        public RelayCommand AdjustCommand { get; }

        // ── Methods ─────────────────────────────────────────────────────
        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Logs.Clear();
                foreach (InventoryLog l in _inventoryService.GetRecentLogs())
                    Logs.Add(l);

                LowStock.Clear();
                foreach (InventoryLog l in _inventoryService.GetLowStockVariants())
                    LowStock.Add(l);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void AdjustStock(object param)
        {
            ClearMessages();
            try
            {
                _inventoryService.AdjustStock(
                    AdjustVariantId, AdjustQuantity,
                    AdjustChangeType, AdjustNotes);
                ShowSuccess("Stock adjusted successfully.");
                AdjustVariantId  = 0;
                AdjustQuantity   = 0;
                AdjustChangeType = string.Empty;
                AdjustNotes      = string.Empty;
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }
    }
}
