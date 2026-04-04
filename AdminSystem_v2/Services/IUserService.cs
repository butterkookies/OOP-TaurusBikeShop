using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>>   GetStaffUsersAsync();
        Task<IEnumerable<string>> GetAllRoleNamesAsync();
        Task SetUserRoleAsync(int userId, string roleName);
        Task ToggleActiveAsync(int userId, bool isActive);

        /// <summary>
        /// Creates a new staff account. Hashes the plain-text password internally.
        /// Throws <see cref="InvalidOperationException"/> if <paramref name="email"/> is already in use.
        /// </summary>
        Task CreateStaffAsync(string firstName, string lastName,
                              string email, string? phone,
                              string roleName, string plainPassword);

        /// <summary>
        /// Resets the password for a staff user. Hashes the plain-text password internally.
        /// </summary>
        Task ResetPasswordAsync(int userId, string newPlainPassword);
    }
}
