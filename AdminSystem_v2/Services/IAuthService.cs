using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IAuthService
    {
        /// <summary>Authenticates by email and password. Returns the User on success, null on failure.</summary>
        Task<User?> LoginAsync(string email, string password);

        /// <summary>Clears the current session.</summary>
        void Logout();

        /// <summary>Returns true if the current session user has the given role.</summary>
        Task<bool> HasRoleAsync(string roleName);
    }
}
