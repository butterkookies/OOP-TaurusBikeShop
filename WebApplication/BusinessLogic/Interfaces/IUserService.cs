using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<(bool Success, string Error)> RegisterAsync(RegisterViewModel model);
        Task<User?> GetByIdAsync(int userId);
        Task<bool> UpdateProfileAsync(int userId, string firstName, string lastName, string? phone, string? address, string? city, string? postalCode);
    }
}
