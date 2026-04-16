using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    /// <summary>
    /// CRUD screen for <see cref="StorePaymentAccount"/>. Lets admin/manager
    /// rotate the store-side GCash / BPI account numbers that the customer
    /// website displays on its payment page.
    /// </summary>
    public class StorePaymentAccountViewModel : BaseViewModel
    {
        private readonly IStorePaymentAccountService _service;
        private readonly IDialogService              _dialog;

        public ObservableCollection<StorePaymentAccount> Accounts { get; } = new();

        public List<string> PaymentMethods { get; } = new() { "GCash", "BankTransfer" };

        // ── Selection / form state ────────────────────────────────────────────

        private StorePaymentAccount? _selected;
        public  StorePaymentAccount? Selected
        {
            get => _selected;
            set
            {
                if (SetProperty(ref _selected, value))
                    LoadForm(value);
            }
        }

        private bool _isEditing;
        public  bool IsEditing
        {
            get => _isEditing;
            private set => SetProperty(ref _isEditing, value);
        }

        // Form fields
        private int     _editingId;
        private string  _formMethod       = "GCash";
        private string  _formAccountName  = string.Empty;
        private string  _formAccountNumber = string.Empty;
        private string? _formBankName;
        private string? _formQrImageUrl;
        private string? _formInstructions;
        private bool    _formIsActive     = true;
        private int     _formDisplayOrder;

        public string  FormMethod        { get => _formMethod;        set => SetProperty(ref _formMethod, value); }
        public string  FormAccountName   { get => _formAccountName;   set => SetProperty(ref _formAccountName, value); }
        public string  FormAccountNumber { get => _formAccountNumber; set => SetProperty(ref _formAccountNumber, value); }
        public string? FormBankName      { get => _formBankName;      set => SetProperty(ref _formBankName, value); }
        public string? FormQrImageUrl    { get => _formQrImageUrl;    set => SetProperty(ref _formQrImageUrl, value); }
        public string? FormInstructions  { get => _formInstructions;  set => SetProperty(ref _formInstructions, value); }
        public bool    FormIsActive      { get => _formIsActive;      set => SetProperty(ref _formIsActive, value); }
        public int     FormDisplayOrder  { get => _formDisplayOrder;  set => SetProperty(ref _formDisplayOrder, value); }

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand RefreshCommand { get; }
        public ICommand NewCommand     { get; }
        public ICommand SaveCommand    { get; }
        public ICommand CancelCommand  { get; }
        public ICommand DeleteCommand  { get; }

        public StorePaymentAccountViewModel(
            IStorePaymentAccountService service,
            IDialogService              dialog)
        {
            _service = service;
            _dialog  = dialog;

            RefreshCommand = new RelayCommand(async () => await LoadAsync());
            NewCommand     = new RelayCommand(StartNew);
            SaveCommand    = new RelayCommand(async () => await SaveAsync());
            CancelCommand  = new RelayCommand(CancelEdit);
            DeleteCommand  = new RelayCommand(async () => await DeleteAsync());
        }

        public async Task LoadAsync()
        {
            try
            {
                IsLoading = true;
                ClearMessages();

                Accounts.Clear();
                foreach (var a in await _service.GetAllAsync())
                    Accounts.Add(a);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to load store payment accounts: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void LoadForm(StorePaymentAccount? a)
        {
            if (a is null)
            {
                ResetForm();
                IsEditing = false;
                return;
            }

            _editingId        = a.StorePaymentAccountId;
            FormMethod        = a.PaymentMethod;
            FormAccountName   = a.AccountName;
            FormAccountNumber = a.AccountNumber;
            FormBankName      = a.BankName;
            FormQrImageUrl    = a.QrImageUrl;
            FormInstructions  = a.Instructions;
            FormIsActive      = a.IsActive;
            FormDisplayOrder  = a.DisplayOrder;
            IsEditing         = true;
            ClearMessages();
        }

        private void StartNew()
        {
            Selected = null;
            ResetForm();
            IsEditing = true;
            ClearMessages();
        }

        private void ResetForm()
        {
            _editingId         = 0;
            FormMethod         = "GCash";
            FormAccountName    = string.Empty;
            FormAccountNumber  = string.Empty;
            FormBankName       = null;
            FormQrImageUrl     = null;
            FormInstructions   = null;
            FormIsActive       = true;
            FormDisplayOrder   = 0;
        }

        private void CancelEdit()
        {
            ResetForm();
            Selected  = null;
            IsEditing = false;
            ClearMessages();
        }

        private async Task SaveAsync()
        {
            try
            {
                IsLoading = true;
                ClearMessages();

                (bool ok, string err) result = _editingId == 0
                    ? await _service.CreateAsync(FormMethod, FormAccountName, FormAccountNumber,
                        FormBankName, FormQrImageUrl, FormInstructions, FormIsActive, FormDisplayOrder)
                    : await _service.UpdateAsync(_editingId, FormMethod, FormAccountName, FormAccountNumber,
                        FormBankName, FormQrImageUrl, FormInstructions, FormIsActive, FormDisplayOrder);

                if (!result.ok) { ShowError(result.err); return; }

                ShowSuccess(_editingId == 0 ? "Payment account created." : "Payment account updated.");
                await LoadAsync();
                CancelEdit();
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Save failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteAsync()
        {
            if (_editingId == 0) return;

            if (!_dialog.Confirm(
                    $"Delete payment account #{_editingId}? This cannot be undone.",
                    "Delete payment account"))
                return;

            try
            {
                IsLoading = true;
                ClearMessages();
                await _service.DeleteAsync(_editingId);
                ShowSuccess("Payment account deleted.");
                await LoadAsync();
                CancelEdit();
            }
            catch (UnauthorizedAccessException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Delete failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
