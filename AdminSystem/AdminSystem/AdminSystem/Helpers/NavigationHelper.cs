// AdminSystem/Helpers/NavigationHelper.cs

using System.Windows;

namespace AdminSystem.Helpers
{
    /// <summary>
    /// Manages window navigation throughout the AdminSystem.
    /// All window transitions go through this helper to ensure
    /// proper open/close sequencing.
    /// </summary>
    public static class NavigationHelper
    {
        /// <summary>
        /// Opens the main window and closes the login window.
        /// Called after successful authentication.
        /// </summary>
        /// <param name="loginWindow">The login window to close.</param>
        public static void NavigateToMain(Window loginWindow)
        {
            Views.MainWindow main = new Views.MainWindow();
            main.Show();
            loginWindow.Close();
        }

        /// <summary>
        /// Signs out the current user, closes the main window,
        /// and returns to the login screen.
        /// </summary>
        /// <param name="mainWindow">The main window to close.</param>
        public static void SignOut(Window mainWindow)
        {
            App.CurrentUser = null;
            Views.Login login = new Views.Login();
            login.Show();
            mainWindow.Close();
        }

        /// <summary>
        /// Navigates to a specific view within the main window's content area.
        /// </summary>
        /// <param name="mainWindow">The main window hosting the content area.</param>
        /// <param name="view">The view to display.</param>
        public static void NavigateTo(Views.MainWindow mainWindow, FrameworkElement view)
        {
            mainWindow.ContentArea.Content = view;
        }
    }
}