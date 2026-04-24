// WebApplication/DataAccess/Repositories/UserRepository.cs

using Microsoft.EntityFrameworkCore;
using WebApplication.DataAccess.Context;
using WebApplication.Models.Entities;

namespace WebApplication.DataAccess.Repositories;

/// <summary>
/// Data access for <see cref="User"/> and <see cref="OTPVerification"/> entities.
/// Handles registration, OTP lifecycle, login lookup, and profile updates.
/// </summary>
public sealed class UserRepository : Repository<User>
{
    /// <inheritdoc/>
    public UserRepository(AppDbContext context) : base(context) { }

    /// <summary>
    /// Finds a user by their email address.
    /// Used during login to retrieve the account for password verification.
    /// Returns <c>null</c> when no active user with that email exists.
    /// </summary>
    /// <param name="email">The email address to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The matching <see cref="User"/>, or <c>null</c> if not found.</returns>
    public async Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
            return null;

        return await Context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Email == email && u.IsActive,
                cancellationToken);
    }

    /// <summary>
    /// Creates a new OTP verification record for the given email address.
    /// Called during registration — the user account does not exist yet at this point.
    /// Any prior unused OTP records for the same email are invalidated first
    /// to prevent replay attacks.
    /// </summary>
    /// <param name="email">The email address the OTP was sent to.</param>
    /// <param name="code">The 6-digit OTP code.</param>
    /// <param name="expiresAt">UTC expiry time — typically CreatedAt + 10 minutes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task CreateOTPAsync(
        string email,
        string code,
        DateTime expiresAt,
        CancellationToken cancellationToken = default)
    {
        // Invalidate any existing unused OTPs for this email
        List<OTPVerification> existingOTPs = await Context.OTPVerifications
            .AsTracking() // Modified below (IsUsed = true)
            .Where(o => o.Email == email && !o.IsUsed)
            .ToListAsync(cancellationToken);

        foreach (OTPVerification existing in existingOTPs)
            existing.IsUsed = true;

        OTPVerification otp = new()
        {
            Email = email,
            OTPCode = code,
            IsUsed = false,
            ExpiresAt = expiresAt,
            CreatedAt = DateTime.UtcNow
        };

        await Context.OTPVerifications.AddAsync(otp, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Verifies an OTP code for the given email address.
    /// Checks that the code exists, has not been used, and has not expired.
    /// On success, marks the OTP as used to prevent reuse.
    /// </summary>
    /// <param name="email">The email address the OTP was sent to.</param>
    /// <param name="code">The 6-digit code submitted by the user.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>
    /// <c>true</c> if the code is valid and was successfully consumed;
    /// <c>false</c> if the code is invalid, already used, or expired.
    /// </returns>
    public async Task<bool> VerifyOTPAsync(
        string email,
        string code,
        CancellationToken cancellationToken = default)
    {
        OTPVerification? otp = await Context.OTPVerifications
            .AsTracking() // Modified below (IsUsed = true)
            .FirstOrDefaultAsync(
                o => o.Email == email
                  && o.OTPCode == code
                  && !o.IsUsed
                  && o.ExpiresAt > DateTime.UtcNow,
                cancellationToken);

        if (otp is null)
            return false;

        otp.IsUsed = true;
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }

    /// <summary>
    /// Updates <see cref="User.LastLoginAt"/> to the current UTC time.
    /// Called immediately after a successful login verification.
    /// </summary>
    /// <param name="userId">The ID of the user who just logged in.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task UpdateLastLoginAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        User? user = await DbSet.FindAsync(new object[] { userId }, cancellationToken);

        if (user is null)
            return;

        user.LastLoginAt = DateTime.UtcNow;
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Returns a user with their role assignments loaded.
    /// Used by services that need to check role membership after login.
    /// </summary>
    /// <param name="userId">The user ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user with <c>UserRoles.Role</c> included, or <c>null</c>.</returns>
    public async Task<User?> GetWithRolesAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Users
            .AsNoTracking()
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    /// <summary>
    /// Returns a user with their saved (non-snapshot) addresses loaded.
    /// Used by the profile and checkout flows.
    /// </summary>
    /// <param name="userId">The user ID to load.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The user with live addresses included, or <c>null</c>.</returns>
    public async Task<User?> GetWithAddressesAsync(
        int userId,
        CancellationToken cancellationToken = default)
    {
        return await Context.Users
            .AsNoTracking()
            .Include(u => u.Addresses.Where(a => !a.IsSnapshot))
            .FirstOrDefaultAsync(u => u.UserId == userId, cancellationToken);
    }

    /// <summary>
    /// Creates a new ActiveSession record in the database for a logged-in user.
    /// </summary>
    /// <param name="userId">The ID of the user logging in.</param>
    /// <param name="refreshToken">The session GUID/Token assigned.</param>
    /// <param name="ipAddress">The IP address of the client.</param>
    /// <param name="deviceInfo">The User-Agent of the client.</param>
    /// <param name="expiresInDays">Session validity duration.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task CreateActiveSessionAsync(
        int userId,
        string refreshToken,
        string? ipAddress,
        string? deviceInfo,
        int expiresInDays = 30,
        CancellationToken cancellationToken = default)
    {
        ActiveSession session = new()
        {
            UserId = userId,
            RefreshToken = refreshToken,
            IpAddress = ipAddress,
            DeviceInfo = deviceInfo,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddDays(expiresInDays),
            CreatedAt = DateTime.UtcNow
        };

        await Context.ActiveSessions.AddAsync(session, cancellationToken);
        await Context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Revokes an active session by its RefreshToken/SessionId preventing reuse.
    /// </summary>
    /// <param name="refreshToken">The token of the session to revoke.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task RevokeActiveSessionAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        ActiveSession? session = await Context.ActiveSessions
            .AsTracking()
            .FirstOrDefaultAsync(s => s.RefreshToken == refreshToken && !s.IsRevoked, cancellationToken);

        if (session is null)
            return;

        session.IsRevoked = true;
        session.RevokedAt = DateTime.UtcNow;
        await Context.SaveChangesAsync(cancellationToken);
    }
}