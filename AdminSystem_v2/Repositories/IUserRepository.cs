using AdminSystem_v2.Models;

namespace AdminSystem_v2.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        // ── Auth ──────────────────────────────────────────────────────────────
        Task<User?>  FindByEmailAsync(string email);
        Task<string> GetUserRoleAsync(int userId);
        Task         UpdateLastLoginAsync(int userId);
        Task<bool>   HasRoleAsync(int userId, string roleName);

        // ── Staff management ──────────────────────────────────────────────────

        /// <summary>Returns all users with a staff role (Admin / Manager / Staff).</summary>
        Task<IEnumerable<User>> GetStaffUsersAsync();

        /// <summary>Returns the names of every role defined in the Role table.</summary>
        Task<IEnumerable<string>> GetAllRoleNamesAsync();

        /// <summary>
        /// Replaces any existing staff role for the user with the given role name.
        /// Runs inside a transaction.
        /// </summary>
        Task SetUserRoleAsync(int userId, string roleName);

        /// <summary>Flips the IsActive flag for a user account.</summary>
        Task ToggleActiveAsync(int userId, bool isActive);

        /// <summary>
        /// Creates a new staff user account and assigns the given role.
        /// <paramref name="passwordHash"/> must already be a BCrypt hash.
        /// Runs inside a transaction.
        /// </summary>
        Task CreateStaffAsync(User user, string passwordHash, string roleName);

        /// <summary>
        /// Overwrites the password hash for a user.
        /// <paramref name="passwordHash"/> must already be a BCrypt hash.
        /// </summary>
        Task ResetPasswordAsync(int userId, string passwordHash);
    }
}
