using System.Windows;

namespace AdminSystem_v2.Services
{
    public class DialogService : IDialogService
    {
        public bool Confirm(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question)
            == MessageBoxResult.Yes;

        public void ShowInfo(string message, string title) =>
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
