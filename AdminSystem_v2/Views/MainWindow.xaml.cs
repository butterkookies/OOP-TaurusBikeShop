using System.ComponentModel;
using System.Windows;
using AdminSystem_v2.ViewModels;

namespace AdminSystem_v2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Let the ViewModel's ExitCommand handle the confirmation dialog.
            // Only cancel if the user hasn't already confirmed through the command.
            if (App.CurrentUser != null)
            {
                var result = MessageBox.Show(
                    "Exit Taurus Bike Shop Admin?",
                    "Exit",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes)
                    e.Cancel = true;
            }
        }
    }
}
