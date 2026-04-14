using System.Windows;
using AdminSystem_v2.Views;

namespace AdminSystem_v2.Services
{
    public class DialogService : IDialogService
    {
        public bool Confirm(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
            == MessageBoxResult.Yes;

        public void ShowInfo(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);

        public bool PromptCredentials(string actionDescription)
        {
            var dialog = new CredentialDialog(actionDescription)
            {
                Owner = Application.Current.MainWindow
            };
            return dialog.ShowDialog() == true && dialog.IsVerified;
        }
    }
}
