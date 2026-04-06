using System.ComponentModel;
using AdminSystem_v2.Models;

namespace AdminSystem_v2.ViewModels
{
    /// <summary>
    /// Wraps an <see cref="Order"/> and adds a checkbox-selection flag for the
    /// bulk-action DataGrid, keeping selection state out of the domain model.
    /// </summary>
    public sealed class SelectableOrder : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>The underlying domain order.</summary>
        public Order Order { get; }

        private bool _isSelected;

        /// <summary>
        /// Whether this row is checked for bulk operations.
        /// Independent of the DataGrid's row-selection highlight.
        /// </summary>
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected == value) return;
                _isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
            }
        }

        public SelectableOrder(Order order) => Order = order;
    }
}
