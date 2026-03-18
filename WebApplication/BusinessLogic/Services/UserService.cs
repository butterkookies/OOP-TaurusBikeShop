// WebApplication/BusinessLogic/Services/UserService.cs

using System.Security.Cryptography;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.DataAccess.Repositories;
using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;
using WebApplication.Utilities;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Implements <see cref="IUserService"/> — handles customer registration,
/// OTP verification, login, and profile management.
/// Follows the flow defined in flowchart Part 1: Customer Registration &amp; Login.
/// </summary>
public sealed class UserService : IUserService
{
    private readonly UserRepository _userRepo;
    private readonly INotificationService _notifications;

    private const int OTPExpiryMinutes = 10;
    private const int OTPLength = 6;

    /// <inheritdoc/>
    public UserService(
        UserRepository userRepo,
        INotificationService notifications)
    {
        _userRepo      = userRepo      ?? throw new ArgumentNullException(nameof(userRepo));
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RegisterAsync(
        RegisterViewModel vm,
        CancellationToken cancellationToken = default)
    {
        // Guard: phone format
        if (!ValidationHelper.IsValidPhilippinePhone(vm.PhoneNumber))
            return ServiceResult.Fail("Phone number must be a valid Philippine mobile number (+639XXXXXXXXX or 09XXXXXXXXX).");

        // Guard: postal code format
        if (!ValidationHelper.IsValidPostalCode(vm.PostalCode))
            return ServiceResult.Fail("Postal code must be a 4-digit number.");

        // Guard: email uniqueness
        User? existing = await _userRepo.FindByEmailAsync(vm.Email, cancellationToken);
        if (existing != null)
            return ServiceResult.Fail("An account with this email address already exists.");

        // Generate and store OTP
        string code = GenerateOTPCode();
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(OTPExpiryMinutes);

        await _userRepo.CreateOTPAsync(vm.Email, code, expiresAt, cancellationToken);

        // Queue OTP notification
        await _notifications.QueueAsync(
            channel:   NotifChannels.Email,
            notifType: NotifTypes.OTPCode,
            recipient: vm.Email,
            subject:   "Your Taurus Bike Shop Verification Code",
            body:      $"Your verification code is: {code}\n\nThis code expires in {OTPExpiryMinutes} minutes.",
            userId:    0, // user does not exist yet — will be updated after account creation
            cancellationToken: cancellationToken);

        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<User>> VerifyOTPAndCreateAccountAsync(
        string email,
        string code,
        RegisterViewModel vm,
        CancellationToken cancellationToken = default)
    {
        // Verify OTP
        bool valid = await _userRepo.VerifyOTPAsync(email, code, cancellationToken);
        if (!valid)
            return ServiceResult<User>.Fail("Invalid or expired verification code.");

        // Create User
        User user = new()
        {
            Email        = vm.Email,
            PasswordHash = PasswordHelper.Hash(vm.Password),
            FirstName    = vm.FirstName.Trim(),
            LastName     = vm.LastName.Trim(),
            PhoneNumber  = vm.PhoneNumber.Trim(),
            IsActive     = true,
            IsWalkIn     = false,
            CreatedAt    = DateTime.UtcNow
        };

        await _userRepo.AddAsync(user, cancellationToken);

        // Create default Address
        Address address = new()
        {
            UserId     = user.UserId,
            Label      = AddressLabels.Home,
            Street     = vm.Street.Trim(),
            City       = vm.City.Trim(),
            PostalCode = vm.PostalCode.Trim(),
            Province   = vm.Province?.Trim(),
            Country    = string.IsNullOrWhiteSpace(vm.Country) ? "Philippines" : vm.Country.Trim(),
            IsDefault  = true,
            IsSnapshot = false,
            CreatedAt  = DateTime.UtcNow
        };

        await _userRepo.Context.Addresses.AddAsync(address, cancellationToken);

        // Set default address FK
        user.DefaultAddressId = address.AddressId;
        await _userRepo.Context.SaveChangesAsync(cancellationToken);

        // Queue Welcome Email
        await _notifications.QueueAsync(
            channel:   NotifChannels.Email,
            notifType: NotifTypes.WelcomeEmail,
            recipient: user.Email!,
            subject:   "Welcome to Taurus Bike Shop!",
            body:      $"Hi {user.FirstName}, your account has been created successfully. Start shopping at Taurus Bike Shop!",
            userId:    user.UserId,
            cancellationToken: cancellationToken);

        return ServiceResult<User>.Ok(user);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> ResendOTPAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ServiceResult.Fail("Email address is required.");

        string code = GenerateOTPCode();
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(OTPExpiryMinutes);

        await _userRepo.CreateOTPAsync(email, code, expiresAt, cancellationToken);

        await _notifications.QueueAsync(
            channel:   NotifChannels.Email,
            notifType: NotifTypes.OTPCode,
            recipient: email,
            subject:   "Your New Taurus Bike Shop Verification Code",
            body:      $"Your new verification code is: {code}\n\nThis code expires in {OTPExpiryMinutes} minutes.",
            userId:    0,
            cancellationToken: cancellationToken);

        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<User>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return ServiceResult<User>.Fail("Email and password are required.");

        User? user = await _userRepo.FindByEmailAsync(email, cancellationToken);

        if (user is null || !PasswordHelper.Verify(password, user.PasswordHash))
            return ServiceResult<User>.Fail("Invalid email address or password.");

        if (!user.IsActive)
            return ServiceResult<User>.Fail("This account has been deactivated. Please contact support.");

        await _userRepo.UpdateLastLoginAsync(user.UserId, cancellationToken);

        return ServiceResult<User>.Ok(user);
    }

    /// <inheritdoc/>
    public async Task<ProfileViewModel?> GetProfileAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        User? user = await _userRepo.GetWithAddressesAsync(userId, cancellationToken);

        if (user is null)
            return null;

        return new ProfileViewModel
        {
            FirstName      = user.FirstName,
            LastName       = user.LastName,
            Email          = user.Email ?? string.Empty,
            PhoneNumber    = user.PhoneNumber,
            SavedAddresses = user.Addresses
                .Where(a => !a.IsSnapshot)
                .OrderByDescending(a => a.IsDefault)
                .ThenBy(a => a.CreatedAt)
                .ToList()
                .AsReadOnly()
        };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> UpdateProfileAsync(
        int userId,
        ProfileViewModel vm,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(vm.PhoneNumber) &&
            !ValidationHelper.IsValidPhilippinePhone(vm.PhoneNumber))
            return ServiceResult.Fail("Phone number must be a valid Philippine mobile number.");

        User? user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return ServiceResult.Fail("User not found.");

        user.FirstName   = vm.FirstName.Trim();
        user.LastName    = vm.LastName.Trim();
        user.PhoneNumber = vm.PhoneNumber?.Trim();

        await _userRepo.UpdateAsync(user, cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> ChangePasswordAsync(
        int userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        User? user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user is null)
            return ServiceResult.Fail("User not found.");

        if (!PasswordHelper.Verify(currentPassword, user.PasswordHash))
            return ServiceResult.Fail("Current password is incorrect.");

        if (newPassword.Length < 8)
            return ServiceResult.Fail("New password must be at least 8 characters.");

        user.PasswordHash = PasswordHelper.Hash(newPassword);
        await _userRepo.UpdateAsync(user, cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> AddAddressAsync(
        int userId,
        ProfileViewModel.NewAddressModel model,
        CancellationToken cancellationToken = default)
    {
        if (!ValidationHelper.IsValidPostalCode(model.PostalCode))
            return ServiceResult.Fail("Postal code must be a 4-digit number.");

        Address address = new()
        {
            UserId     = userId,
            Label      = model.Label,
            Street     = model.Street.Trim(),
            City       = model.City.Trim(),
            PostalCode = model.PostalCode.Trim(),
            Province   = model.Province?.Trim(),
            Country    = string.IsNullOrWhiteSpace(model.Country) ? "Philippines" : model.Country.Trim(),
            IsDefault  = false,
            IsSnapshot = false,
            CreatedAt  = DateTime.UtcNow
        };

        await _userRepo.Context.Addresses.AddAsync(address, cancellationToken);
        await _userRepo.Context.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> DeleteAddressAsync(
        int userId,
        int addressId,
        CancellationToken cancellationToken = default)
    {
        Address? address = await _userRepo.Context.Addresses
            .FindAsync(new object[] { addressId }, cancellationToken);

        if (address is null || address.UserId != userId)
            return ServiceResult.Fail("Address not found.");

        if (address.IsDefault)
            return ServiceResult.Fail("Cannot delete your default address. Please set another address as default first.");

        if (address.IsSnapshot)
            return ServiceResult.Fail("Order snapshot addresses cannot be deleted.");

        _userRepo.Context.Addresses.Remove(address);
        await _userRepo.Context.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok();
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> SetDefaultAddressAsync(
        int userId,
        int addressId,
        CancellationToken cancellationToken = default)
    {
        Address? newDefault = await _userRepo.Context.Addresses
            .FindAsync(new object[] { addressId }, cancellationToken);

        if (newDefault is null || newDefault.UserId != userId || newDefault.IsSnapshot)
            return ServiceResult.Fail("Address not found.");

        // Clear existing default
        List<Address> existingDefaults = await System.Linq.AsyncEnumerable
            .ToListAsync(
                _userRepo.Context.Addresses
                    .Where(a => a.UserId == userId && a.IsDefault && !a.IsSnapshot),
                cancellationToken);

        foreach (Address addr in existingDefaults)
            addr.IsDefault = false;

        newDefault.IsDefault = true;

        // Update User.DefaultAddressId
        User? user = await _userRepo.GetByIdAsync(userId, cancellationToken);
        if (user != null)
            user.DefaultAddressId = addressId;

        await _userRepo.Context.SaveChangesAsync(cancellationToken);
        return ServiceResult.Ok();
    }

    // =========================================================================
    // Private helpers
    // =========================================================================

    /// <summary>
    /// Generates a cryptographically random 6-digit OTP code.
    /// Uses <see cref="RandomNumberGenerator"/> for unpredictability.
    /// </summary>
    private static string GenerateOTPCode()
    {
        int code = RandomNumberGenerator.GetInt32(100000, 1000000);
        return code.ToString("D6");
    }
}