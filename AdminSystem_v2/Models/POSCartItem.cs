using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdminSystem_v2.Models
{
    /// <summary>A single line in the POS cart — INotifyPropertyChanged so Qty/LineTotal bindings update live.</summary>
    public class POSCartItem : INotifyPropertyChanged
    {
        public int     ProductId        { get; set; }
        public int     ProductVariantId { get; set; }
        public string  ProductName      { get; set; } = string.Empty;
        public string  VariantName      { get; set; } = string.Empty;
        public decimal UnitPrice        { get; set; }
        public int     AvailableStock   { get; set; }

        private int _quantity = 1;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity == value) return;
                _quantity = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(LineTotal));
            }
        }

        public decimal LineTotal => UnitPrice * Quantity;

        public string DisplayName =>
            string.IsNullOrEmpty(VariantName) || VariantName == "Default"
                ? ProductName
                : $"{ProductName} — {VariantName}";

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
