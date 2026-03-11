// Login.xaml.cs
// C# 7.3 compatible
using System.Windows;
using System.Windows.Input;

namespace TaurusBikeShop
{
    public partial class Login : Window
    {
        private bool _passwordVisible = false;

        public Login()
        {
            InitializeComponent();

            // Wire all events — no Click= in XAML
            BtnLogin.Click += BtnLogin_Click;
            BtnTogglePassword.Click += BtnTogglePassword_Click;
            BtnForgotPassword.Click += BtnForgotPassword_Click;

            TbUsername.KeyDown += Field_KeyDown;
            PbPassword.KeyDown += Field_KeyDown;
            TbPasswordVisible.KeyDown += Field_KeyDown;

            // Keep the two password fields in sync
            TbPasswordVisible.TextChanged += TbPasswordVisible_TextChanged;

            TbUsername.Focus();
        }

        // ── SYNC: when visible textbox changes, update PasswordBox ──────
        private void TbPasswordVisible_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (_passwordVisible)
                PbPassword.Password = TbPasswordVisible.Text;
        }

        // ── ENTER KEY on any field ───────────────────────────────────────
        private void Field_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                AttemptLogin();
        }

        // ── SHOW / HIDE PASSWORD TOGGLE ──────────────────────────────────
        private void BtnTogglePassword_Click(object sender, RoutedEventArgs e)
        {
            _passwordVisible = !_passwordVisible;

            if (_passwordVisible)
            {
                TbPasswordVisible.Text = PbPassword.Password;
                PbPassword.Visibility = Visibility.Collapsed;
                TbPasswordVisible.Visibility = Visibility.Visible;
                TbEyeIcon.Foreground = new System.Windows.Media.SolidColorBrush(
                                                System.Windows.Media.Color.FromRgb(0xCC, 0x00, 0x00));
                TbPasswordVisible.CaretIndex = TbPasswordVisible.Text.Length;
                TbPasswordVisible.Focus();
            }
            else
            {
                PbPassword.Password = TbPasswordVisible.Text;
                TbPasswordVisible.Visibility = Visibility.Collapsed;
                PbPassword.Visibility = Visibility.Visible;
                TbEyeIcon.Foreground = new System.Windows.Media.SolidColorBrush(
                                                System.Windows.Media.Color.FromRgb(0x60, 0x60, 0x60));
                PbPassword.Focus();
            }
        }

        private void BtnForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            string user = TbUsername.Text.Trim();

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show(
                    "Please enter your username before requesting a password reset.",
                    "Username Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TbUsername.Focus();
                return;
            }

            // TODO: trigger actual password reset via your backend
            MessageBox.Show(
                "Password reset instructions have been sent to the email address linked to \"" + user + "\".\n\nPlease check your inbox.",
                "Reset Email Sent",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ── SIGN IN ──────────────────────────────────────────────────────
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void AttemptLogin()
        {
            string user = TbUsername.Text.Trim();
            string pass = _passwordVisible ? TbPasswordVisible.Text : PbPassword.Password;

            // Both empty
            if (string.IsNullOrEmpty(user) && string.IsNullOrEmpty(pass))
            {
                MessageBox.Show(
                    "Both username and password are empty.\nPlease fill in your credentials to sign in.",
                    "Fields Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TbUsername.Focus();
                return;
            }

            // Empty username only
            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show(
                    "Username cannot be empty.\nPlease enter your username.",
                    "Username Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                TbUsername.Focus();
                return;
            }

            // Empty password only
            if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show(
                    "Password cannot be empty.\nPlease enter your password.",
                    "Password Required",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                if (_passwordVisible) TbPasswordVisible.Focus();
                else PbPassword.Focus();
                return;
            }

            // TODO: replace with your database authentication call
            // Check username exists (demo: only "admin" is valid)
            if (user != "admin")
            {
                MessageBox.Show(
                    "The username \"" + user + "\" was not found.\nPlease check your username and try again.",
                    "Incorrect Username",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                TbUsername.SelectAll();
                TbUsername.Focus();
                return;
            }

            // Username correct but wrong password
            if (pass != "admin123")
            {
                MessageBox.Show(
                    "The password you entered is incorrect.\nPlease try again or use Forgot password.",
                    "Incorrect Password",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                PbPassword.Clear();
                TbPasswordVisible.Clear();
                if (_passwordVisible) TbPasswordVisible.Focus();
                else PbPassword.Focus();
                return;
            }

            // SUCCESS
            AdminDashboardWindow dashboard = new AdminDashboardWindow();
            dashboard.Show();
            this.Close();
        }
    }
}
