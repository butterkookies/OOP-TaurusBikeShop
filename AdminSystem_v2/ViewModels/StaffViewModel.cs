using System.Collections.ObjectModel;
using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class StaffViewModel : BaseViewModel
    {
        private readonly IUserService _userService;

        // ── Master list ───────────────────────────────────────────────────────

        private List<User> _allUsers = new();

        /// <summary>Role-filter + search-filtered view of _allUsers. Recomputed on demand.</summary>
        public IEnumerable<User> FilteredUsers
        {
            get
            {
                var q = _allUsers.AsEnumerable();
                if (SelectedRoleFilter != "All")
                    q = q.Where(u => u.Role == SelectedRoleFilter);
                if (!string.IsNullOrWhiteSpace(SearchText))
                    q = q.Where(u =>
                        u.FullName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
                return q;
            }
        }

        // ── Role filter chips ─────────────────────────────────────────────────

        public List<string> RoleFilters { get; } = new()
        {
            "All",
            RoleNames.Admin,
            RoleNames.Manager,
            RoleNames.Staff,
        };

        private string _selectedRoleFilter = "All";
        public  string SelectedRoleFilter
        {
            get => _selectedRoleFilter;
            private set
            {
                if (SetProperty(ref _selectedRoleFilter, value))
                    OnPropertyChanged(nameof(FilteredUsers));
            }
        }

        // ── Search ────────────────────────────────────────────────────────────

        private string _searchText = string.Empty;
        public  string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                    OnPropertyChanged(nameof(FilteredUsers));
            }
        }

        // ── Selected user / detail panel ──────────────────────────────────────

        private User? _selectedUser;
        public  User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (SetProperty(ref _selectedUser, value))
                    OnUserSelected(value);
            }
        }

        private bool _isDetailVisible;
        public  bool IsDetailVisible
        {
            get => _isDetailVisible;
            private set => SetProperty(ref _isDetailVisible, value);
        }

        // ── Role edit (detail panel) ──────────────────────────────────────────

        private string _selectedRole = string.Empty;
        public  string SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        // Role names for drop-downs (staff roles only — loaded once)
        public List<string> StaffRoleNames { get; } = new()
        {
            RoleNames.Admin,
            RoleNames.Manager,
            RoleNames.Staff,
        };

        // ── Password reset (detail panel) ─────────────────────────────────────

        private string _newPasswordText = string.Empty;
        public  string NewPasswordText
        {
            get => _newPasswordText;
            set => SetProperty(ref _newPasswordText, value);
        }

        // ── Add Staff modal ───────────────────────────────────────────────────

        private bool _isCreating;
        public  bool IsCreating
        {
            get => _isCreating;
            private set => SetProperty(ref _isCreating, value);
        }

        private string _newFirstName   = string.Empty;
        private string _newLastName    = string.Empty;
        private string _newEmail       = string.Empty;
        private string _newPhone       = string.Empty;
        private string _newRole        = RoleNames.Staff;
        private string _newPassword    = string.Empty;

        public string NewFirstName { get => _newFirstName; set => SetProperty(ref _newFirstName, value); }
        public string NewLastName  { get => _newLastName;  set => SetProperty(ref _newLastName,  value); }
        public string NewEmail     { get => _newEmail;     set => SetProperty(ref _newEmail,     value); }
        public string NewPhone     { get => _newPhone;     set => SetProperty(ref _newPhone,     value); }
        public string NewRole      { get => _newRole;      set => SetProperty(ref _newRole,      value); }
        public string NewPassword  { get => _newPassword;  set => SetProperty(ref _newPassword,  value); }

        // ── Commands ──────────────────────────────────────────────────────────

        public ICommand RefreshCommand        { get; }
        public ICommand SelectRoleCommand     { get; }
        public ICommand AddStaffCommand       { get; }
        public ICommand SaveNewStaffCommand   { get; }
        public ICommand CancelNewStaffCommand { get; }
        public ICommand SaveRoleCommand       { get; }
        public ICommand ToggleActiveCommand   { get; }
        public ICommand ResetPasswordCommand  { get; }

        // ── Constructor ───────────────────────────────────────────────────────

        public StaffViewModel(IUserService userService)
        {
            _userService = userService;

            RefreshCommand        = new RelayCommand(async () => await LoadAsync());
            SelectRoleCommand     = new RelayCommand<string>(r => SelectedRoleFilter = r ?? "All");
            AddStaffCommand       = new RelayCommand(BeginCreate);
            SaveNewStaffCommand   = new RelayCommand(async () => await SaveNewStaffAsync(), () => IsCreating);
            CancelNewStaffCommand = new RelayCommand(() => IsCreating = false);
            SaveRoleCommand       = new RelayCommand(async () => await SaveRoleAsync(),
                                        () => _selectedUser != null && !string.IsNullOrEmpty(SelectedRole));
            ToggleActiveCommand   = new RelayCommand(async () => await ToggleActiveAsync(),
                                        () => _selectedUser != null);
            ResetPasswordCommand  = new RelayCommand(async () => await ResetPasswordAsync(),
                                        () => _selectedUser != null && !string.IsNullOrWhiteSpace(NewPasswordText));
        }

        // ── Called by MainWindowViewModel on navigation ───────────────────────

        public async Task LoadAsync()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                var users = await _userService.GetStaffUsersAsync();
                _allUsers = users.ToList();
                OnPropertyChanged(nameof(FilteredUsers));
            }
            catch (Exception ex)
            {
                ShowError("Failed to load staff users. Check your database connection.");
                System.Diagnostics.Debug.WriteLine($"[Staff] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Selection ─────────────────────────────────────────────────────────

        private void OnUserSelected(User? user)
        {
            IsDetailVisible  = user != null;
            SelectedRole     = user?.Role ?? string.Empty;
            NewPasswordText  = string.Empty;
            ClearMessages();
        }

        // ── Create staff ──────────────────────────────────────────────────────

        private void BeginCreate()
        {
            NewFirstName = string.Empty;
            NewLastName  = string.Empty;
            NewEmail     = string.Empty;
            NewPhone     = string.Empty;
            NewRole      = RoleNames.Staff;
            NewPassword  = string.Empty;
            IsCreating   = true;
            ClearMessages();
        }

        private async Task SaveNewStaffAsync()
        {
            if (string.IsNullOrWhiteSpace(NewFirstName))
            { ShowError("First name is required."); return; }
            if (string.IsNullOrWhiteSpace(NewLastName))
            { ShowError("Last name is required."); return; }
            if (string.IsNullOrWhiteSpace(NewEmail))
            { ShowError("Email is required."); return; }
            if (string.IsNullOrWhiteSpace(NewPassword) || NewPassword.Length < 8)
            { ShowError("Password must be at least 8 characters."); return; }

            IsLoading = true;
            ClearMessages();
            try
            {
                await _userService.CreateStaffAsync(
                    NewFirstName, NewLastName, NewEmail, NewPhone, NewRole, NewPassword,
                    App.CurrentUser?.Role ?? string.Empty);

                IsCreating = false;
                ShowSuccess($"Staff account created for {NewFirstName} {NewLastName}.");
                await LoadAsync();
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Failed to create account: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Role change ───────────────────────────────────────────────────────

        private async Task SaveRoleAsync()
        {
            if (_selectedUser == null) return;

            IsLoading = true;
            ClearMessages();
            try
            {
                await _userService.SetUserRoleAsync(_selectedUser.UserId, SelectedRole,
                    App.CurrentUser?.Role ?? string.Empty);
                _selectedUser.Role = SelectedRole;
                ShowSuccess($"Role updated to {SelectedRole}.");

                // Refresh list so the role badge updates
                await LoadAsync();

                // Re-select same user
                var refreshed = _allUsers.FirstOrDefault(u => u.UserId == _selectedUser?.UserId);
                if (refreshed != null)
                {
                    _selectedUser = refreshed;
                    SelectedRole  = refreshed.Role;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
            catch (Exception ex)
            {
                ShowError($"Role update failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Toggle active ─────────────────────────────────────────────────────

        private async Task ToggleActiveAsync()
        {
            if (_selectedUser == null) return;

            bool newState = !_selectedUser.IsActive;
            IsLoading = true;
            ClearMessages();
            try
            {
                await _userService.ToggleActiveAsync(_selectedUser.UserId, newState,
                    App.CurrentUser?.Role ?? string.Empty);
                _selectedUser.IsActive = newState;
                ShowSuccess(newState ? "Account activated." : "Account deactivated.");
                OnPropertyChanged(nameof(SelectedUser));
                await LoadAsync();
            }
            catch (Exception ex)
            {
                ShowError($"Status update failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        // ── Password reset ────────────────────────────────────────────────────

        private async Task ResetPasswordAsync()
        {
            if (_selectedUser == null || string.IsNullOrWhiteSpace(NewPasswordText)) return;
            if (NewPasswordText.Length < 8)
            { ShowError("New password must be at least 8 characters."); return; }

            IsLoading = true;
            ClearMessages();
            try
            {
                await _userService.ResetPasswordAsync(_selectedUser.UserId, NewPasswordText,
                    App.CurrentUser?.Role ?? string.Empty);
                NewPasswordText = string.Empty;
                ShowSuccess("Password reset successfully.");
            }
            catch (Exception ex)
            {
                ShowError($"Password reset failed: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
