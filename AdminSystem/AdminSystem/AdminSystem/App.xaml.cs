// AdminSystem/App.xaml.cs

using System.Windows;

namespace AdminSystem
{
    /// <summary>
    /// Application entry point.
    /// The app starts at Login.xaml. After successful authentication,
    /// NavigationHelper.NavigateToMain() replaces it with MainWindow.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// The currently authenticated admin user.
        /// Set by AuthService after login and cleared on logout.
        /// Null when no user is logged in.
        /// </summary>
        public static Models.User CurrentUser { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Global unhandled exception handler — logs and shows a friendly message
            DispatcherUnhandledException += (s, args) =>
            {
                System.Diagnostics.Debug.WriteLine(
                    $"[UNHANDLED] {args.Exception.Message}\n{args.Exception.StackTrace}");

                MessageBox.Show(
                    "An unexpected error occurred. Please restart the application.\n\n" +
                    args.Exception.Message,
                    "Taurus Bike Shop — Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                args.Handled = true;
            };
        }
    }
}