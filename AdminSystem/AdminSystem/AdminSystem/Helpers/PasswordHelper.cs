// AdminSystem/Helpers/PasswordHelper.cs

using BCrypt.Net;

namespace AdminSystem.Helpers
{
    /// <summary>
    /// Password hashing and verification using BCrypt.
    /// Work factor 12 matches the WebApplication — both apps
    /// share the same Users table and password hashes.
    /// </summary>
    public static class PasswordHelper
    {
        private const int WorkFactor = 12;

        /// <summary>
        /// Hashes a plain-text password using BCrypt with work factor 12.
        /// </summary>
        /// <param name="plainText">The plain-text password to hash.</param>
        /// <returns>A BCrypt hash string safe to store in the database.</returns>
        public static string Hash(string plainText)
            => BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);

        /// <summary>
        /// Verifies a plain-text password against a stored BCrypt hash.
        /// Returns false (never throws) if the hash is null, empty,
        /// or malformed — matches the WebApplication behaviour.
        /// </summary>
        /// <param name="plainText">The plain-text password to verify.</param>
        /// <param name="hash">The stored BCrypt hash from the database.</param>
        /// <returns>True if the password matches the hash; false otherwise.</returns>
        public static bool Verify(string plainText, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                return false;

            try
            {
                return BCrypt.Net.BCrypt.Verify(plainText, hash);
            }
            catch (SaltParseException)
            {
                return false;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                return false;
            }
        }
    }
}