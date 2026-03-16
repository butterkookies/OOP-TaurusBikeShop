// WebApplication/Utilities/PasswordHelper.cs

namespace WebApplication.Utilities;

/// <summary>
/// Pure static helper for BCrypt password hashing and verification.
/// Used by <c>UserService</c> during registration and login.
/// <para>
/// Work factor is set to 12 — a balance between security and performance
/// suitable for a production web application. Increasing beyond 12 will
/// noticeably slow down login on low-CPU hosting environments.
/// </para>
/// <para>
/// <b>Security rules:</b>
/// <list type="bullet">
///   <item>Never log, serialize, store in a cookie, or transmit password hashes.</item>
///   <item>Never compare hashes directly — always use <see cref="Verify"/>.</item>
///   <item>Never store plaintext passwords anywhere, even temporarily.</item>
/// </list>
/// </para>
/// </summary>
public static class PasswordHelper
{
    /// <summary>BCrypt work factor. Higher = slower hash = more brute-force resistant.</summary>
    private const int WorkFactor = 12;

    /// <summary>
    /// Hashes a plaintext password using BCrypt with work factor 12.
    /// The returned hash is safe to store in <c>User.PasswordHash</c>.
    /// </summary>
    /// <param name="password">The plaintext password to hash. Must not be null or empty.</param>
    /// <returns>A BCrypt hash string suitable for database storage.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="password"/> is null or whitespace.
    /// </exception>
    public static string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password must not be null or whitespace.", nameof(password));

        return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
    }

    /// <summary>
    /// Verifies a plaintext password against a stored BCrypt hash.
    /// Safe to call even when <paramref name="hash"/> is null — returns
    /// <c>false</c> rather than throwing, to support walk-in user rows
    /// that have a NULL <c>PasswordHash</c>.
    /// </summary>
    /// <param name="password">The plaintext password provided by the user at login.</param>
    /// <param name="hash">
    /// The BCrypt hash stored in <c>User.PasswordHash</c>.
    /// NULL is accepted and will always return <c>false</c>.
    /// </param>
    /// <returns>
    /// <c>true</c> if the password matches the hash; <c>false</c> otherwise.
    /// </returns>
    public static bool Verify(string password, string? hash)
    {
        if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
        catch (BCrypt.Net.SaltParseException)
        {
            // Hash is malformed — treat as verification failure, not an application error
            return false;
        }
    }
}