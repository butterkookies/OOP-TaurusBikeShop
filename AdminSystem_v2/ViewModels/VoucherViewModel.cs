using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class VoucherViewModel : BaseViewModel
    {
        private readonly IVoucherService _svc;
        private readonly IDialogService  _dialog;

        /// <summary>True when the current user can create/edit/delete vouchers (Admin or Manager).</summary>
        public bool CanEdit => RoleGuard.IsAdminOrManager(App.CurrentUser?.Role ?? string.Empty);

        // ── Active tab ────────────────────────────────────────────────────────

        private string _activeTab = "Vouchers";
        public  string ActiveTab
        {
            get => _activeTab;
            private set => SetProperty(ref _activeTab, value);
        }

        public bool IsVouchersTab => ActiveTab == "Vouchers";
        public bool IsAssignTab   => ActiveTab == "Assign";

        // ── Voucher list ──────────────────────────────────────────────────────

        private ObservableCollection<VoucherListItem> _vouchers = new();
        public  ObservableCollection<VoucherListItem> Vouchers
        {
            get => _vouchers;
            private set => SetProperty(ref _vouchers, value);
        }

        private string _listSearch = string.Empty;
        public  string ListSearch
        {
            get => _listSearch;
            set
            {
                if (SetProperty(ref _listSearch, value))
                    ApplyFilter();
            }
        }

        private ObservableCollection<VoucherListItem> _filteredVouchers = new();
        public  ObservableCollection<VoucherListItem> FilteredVouchers
        {
            get => _filteredVouchers;
            private set => SetProperty(ref _filteredVouchers, value);
        }

        // ── Form state ────────────────────────────────────────────────────────

        private bool _isFormOpen;
        public  bool IsFormOpen
        {
            get => _isFormOpen;
            private set => SetProperty(ref _isFormOpen, value);
        }

        private bool _isEditMode;
        public  bool IsEditMode
        {
            get => _isEditMode;
            private set
            {
                SetProperty(ref _isEditMode, value);
                OnPropertyChanged(nameof(FormTitle));
            }
        }

        public string FormTitle => _isEditMode ? "Edit Voucher" : "New Voucher";

        private int _editingVoucherId;

        // ── Form fields ───────────────────────────────────────────────────────

        private string _code = string.Empty;
        public  string Code
        {
            get => _code;
            set => SetProperty(ref _code, value.ToUpperInvariant());
        }

        private string _description = string.Empty;
        public  string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _discountType = "Percentage";
        public  string DiscountType
        {
            get => _discountType;
            set
            {
                SetProperty(ref _discountType, value);
                OnPropertyChanged(nameof(IsPercentage));
            }
        }
        public bool IsPercentage => _discountType == "Percentage";

        public IReadOnlyList<string> DiscountTypes { get; } = new[] { "Percentage", "Fixed" };

        private string _discountValueText = string.Empty;
        public  string DiscountValueText
        {
            get => _discountValueText;
            set => SetProperty(ref _discountValueText, value);
        }

        private string _minOrderText = string.Empty;
        public  string MinOrderText
        {
            get => _minOrderText;
            set => SetProperty(ref _minOrderText, value);
        }

        private string _maxUsesText = string.Empty;
        public  string MaxUsesText
        {
            get => _maxUsesText;
            set => SetProperty(ref _maxUsesText, value);
        }

        private string _maxUsesPerUserText = string.Empty;
        public  string MaxUsesPerUserText
        {
            get => _maxUsesPerUserText;
            set => SetProperty(ref _maxUsesPerUserText, value);
        }

        private DateTime _startDate = DateTime.Today;
        public  DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        private DateTime? _endDate;
        public  DateTime? EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        private bool _formIsActive = true;
        public  bool FormIsActive
        {
            get => _formIsActive;
            set => SetProperty(ref _formIsActive, value);
        }

        // ── Assign tab ────────────────────────────────────────────────────────

        private VoucherListItem? _assignVoucher;
        public  VoucherListItem? AssignVoucher
        {
            get => _assignVoucher;
            set => SetProperty(ref _assignVoucher, value);
        }

        private string _userSearch = string.Empty;
        public  string UserSearch
        {
            get => _userSearch;
            set
            {
                if (SetProperty(ref _userSearch, value))
                    _ = SearchUsersAsync();
            }
        }

        private ObservableCollection<VoucherUserRow> _userResults = new();
        public  ObservableCollection<VoucherUserRow> UserResults
        {
            get => _userResults;
            private set => SetProperty(ref _userResults, value);
        }

        private bool _isUserDropdownOpen;
        public  bool IsUserDropdownOpen
        {
            get => _isUserDropdownOpen;
            set => SetProperty(ref _isUserDropdownOpen, value);
        }

        private ObservableCollection<VoucherUserRow> _selectedUsers = new();
        public  ObservableCollection<VoucherUserRow> SelectedUsers
        {
            get => _selectedUsers;
            private set => SetProperty(ref _selectedUsers, value);
        }

        private DateTime? _assignExpiresAt;
        public  DateTime? AssignExpiresAt
        {
            get => _assignExpiresAt;
            set => SetProperty(ref _assignExpiresAt, value);
        }

        private bool _sendInApp = true;
        public  bool SendInApp
        {
            get => _sendInApp;
            set => SetProperty(ref _sendInApp, value);
        }

        private bool _sendEmail;
        public  bool SendEmail
        {
            get => _sendEmail;
            set => SetProperty(ref _sendEmail, value);
        }

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand ShowVouchersTabCommand { get; }
        public ICommand ShowAssignTabCommand   { get; }

        public ICommand NewVoucherCommand      { get; }
        public ICommand EditVoucherCommand     { get; }
        public ICommand SaveVoucherCommand     { get; }
        public ICommand CancelFormCommand      { get; }
        public ICommand ToggleActiveCommand    { get; }
        public ICommand GenerateCodeCommand    { get; }
        public ICommand RefreshCommand         { get; }

        public ICommand AddUserCommand         { get; }
        public ICommand RemoveUserCommand      { get; }
        public ICommand AssignCommand          { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public VoucherViewModel(IVoucherService svc, IDialogService dialog)
        {
            _svc    = svc;
            _dialog = dialog;

            ShowVouchersTabCommand = new RelayCommand(() => SwitchTab("Vouchers"));
            ShowAssignTabCommand   = new RelayCommand(() => SwitchTab("Assign"));

            NewVoucherCommand   = new RelayCommand(OpenNewForm);
            EditVoucherCommand  = new RelayCommand<VoucherListItem>(OpenEditForm);
            SaveVoucherCommand  = new RelayCommand(async () => await SaveAsync());
            CancelFormCommand   = new RelayCommand(CloseForm);
            ToggleActiveCommand = new RelayCommand<VoucherListItem>(async v => await ToggleActiveAsync(v));
            GenerateCodeCommand = new RelayCommand(GenerateCode);
            RefreshCommand      = new RelayCommand(async () => await LoadAsync());

            AddUserCommand    = new RelayCommand<VoucherUserRow>(AddUser);
            RemoveUserCommand = new RelayCommand<VoucherUserRow>(RemoveUser);
            AssignCommand     = new RelayCommand(async () => await AssignAndNotifyAsync());
        }

        // ── Navigation entry point ────────────────────────────────────────────

        public async Task LoadAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                var list = await _svc.GetAllVouchersAsync();
                Vouchers = new ObservableCollection<VoucherListItem>(list);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                ShowError("Failed to load vouchers. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Vouchers] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Tab switching ─────────────────────────────────────────────────────

        private void SwitchTab(string tab)
        {
            ActiveTab = tab;
            OnPropertyChanged(nameof(IsVouchersTab));
            OnPropertyChanged(nameof(IsAssignTab));
            ClearMessages();

            if (tab == "Assign")
            {
                // Populate ComboBox with active vouchers only
                var active = Vouchers.Where(v => v.IsActive && !v.IsExpired).ToList();
                AssignVoucher = active.FirstOrDefault();
            }
        }

        // ── Filter ────────────────────────────────────────────────────────────

        private void ApplyFilter()
        {
            if (string.IsNullOrWhiteSpace(_listSearch))
            {
                FilteredVouchers = new ObservableCollection<VoucherListItem>(Vouchers);
                return;
            }

            string q = _listSearch.Trim().ToLowerInvariant();
            FilteredVouchers = new ObservableCollection<VoucherListItem>(
                Vouchers.Where(v =>
                    v.Code.ToLowerInvariant().Contains(q) ||
                    v.Description.ToLowerInvariant().Contains(q) ||
                    v.StatusDisplay.ToLowerInvariant().Contains(q)));
        }

        // ── Form open/close ───────────────────────────────────────────────────

        private void OpenNewForm()
        {
            _editingVoucherId  = 0;
            IsEditMode         = false;
            Code               = string.Empty;
            Description        = string.Empty;
            DiscountType       = "Percentage";
            DiscountValueText  = string.Empty;
            MinOrderText       = string.Empty;
            MaxUsesText        = string.Empty;
            MaxUsesPerUserText = string.Empty;
            StartDate          = DateTime.Today;
            EndDate            = null;
            FormIsActive       = true;
            ClearMessages();
            IsFormOpen         = true;
        }

        private void OpenEditForm(VoucherListItem? v)
        {
            if (v == null) return;
            _editingVoucherId  = v.VoucherId;
            IsEditMode         = true;
            Code               = v.Code;
            Description        = v.Description;
            DiscountType       = v.DiscountType;
            DiscountValueText  = v.DiscountValue.ToString("N2");
            MinOrderText       = v.MinimumOrderAmount.HasValue ? v.MinimumOrderAmount.Value.ToString("N2") : string.Empty;
            MaxUsesText        = v.MaxUses.HasValue            ? v.MaxUses.Value.ToString()               : string.Empty;
            MaxUsesPerUserText = v.MaxUsesPerUser.HasValue     ? v.MaxUsesPerUser.Value.ToString()        : string.Empty;
            StartDate          = v.StartDate;
            EndDate            = v.EndDate;
            FormIsActive       = v.IsActive;
            ClearMessages();
            IsFormOpen         = true;
        }

        private void CloseForm()
        {
            IsFormOpen = false;
            ClearMessages();
        }

        // ── Save ──────────────────────────────────────────────────────────────

        private async Task SaveAsync()
        {
            if (!decimal.TryParse(DiscountValueText, out decimal discountValue))
            {
                ShowError("Discount value must be a valid number."); return;
            }

            decimal? minOrder = null;
            if (!string.IsNullOrWhiteSpace(MinOrderText))
            {
                if (!decimal.TryParse(MinOrderText, out decimal mo)) { ShowError("Minimum order amount must be a valid number."); return; }
                minOrder = mo;
            }

            int? maxUses = null;
            if (!string.IsNullOrWhiteSpace(MaxUsesText))
            {
                if (!int.TryParse(MaxUsesText, out int mu) || mu <= 0) { ShowError("Max uses must be a positive whole number."); return; }
                maxUses = mu;
            }

            int? maxPerUser = null;
            if (!string.IsNullOrWhiteSpace(MaxUsesPerUserText))
            {
                if (!int.TryParse(MaxUsesPerUserText, out int mp) || mp <= 0) { ShowError("Max uses per user must be a positive whole number."); return; }
                maxPerUser = mp;
            }

            IsLoading = true;
            ClearMessages();
            try
            {
                (bool ok, string error) = _isEditMode
                    ? await _svc.UpdateVoucherAsync(
                        _editingVoucherId, Code, Description, DiscountType, discountValue,
                        minOrder, maxUses, maxPerUser, StartDate, EndDate, FormIsActive)
                    : await _svc.CreateVoucherAsync(
                        Code, Description, DiscountType, discountValue,
                        minOrder, maxUses, maxPerUser, StartDate, EndDate, FormIsActive);

                if (!ok) { ShowError(error); return; }

                ShowSuccess(_isEditMode ? "Voucher updated." : "Voucher created.");
                IsFormOpen = false;
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError("Could not save voucher. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Vouchers/Save] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Toggle active ─────────────────────────────────────────────────────

        private async Task ToggleActiveAsync(VoucherListItem? v)
        {
            if (v == null) return;

            string action = v.IsActive ? "deactivate" : "activate";
            if (!_dialog.Confirm($"Are you sure you want to {action} voucher \"{v.Code}\"?", "Confirm"))
                return;

            IsLoading = true;
            try
            {
                if (v.IsActive) await _svc.DeactivateVoucherAsync(v.VoucherId);
                else            await _svc.ActivateVoucherAsync(v.VoucherId);

                await LoadAsync();
                ShowSuccess($"Voucher \"{v.Code}\" {action}d.");
            }
            catch (Exception ex)
            {
                ShowError($"Could not {action} voucher.");
                System.Diagnostics.Debug.WriteLine($"[Vouchers/Toggle] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Generate code ─────────────────────────────────────────────────────

        private void GenerateCode()
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var rng = new Random();
            string suffix = new string(Enumerable.Range(0, 8).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
            Code = $"TBS-{suffix}";
        }

        // ── Assign tab — user search ──────────────────────────────────────────

        private async Task SearchUsersAsync()
        {
            if (string.IsNullOrWhiteSpace(_userSearch))
            {
                UserResults        = new ObservableCollection<VoucherUserRow>();
                IsUserDropdownOpen = false;
                return;
            }
            try
            {
                var results = await _svc.SearchUsersAsync(_userSearch);
                // Exclude already-selected users
                var filtered = results
                    .Where(r => !SelectedUsers.Any(s => s.UserId == r.UserId))
                    .ToList();
                UserResults        = new ObservableCollection<VoucherUserRow>(filtered);
                IsUserDropdownOpen = UserResults.Count > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[Vouchers/UserSearch] {ex}");
            }
        }

        private void AddUser(VoucherUserRow? user)
        {
            if (user == null) return;
            if (!SelectedUsers.Any(u => u.UserId == user.UserId))
                SelectedUsers.Add(user);

            UserResults        = new ObservableCollection<VoucherUserRow>();
            IsUserDropdownOpen = false;
            UserSearch         = string.Empty;
            OnPropertyChanged(nameof(SelectedUsers));
        }

        private void RemoveUser(VoucherUserRow? user)
        {
            if (user == null) return;
            SelectedUsers.Remove(user);
            OnPropertyChanged(nameof(SelectedUsers));
        }

        // ── Assign & notify ───────────────────────────────────────────────────

        private async Task AssignAndNotifyAsync()
        {
            if (AssignVoucher == null)       { ShowError("Please select a voucher.");             return; }
            if (SelectedUsers.Count == 0)    { ShowError("Please add at least one customer.");    return; }
            if (!SendInApp && !SendEmail)    { ShowError("Select at least one notification channel."); return; }

            if (!_dialog.Confirm(
                    $"Assign \"{AssignVoucher.Code}\" to {SelectedUsers.Count} customer(s) and send notifications?",
                    "Confirm Assignment"))
                return;

            IsLoading = true;
            ClearMessages();
            try
            {
                var (assigned, message) = await _svc.AssignAndNotifyAsync(
                    AssignVoucher.VoucherId,
                    AssignVoucher.Code,
                    AssignVoucher.DiscountDisplay,
                    AssignVoucher.Description,
                    SelectedUsers,
                    AssignExpiresAt,
                    SendInApp,
                    SendEmail);

                ShowSuccess(message);
                SelectedUsers      = new ObservableCollection<VoucherUserRow>();
                AssignExpiresAt    = null;
                UserSearch         = string.Empty;
                await LoadAsync();
            }
            catch (Exception ex)
            {
                string detail = ex.InnerException?.Message ?? ex.Message;
                ShowError($"Assignment failed: {detail}");
                System.Diagnostics.Debug.WriteLine($"[Vouchers/Assign] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
