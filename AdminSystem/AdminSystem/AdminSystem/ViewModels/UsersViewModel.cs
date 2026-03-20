using System.Collections.ObjectModel;
using AdminSystem.Models;
using AdminSystem.Repositories;

namespace AdminSystem.ViewModels
{
    public class UsersViewModel : BaseViewModel
    {
        private readonly UserRepository _userRepo;

        public UsersViewModel(UserRepository userRepo)
        {
            _userRepo = userRepo;
            Users = new ObservableCollection<User>();

            LoadCommand       = new RelayCommand(Load);
            DeactivateCommand = new RelayCommand(DeactivateUser,
                _ => SelectedUser != null && SelectedUser.IsActive
                     && SelectedUser.UserId != (App.CurrentUser?.UserId ?? 0));
        }

        public ObservableCollection<User> Users { get; }

        private User _selectedUser;
        public User SelectedUser
        {
            get { return _selectedUser; }
            set { SetField(ref _selectedUser, value, "SelectedUser"); }
        }

        public RelayCommand LoadCommand       { get; }
        public RelayCommand DeactivateCommand { get; }

        public void Load()
        {
            IsLoading = true;
            ClearMessages();
            try
            {
                Users.Clear();
                foreach (User u in _userRepo.GetAll())
                    Users.Add(u);
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
            finally { IsLoading = false; }
        }

        private void DeactivateUser(object param)
        {
            if (SelectedUser == null) return;
            ClearMessages();
            try
            {
                _userRepo.Delete(SelectedUser.UserId);
                ShowSuccess(SelectedUser.FullName + " deactivated.");
                Load();
            }
            catch (System.Exception ex) { ShowError(ex.Message); }
        }
    }
}
