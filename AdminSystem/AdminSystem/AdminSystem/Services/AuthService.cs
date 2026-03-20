using AdminSystem.Helpers;
using AdminSystem.Models;
using AdminSystem.Repositories;

namespace AdminSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserRepository _userRepo;

        public AuthService(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
                return null;

            User user = _userRepo.FindByEmail(email.Trim());
            if (user == null || !user.IsActive) return null;
            if (!PasswordHelper.Verify(password, user.PasswordHash)) return null;

            _userRepo.UpdateLastLogin(user.UserId);
            App.CurrentUser = user;
            return user;
        }

        public void Logout()
        {
            App.CurrentUser = null;
        }

        public bool HasRole(string roleName)
        {
            if (App.CurrentUser == null) return false;
            return _userRepo.HasRole(App.CurrentUser.UserId, roleName);
        }
    }
}
