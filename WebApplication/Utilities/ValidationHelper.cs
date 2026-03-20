using System.Text.RegularExpressions;

namespace WebApplication.Utilities
{
    public static class ValidationHelper
    {
        private static readonly Regex EmailRegex =
            new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex PhilPhoneRegex =
            new(@"^(09|\+639)\d{9}$", RegexOptions.Compiled);

        public static bool IsValidEmail(string? email)
            => !string.IsNullOrWhiteSpace(email) && EmailRegex.IsMatch(email);

        public static bool IsValidPhilippinePhone(string? phone)
            => !string.IsNullOrWhiteSpace(phone) && PhilPhoneRegex.IsMatch(phone.Trim());

        public static bool IsStrongPassword(string? password)
        {
            if (string.IsNullOrEmpty(password) || password.Length < 8)
                return false;
            bool hasUpper   = password.Any(char.IsUpper);
            bool hasDigit   = password.Any(char.IsDigit);
            bool hasSpecial = password.Any(c => !char.IsLetterOrDigit(c));
            return hasUpper && hasDigit && hasSpecial;
        }

        public static bool IsValidPostalCode(string? postalCode)
            => !string.IsNullOrWhiteSpace(postalCode) &&
               postalCode.Trim().Length >= 4 &&
               postalCode.Trim().All(char.IsDigit);
    }
}
