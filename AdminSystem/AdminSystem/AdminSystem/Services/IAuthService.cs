using AdminSystem.Models;

namespace AdminSystem.Services
{
    /// <summary>
    /// Contract for admin authentication operations.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticates a user by email and password.
        /// Returns the User on success, null on failure.
        /// </summary>
        User Login(string email, string password);

        /// <summary>
        /// Clears the current session user.
        /// </summary>
        void Logout();

        /// <summary>
        /// Returns true if the current user has the given role.
        /// </summary>
        bool HasRole(string roleName);
    }
}
