using AdminSystem_v2.Helpers;
using AdminSystem_v2.Models;
using AdminSystem_v2.Repositories;

namespace AdminSystem_v2.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;

        public UserService(IUserRepository repo) => _repo = repo;

        public Task<IEnumerable<User>>   GetStaffUsersAsync()   => _repo.GetStaffUsersAsync();
        public Task<IEnumerable<string>> GetAllRoleNamesAsync() => _repo.GetAllRoleNamesAsync();

        public Task SetUserRoleAsync(int userId, string roleName, string callerRole)
        {
            RoleGuard.RequireAdmin(callerRole);
            RejectSelfModification(userId);
            return _repo.SetUserRoleAsync(userId, roleName);
        }

        public Task ToggleActiveAsync(int userId, bool isActive, string callerRole)
        {
            RoleGuard.RequireAdmin(callerRole);
            RejectSelfModification(userId);
            return _repo.ToggleActiveAsync(userId, isActive);
        }

        public async Task CreateStaffAsync(
            string firstName, string lastName,
            string email, string? phone,
            string roleName, string plainPassword,
            string callerRole)
        {
            RoleGuard.RequireAdmin(callerRole);

            var existing = await _repo.FindByEmailAsync(email);
            if (existing != null)
                throw new InvalidOperationException($"An account with email '{email}' already exists.");

            string hash = PasswordHelper.Hash(plainPassword);

            var user = new User
            {
                FirstName   = firstName.Trim(),
                LastName    = lastName.Trim(),
                Email       = email.Trim(),
                PhoneNumber = phone?.Trim() ?? string.Empty,
                IsActive    = true,
                IsWalkIn    = false,
            };

            await _repo.CreateStaffAsync(user, hash, roleName);
        }

        public Task ResetPasswordAsync(int userId, string newPlainPassword, string callerRole)
        {
            RoleGuard.RequireAdmin(callerRole);
            RejectSelfModification(userId);
            string hash = PasswordHelper.Hash(newPlainPassword);
            return _repo.ResetPasswordAsync(userId, hash);
        }

        private static void RejectSelfModification(int targetUserId)
        {
            if (App.CurrentUser != null && App.CurrentUser.UserId == targetUserId)
                throw new InvalidOperationException("You cannot modify your own account.");
        }
    }
}
