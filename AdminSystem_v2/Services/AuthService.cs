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

        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes   = 15;

        private static readonly HashSet<string> AllowedRoles = new(StringComparer.OrdinalIgnoreCase)
        {
            RoleNames.Admin, RoleNames.Manager, RoleNames.Staff
        };

        public async Task<LoginResult> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return LoginResult.Fail("Invalid email or password.");

            User? user = await _userRepo.FindByEmailAsync(email.Trim());
            if (user == null || !user.IsActive)
                return LoginResult.Fail("Invalid email or password.");

            // Check lockout before password verification
            if (user.LockoutUntil.HasValue && user.LockoutUntil.Value > DateTime.UtcNow)
                return LoginResult.Fail("Account locked. Try again later.");

            if (!PasswordHelper.Verify(password, user.PasswordHash))
            {
                await _userRepo.IncrementFailedLoginsAsync(user.UserId, MaxFailedAttempts, LockoutMinutes);
                int remaining = MaxFailedAttempts - (user.FailedLoginAttempts + 1);
                if (remaining <= 0)
                    return LoginResult.Fail("Account locked. Try again later.");
                return LoginResult.Fail("Invalid email or password.");
            }

            // Reject non-staff accounts (e.g. Customer) from the admin system
            string role = await _userRepo.GetUserRoleAsync(user.UserId);
            if (!AllowedRoles.Contains(role))
                return LoginResult.Fail("Invalid email or password.");

            // Success — reset lockout state and record login
            await _userRepo.ResetFailedLoginsAsync(user.UserId);
            await _userRepo.UpdateLastLoginAsync(user.UserId);
            user.Role = role;

            App.CurrentUser = user;
            return LoginResult.Ok(user);
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
