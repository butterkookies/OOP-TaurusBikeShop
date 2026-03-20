using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;

        public UserService(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null) return null;
            if (string.IsNullOrEmpty(user.PasswordHash)) return null;
            if (!PasswordHelper.VerifyPassword(password, user.PasswordHash)) return null;

            user.LastLoginAt = DateTime.UtcNow;
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return user;
        }

        public async Task<(bool Success, string Error)> RegisterAsync(RegisterViewModel model)
        {
            if (await _userRepo.EmailExistsAsync(model.Email))
                return (false, "An account with this email already exists.");

            var user = new User
            {
                Email       = model.Email.Trim().ToLowerInvariant(),
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                // Names are not collected on the registration form; set empty defaults
                FirstName   = string.Empty,
                LastName    = string.Empty,
                PhoneNumber = model.ContactNumber,
                Address     = model.StreetAddress,
                City        = model.City,
                Province    = model.Province,
                PostalCode  = model.PostalCode,
                IsActive    = true,
                CreatedAt   = DateTime.UtcNow
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();
            return (true, string.Empty);
        }

        public async Task<User?> GetByIdAsync(int userId)
            => await _userRepo.GetByIdAsync(userId);

        public async Task<bool> UpdateProfileAsync(int userId, string firstName, string lastName,
            string? phone, string? address, string? city, string? postalCode)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return false;

            user.FirstName   = firstName;
            user.LastName    = lastName;
            user.PhoneNumber = phone;
            user.Address     = address;
            user.City        = city;
            user.PostalCode  = postalCode;

            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return true;
        }
    }
}
