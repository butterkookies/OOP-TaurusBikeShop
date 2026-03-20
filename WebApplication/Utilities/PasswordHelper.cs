using System.Security.Cryptography;
using System.Text;

namespace WebApplication.Utilities
{
    public static class PasswordHelper
    {
        /// <summary>
        /// Hashes a plain-text password using SHA-256 + salt.
        /// Use BCrypt in production; SHA-256 used here to avoid extra NuGet dependency.
        /// </summary>
        public static string HashPassword(string password)
        {
            // Generate a 16-byte random salt
            var saltBytes = RandomNumberGenerator.GetBytes(16);
            var salt = Convert.ToBase64String(saltBytes);

            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(salt + password);
            var hashBytes = sha256.ComputeHash(combined);
            var hash = Convert.ToBase64String(hashBytes);

            // Store as "salt:hash"
            return $"{salt}:{hash}";
        }

        /// <summary>
        /// Verifies a plain-text password against a stored hash.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(storedHash) || !storedHash.Contains(':'))
                return false;

            var parts = storedHash.Split(':', 2);
            var salt = parts[0];
            var expectedHash = parts[1];

            using var sha256 = SHA256.Create();
            var combined = Encoding.UTF8.GetBytes(salt + password);
            var hashBytes = sha256.ComputeHash(combined);
            var actualHash = Convert.ToBase64String(hashBytes);

            // Constant-time comparison to prevent timing attacks
            return CryptographicOperations.FixedTimeEquals(
                Encoding.UTF8.GetBytes(actualHash),
                Encoding.UTF8.GetBytes(expectedHash));
        }
    }
}
