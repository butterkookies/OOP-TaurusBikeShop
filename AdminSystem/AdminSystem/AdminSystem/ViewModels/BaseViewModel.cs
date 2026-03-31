using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AdminSystem.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels.
    /// Provides INotifyPropertyChanged, RelayCommand, and shared helpers.
    /// </summary>
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, string propertyName)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        // ── Shared UI state ────────────────────────────────────────────
        private bool _isLoading;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetField(ref _isLoading, value, "IsLoading"); }
        }

        private string _errorMessage;
        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                SetField(ref _errorMessage, value, "ErrorMessage");
                OnPropertyChanged("HasError");
            }
        }

        public bool HasError
        {
            get { return !string.IsNullOrEmpty(_errorMessage); }
        }

        private string _successMessage;
        public string SuccessMessage
        {
            get { return _successMessage; }
            set
            {
                SetField(ref _successMessage, value, "SuccessMessage");
                OnPropertyChanged("HasSuccess");
            }
        }

        public bool HasSuccess
        {
            get { return !string.IsNullOrEmpty(_successMessage); }
        }

        protected void ClearMessages()
        {
            ErrorMessage   = string.Empty;
            SuccessMessage = string.Empty;
        }

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

        /// <summary>
        /// Runs action on the UI thread safely.
        /// </summary>
        protected void RunOnUI(Action action)
        {
            if (Application.Current?.Dispatcher != null)
                Application.Current.Dispatcher.Invoke(action);
            else
                action();
        }
    }

    // ══════════════════════════════════════════════════════════════════════
    //  RelayCommand — standard ICommand implementation for WPF
    // ══════════════════════════════════════════════════════════════════════
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute,
            Func<object, bool> canExecute = null)
        {
            _execute    = execute
                ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute,
            Func<bool> canExecute = null)
            : this(
                _ => execute(),
                canExecute != null ? (_ => canExecute()) : (Func<object, bool>)null)
        { }

        public event EventHandler CanExecuteChanged
        {
            add    { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
            => _canExecute == null || _canExecute(parameter);

        public void Execute(object parameter)
            => _execute(parameter);
    }

    // ══════════════════════════════════════════════════════════════════════
    //  AsyncRelayCommand — ICommand for async Task methods
    // ══════════════════════════════════════════════════════════════════════
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<Task> _execute;
        private readonly Func<bool> _canExecute;
        private bool _isExecuting;

        public AsyncRelayCommand(Func<Task> execute, Func<bool> canExecute = null)
        {
            _execute    = execute ?? throw new ArgumentNullException("execute");
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
            => !_isExecuting && (_canExecute == null || _canExecute());

        public async void Execute(object parameter)
        {
            if (!CanExecute(parameter)) return;
            _isExecuting = true;
            RaiseCanExecuteChanged();
            try    { await _execute(); }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add    { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        private static void RaiseCanExecuteChanged()
            => System.Windows.Input.CommandManager.InvalidateRequerySuggested();
    }
}
