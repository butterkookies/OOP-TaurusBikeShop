using BCrypt.Net;

namespace AdminSystem_v2.Helpers
{
    public static class PasswordHelper
    {
        // Work factor 12 matches the WebApplication — both apps share
        // the same Users table and password hashes.
        private const int WorkFactor = 12;

        public static string Hash(string plainText)
            => BCrypt.Net.BCrypt.HashPassword(plainText, WorkFactor);

        /// <summary>
        /// Returns false (never throws) if the hash is null, empty, or malformed.
        /// Matches the WebApplication behaviour.
        /// </summary>
        public static bool Verify(string plainText, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash)) return false;
            try
            {
                return BCrypt.Net.BCrypt.Verify(plainText, hash);
            }
            catch (SaltParseException)           { return false; }
            catch (ArgumentOutOfRangeException)  { return false; }
        }
    }
}
