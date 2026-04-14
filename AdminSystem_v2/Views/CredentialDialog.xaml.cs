using System.Windows;
using System.Windows.Input;
using AdminSystem_v2.Helpers;

namespace AdminSystem_v2.Views
{
    public partial class CredentialDialog : Window
    {
        /// <summary>True when the admin successfully verified their password.</summary>
        public bool IsVerified { get; private set; }

        public CredentialDialog(string actionDescription)
        {
            InitializeComponent();

            ActionDescription.Text = $"Enter your password to {actionDescription}.";
            EmailField.Text = App.CurrentUser?.Email ?? string.Empty;

            Loaded += (_, _) => PasswordField.Focus();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e) => Verify();

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            IsVerified = false;
            DialogResult = false;
        }

        private void PasswordField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Verify();
            if (e.Key == Key.Escape) { IsVerified = false; DialogResult = false; }
        }

        private void Verify()
        {
            string password = PasswordField.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowError("Please enter your password.");
                return;
            }

            string? storedHash = App.CurrentUser?.PasswordHash;
            if (storedHash == null || !PasswordHelper.Verify(password, storedHash))
            {
                ShowError("Incorrect password. Please try again.");
                PasswordField.Password = string.Empty;
                PasswordField.Focus();
                return;
            }

            IsVerified = true;
            DialogResult = true;
        }

        private void ShowError(string message)
        {
            ErrorText.Text = message;
            ErrorText.Visibility = Visibility.Visible;
        }
    }
}
