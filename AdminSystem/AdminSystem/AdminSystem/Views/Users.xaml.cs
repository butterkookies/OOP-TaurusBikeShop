using System.Windows;
using System.Windows.Controls;
using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.ViewModels;

namespace AdminSystem.Views
{
    /// <summary>
    /// Code-behind for the Users management UserControl.
    /// Delegates all data operations to UsersViewModel.
    /// </summary>
    public partial class UsersView : UserControl
    {
        private readonly UsersViewModel _vm;

        public UsersView(UsersViewModel vm)
        {
            InitializeComponent();
            _vm = vm;
            DataContext = _vm;
        }

        // ── Called by MainWindow.Navigate ─────────────────────────────────
        public void Refresh()
        {
            _vm.Load();
            DgUsers.ItemsSource = _vm.Users;
            ClearDetail();
            ShowListError(_vm.HasError ? _vm.ErrorMessage : null);
        }

        // ── Refresh button ────────────────────────────────────────────────
        private void BtnRefreshUsers_Click(object sender, RoutedEventArgs e)
            => Refresh();

        // ── Selection changed ─────────────────────────────────────────────
        private void DgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            User user = DgUsers.SelectedItem as User;
            _vm.SelectedUser = user;
            UpdateDetailPanel(user);
            HideFeedback();
        }

        // ── Deactivate ────────────────────────────────────────────────────
        private void BtnDeactivate_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedUser == null) return;

            string name = _vm.SelectedUser.FullName;
            MessageBoxResult confirm = System.Windows.MessageBox.Show(
                string.Format("Deactivate user '{0}'? They will no longer be able to log in.", name),
                "Confirm Deactivation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);
            if (confirm != MessageBoxResult.Yes) return;

            _vm.DeactivateCommand.Execute(null);
            if (_vm.HasError)
            {
                ShowActionError(_vm.ErrorMessage);
            }
            else
            {
                ShowSuccess(name + " has been deactivated.");
                DgUsers.ItemsSource = _vm.Users;
                ClearDetail();
            }
        }

        // ── Detail panel helpers ──────────────────────────────────────────
        private void UpdateDetailPanel(User user)
        {
            if (user == null) { ClearDetail(); return; }

            TbDetailName.Text  = user.FullName;
            TbDetailEmail.Text = user.Email;
            TbDetailPhone.Text = user.Phone ?? "—";
            TbDetailJoined.Text = string.Format(
                "Joined {0}", user.CreatedAt.ToString("MMM d, yyyy"));

            // Active badge
            BadgeActive.Background   = user.IsActive ? AppColors.ActiveBg   : AppColors.InactiveBg;
            TbBadgeActive.Text       = user.IsActive ? "Active" : "Inactive";
            TbBadgeActive.Foreground = user.IsActive ? AppColors.Success     : AppColors.Accent;

            // Verified badge
            BadgeVerified.Background   = user.IsEmailVerified ? AppColors.VerifiedBg   : AppColors.UnverifiedBg;
            TbBadgeVerified.Text       = user.IsEmailVerified ? "Email Verified" : "Unverified";
            TbBadgeVerified.Foreground = user.IsEmailVerified ? AppColors.VerifiedText  : AppColors.Muted;

            // Deactivate button — only enabled for active users who are not the current user
            int currentUserId = App.CurrentUser?.UserId ?? 0;
            BtnDeactivate.IsEnabled = user.IsActive && user.UserId != currentUserId;
        }

        private void ClearDetail()
        {
            TbDetailName.Text   = string.Empty;
            TbDetailEmail.Text  = string.Empty;
            TbDetailPhone.Text  = string.Empty;
            TbDetailJoined.Text = string.Empty;
            BtnDeactivate.IsEnabled = false;
        }

        private void ShowListError(string msg)
        {
            if (string.IsNullOrWhiteSpace(msg))
            {
                UsersErrorBar.Visibility = Visibility.Collapsed;
            }
            else
            {
                TbUsersError.Text        = msg;
                UsersErrorBar.Visibility = Visibility.Visible;
            }
        }

        private void ShowSuccess(string msg)
        {
            TbUsersSuccess.Text         = msg;
            TbUsersSuccess.Visibility   = Visibility.Visible;
            TbUsersActionError.Visibility = Visibility.Collapsed;
        }

        private void ShowActionError(string msg)
        {
            TbUsersActionError.Text       = msg;
            TbUsersActionError.Visibility = Visibility.Visible;
            TbUsersSuccess.Visibility     = Visibility.Collapsed;
        }

        private void HideFeedback()
        {
            TbUsersSuccess.Visibility    = Visibility.Collapsed;
            TbUsersActionError.Visibility = Visibility.Collapsed;
        }
    }
}
