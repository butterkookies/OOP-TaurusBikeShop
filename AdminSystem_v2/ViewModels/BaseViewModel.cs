using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdminSystem_v2.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // ── Shared UI state ───────────────────────────────────────────────
        // Every ViewModel inherits these so we never duplicate loading/error
        // state across screens.

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                SetProperty(ref _isLoading, value);
                OnPropertyChanged(nameof(IsNotLoading));
            }
        }
        public bool IsNotLoading => !_isLoading;

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                SetProperty(ref _errorMessage, value);
                OnPropertyChanged(nameof(HasError));
            }
        }
        public bool HasError => !string.IsNullOrEmpty(_errorMessage);

        private string _successMessage = string.Empty;
        public string SuccessMessage
        {
            get => _successMessage;
            set
            {
                SetProperty(ref _successMessage, value);
                OnPropertyChanged(nameof(HasSuccess));
            }
        }
        public bool HasSuccess => !string.IsNullOrEmpty(_successMessage);

        // ── Shared helpers ────────────────────────────────────────────────

        protected void ShowError(string message)
        {
            ErrorMessage   = message;
            SuccessMessage = string.Empty;
        }

        protected void ShowSuccess(string message)
        {
            SuccessMessage = message;
            ErrorMessage   = string.Empty;
        }

        protected void ClearMessages()
        {
            ErrorMessage   = string.Empty;
            SuccessMessage = string.Empty;
        }

    }
}
