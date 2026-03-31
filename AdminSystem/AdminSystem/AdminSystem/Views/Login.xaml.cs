using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;

namespace AdminSystem.Views
{
    public partial class Login : Window
    {
        private bool _passwordVisible = false;
        private readonly UserRepository _userRepo = new UserRepository();

        public Login()
        {
            InitializeComponent();
            BtnLogin.Click          += BtnLogin_Click;
            BtnTogglePassword.Click += BtnTogglePassword_Click;
            BtnForgotPassword.Click += BtnForgotPassword_Click;
            TbUsername.KeyDown      += Field_KeyDown;
            PbPassword.KeyDown      += Field_KeyDown;
            TbPasswordVisible.KeyDown += Field_KeyDown;
            TbPasswordVisible.TextChanged += (s, e) =>
            {
                if (_passwordVisible) PbPassword.Password = TbPasswordVisible.Text;
            };
            TbUsername.TextChanged      += (s, e) => HideError(TbUsernameError);
            PbPassword.PasswordChanged  += (s, e) => HideError(TbPasswordError);
            TbUsername.Focus();
        }

        private void Field_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) AttemptLogin();
        }

        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;
            if (_passwordVisible)
            {
                TbPasswordVisible.Text       = PbPassword.Password;
                PbPassword.Visibility        = Visibility.Collapsed;
                TbPasswordVisible.Visibility = Visibility.Visible;
                TbEyeIcon.Foreground = AppColors.EyeOn;
                TbPasswordVisible.CaretIndex = TbPasswordVisible.Text.Length;
                TbPasswordVisible.Focus();
            }
            else
            {
                PbPassword.Password          = TbPasswordVisible.Text;
                TbPasswordVisible.Visibility = Visibility.Collapsed;
                PbPassword.Visibility        = Visibility.Visible;
                TbEyeIcon.Foreground = AppColors.EyeOff;
                PbPassword.Focus();
            }
        }

        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Please contact your system administrator to reset your password.",
                "Password Reset", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e) => AttemptLogin();

        private void AttemptLogin()
        {
            HideError(TbUsernameError);
            HideError(TbPasswordError);
            HideError(TbConnectionError);

            string email    = TbUsername.Text.Trim();
            string password = _passwordVisible
                ? TbPasswordVisible.Text : PbPassword.Password;

            bool valid = true;
            if (string.IsNullOrEmpty(email))
            {
                ShowError(TbUsernameError, "Email address is required.");
                TbUsername.Focus();
                valid = false;
            }
            if (string.IsNullOrEmpty(password))
            {
                ShowError(TbPasswordError, "Password is required.");
                if (valid) { if (_passwordVisible) TbPasswordVisible.Focus(); else PbPassword.Focus(); }
                valid = false;
            }
            if (!valid) return;

            BtnLogin.IsEnabled = false;
            BtnLogin.Content   = "Signing in...";

            try
            {
                User user = _userRepo.FindByEmail(email);
                if (user == null)
                {
                    ShowError(TbUsernameError, "No account found with that email address.");
                    TbUsername.SelectAll(); TbUsername.Focus();
                    return;
                }
                if (!user.IsActive)
                {
                    ShowError(TbUsernameError, "This account has been deactivated.");
                    return;
                }
                if (!PasswordHelper.Verify(password, user.PasswordHash))
                {
                    ShowError(TbPasswordError, "Incorrect password. Please try again.");
                    PbPassword.Clear(); TbPasswordVisible.Clear();
                    if (_passwordVisible) TbPasswordVisible.Focus(); else PbPassword.Focus();
                    return;
                }

                _userRepo.UpdateLastLogin(user.UserId);
                user.Role = _userRepo.GetUserRole(user.UserId);
                App.CurrentUser = user;
                NavigationHelper.NavigateToMain(this);
            }
            catch (System.Exception ex)
            {
                ShowError(TbConnectionError,
                    "Cannot connect to database: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("[Login] " + ex.ToString());
            }
            finally
            {
                BtnLogin.IsEnabled = true;
                BtnLogin.Content   = "Sign In";
            }
        }

        private static void ShowError(TextBlock tb, string message)
        {
            tb.Text = message; tb.Visibility = Visibility.Visible;
        }
        private static void HideError(TextBlock tb)
        {
            tb.Visibility = Visibility.Collapsed; tb.Text = string.Empty;
        }
    }
}
