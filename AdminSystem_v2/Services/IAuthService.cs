using AdminSystem_v2.Models;

namespace AdminSystem_v2.Services
{
    public interface IAuthService
    {
        /// <summary>Authenticates by email and password. Returns a LoginResult indicating success/failure with a message.</summary>
        Task<LoginResult> LoginAsync(string email, string password);

        /// <summary>Clears the current session.</summary>
        void Logout();

        /// <summary>Returns true if the current session user has the given role.</summary>
        Task<bool> HasRoleAsync(string roleName);
    }
}
