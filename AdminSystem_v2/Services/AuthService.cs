using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;

        public AuthService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return null;

            User? user = await _userRepo.FindByEmailAsync(email.Trim());
            if (user == null || !user.IsActive) return null;
            if (!PasswordHelper.Verify(password, user.PasswordHash)) return null;

            await _userRepo.UpdateLastLoginAsync(user.UserId);
            user.Role = await _userRepo.GetUserRoleAsync(user.UserId);

            App.CurrentUser = user;
            return user;
        }

        public void Logout()
        {
            App.CurrentUser = null;
        }

        public async Task<bool> HasRoleAsync(string roleName)
        {
            if (App.CurrentUser == null) return false;
            return await _userRepo.HasRoleAsync(App.CurrentUser.UserId, roleName);
        }
    }
}
