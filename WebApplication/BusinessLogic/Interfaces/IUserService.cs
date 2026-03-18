// WebApplication/BusinessLogic/Interfaces/IUserService.cs

using WebApplication.Models.Entities;
using WebApplication.Models.ViewModels;

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for customer account operations covering the full registration
/// and authentication lifecycle defined in flowchart Part 1.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Initiates customer registration by validating the form data,
    /// hashing the password, and sending an OTP code to the provided email.
    /// The user account is NOT created at this point — only an
    /// <c>OTPVerification</c> row is inserted.
    /// </summary>
    /// <param name="vm">The registration form data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success or the specific
    /// validation/business error that occurred.
    /// </returns>
    Task<ServiceResult> RegisterAsync(
        RegisterViewModel vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifies the OTP code submitted by the customer and, on success,
    /// creates the <c>User</c> account, the default <c>Address</c> row,
    /// and queues a WelcomeEmail notification.
    /// </summary>
    /// <param name="email">The email address the OTP was sent to.</param>
    /// <param name="code">The 6-digit code submitted by the customer.</param>
    /// <param name="vm">
    /// The original registration ViewModel — needed to create the User
    /// and Address rows. Should be stored in TempData between the
    /// Register POST and the VerifyOTP POST.
    /// </param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult{User}"/> containing the new user on success,
    /// or an error description on failure.
    /// </returns>
    Task<ServiceResult<User>> VerifyOTPAndCreateAccountAsync(
        string email,
        string code,
        RegisterViewModel vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Resends a fresh OTP to the given email address.
    /// Invalidates any prior unused OTP for that email.
    /// </summary>
    /// <param name="email">The email address to resend the OTP to.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success or failure.
    /// </returns>
    Task<ServiceResult> ResendOTPAsync(
        string email,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates a customer by verifying their email and password.
    /// Updates <c>User.LastLoginAt</c> on success.
    /// </summary>
    /// <param name="email">The submitted email address.</param>
    /// <param name="password">The submitted plaintext password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult{User}"/> containing the authenticated user
    /// on success, or an error on failure (invalid credentials, inactive account).
    /// </returns>
    Task<ServiceResult<User>> LoginAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the profile data for a customer by their user ID.
    /// Includes all saved (non-snapshot) addresses.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A populated <see cref="ProfileViewModel"/>, or <c>null</c>
    /// if the user does not exist.
    /// </returns>
    Task<ProfileViewModel?> GetProfileAsync(
        int userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a customer's personal information (name and phone number).
    /// Email cannot be changed.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="vm">The updated profile data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success or failure.
    /// </returns>
    Task<ServiceResult> UpdateProfileAsync(
        int userId,
        ProfileViewModel vm,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Changes a customer's password after verifying their current password.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="currentPassword">The customer's existing password for verification.</param>
    /// <param name="newPassword">The desired new password.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// A <see cref="ServiceResult"/> indicating success, or an error
    /// if the current password is incorrect.
    /// </returns>
    Task<ServiceResult> ChangePasswordAsync(
        int userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new address to a customer's saved addresses.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="model">The new address data.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating success or failure.</returns>
    Task<ServiceResult> AddAddressAsync(
        int userId,
        ProfileViewModel.NewAddressModel model,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a saved address belonging to the authenticated user.
    /// The default address cannot be deleted — the user must first
    /// set another address as default.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="addressId">The address to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating success or failure.</returns>
    Task<ServiceResult> DeleteAddressAsync(
        int userId,
        int addressId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets a saved address as the user's default shipping address.
    /// </summary>
    /// <param name="userId">The authenticated user's ID.</param>
    /// <param name="addressId">The address to set as default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating success or failure.</returns>
    Task<ServiceResult> SetDefaultAddressAsync(
        int userId,
        int addressId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Represents the result of a service operation that has no return value.
/// Use <see cref="Ok"/> for success and <see cref="Fail"/> for failure.
/// </summary>
public sealed record ServiceResult
{
    /// <summary><c>true</c> if the operation succeeded.</summary>
    public bool IsSuccess { get; init; }

    /// <summary>
    /// Error message when <see cref="IsSuccess"/> is <c>false</c>.
    /// <c>null</c> on success.
    /// </summary>
    public string? Error { get; init; }

    /// <summary>Creates a successful result.</summary>
    public static ServiceResult Ok() => new() { IsSuccess = true };

    /// <summary>Creates a failed result with the given error message.</summary>
    public static ServiceResult Fail(string error) =>
        new() { IsSuccess = false, Error = error };
}

/// <summary>
/// Represents the result of a service operation that returns a value on success.
/// </summary>
/// <typeparam name="T">The type of the return value.</typeparam>
public sealed record ServiceResult<T>
{
    /// <summary><c>true</c> if the operation succeeded.</summary>
    public bool IsSuccess { get; init; }

    /// <summary>The return value when <see cref="IsSuccess"/> is <c>true</c>. <c>null</c> on failure.</summary>
    public T? Value { get; init; }

    /// <summary>Error message when <see cref="IsSuccess"/> is <c>false</c>. <c>null</c> on success.</summary>
    public string? Error { get; init; }

    /// <summary>Creates a successful result with a value.</summary>
    public static ServiceResult<T> Ok(T value) =>
        new() { IsSuccess = true, Value = value };

    /// <summary>Creates a failed result with an error message.</summary>
    public static ServiceResult<T> Fail(string error) =>
        new() { IsSuccess = false, Error = error };
}