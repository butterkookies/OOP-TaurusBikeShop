using System.Windows;
using AdminSystem_v2.ViewModels;

namespace AdminSystem_v2.Views
{
    public partial class LoginView : Window
    {
        public LoginView(LoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            // PasswordBox cannot bind directly for security reasons.
            // Sync its value into the ViewModel manually.
            PbPassword.PasswordChanged += (s, e) =>
            {
                if (!viewModel.IsPasswordVisible)
                    viewModel.Password = PbPassword.Password;
            };
        }
    }
}
