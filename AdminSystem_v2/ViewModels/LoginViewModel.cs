using System.Windows.Input;
using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Services;

namespace AdminSystem_v2.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService   _authService;
        private readonly IDialogService _dialog;

        // ── Bound Properties ──────────────────────────────────────────────

        private string _email = string.Empty;
        public string Email
        {
            get => _email;
            set { SetProperty(ref _email, value); EmailError = string.Empty; }
        }

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set { SetProperty(ref _password, value); PasswordError = string.Empty; }
        }

        private bool _isPasswordVisible;
        public bool IsPasswordVisible
        {
            get => _isPasswordVisible;
            set => SetProperty(ref _isPasswordVisible, value);
        }

        // IsLoading / IsNotLoading inherited from BaseViewModel

        private string _emailError = string.Empty;
        public string EmailError
        {
            get => _emailError;
            set { SetProperty(ref _emailError, value); OnPropertyChanged(nameof(HasEmailError)); }
        }
        public bool HasEmailError => !string.IsNullOrEmpty(_emailError);

        private string _passwordError = string.Empty;
        public string PasswordError
        {
            get => _passwordError;
            set { SetProperty(ref _passwordError, value); OnPropertyChanged(nameof(HasPasswordError)); }
        }
        public bool HasPasswordError => !string.IsNullOrEmpty(_passwordError);

        // ConnectionError reuses inherited ErrorMessage / HasError

        // ── Commands ──────────────────────────────────────────────────────

        public ICommand LoginCommand          { get; }
        public ICommand TogglePasswordCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        // ── Events ────────────────────────────────────────────────────────

        public event Action? LoginSucceeded;

        // ── Constructor ───────────────────────────────────────────────────

        public LoginViewModel(IAuthService authService, IDialogService dialog)
        {
            _authService = authService;
            _dialog      = dialog;

            LoginCommand          = new RelayCommand(async () => await ExecuteLoginAsync(), () => IsNotLoading);
            TogglePasswordCommand = new RelayCommand(() => IsPasswordVisible = !IsPasswordVisible);
            ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);
        }

        // ── Command Handlers ──────────────────────────────────────────────

        private async Task ExecuteLoginAsync()
        {
            EmailError    = string.Empty;
            PasswordError = string.Empty;
            ClearMessages();

            bool valid = true;
            if (string.IsNullOrWhiteSpace(Email))  { EmailError    = "Email address is required."; valid = false; }
            if (string.IsNullOrWhiteSpace(Password)){ PasswordError = "Password is required.";      valid = false; }
            if (!valid) return;

            IsLoading = true;
            try
            {
                LoginResult result = await _authService.LoginAsync(Email, Password);
                if (!result.Success)
                {
                    ShowError(result.ErrorMessage);
                    Password = string.Empty;
                    return;
                }
                LoginSucceeded?.Invoke();
            }
            catch (Exception ex)
            {
                ShowError("Cannot connect to database. Please try again.");
                System.Diagnostics.Debug.WriteLine($"[Login] {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ExecuteForgotPassword()
        {
            _dialog.ShowInfo(
                "Please contact your system administrator to reset your password.",
                "Password Reset");
        }
    }
}
