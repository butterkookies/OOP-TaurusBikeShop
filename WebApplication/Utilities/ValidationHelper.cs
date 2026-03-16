// WebApplication/Utilities/ValidationHelper.cs

using System.Text.RegularExpressions;

namespace WebApplication.Utilities;

/// <summary>
/// Pure static helper for custom input validation rules specific to the
/// Taurus Bike Shop Philippine context.
/// Used by <c>CustomerController</c> and <c>CheckoutController</c> to validate
/// user-submitted data at the service boundary, supplementing the data annotation
/// validation that runs at the ViewModel level.
/// <para>
/// All methods are pure functions — no state, no I/O, no dependencies.
/// </para>
/// </summary>
public static class ValidationHelper
{
    // Philippine mobile number: starts with +639 or 09, followed by 9 digits.
    // Total digits: 11 (local format) or 12 with country code prefix.
    private static readonly Regex PhilippinePhoneRegex = new(
        @"^(\+639|09)\d{9}$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    // Philippine postal code: exactly 4 numeric digits.
    private static readonly Regex PostalCodeRegex = new(
        @"^\d{4}$",
        RegexOptions.Compiled,
        TimeSpan.FromMilliseconds(100));

    // RFC 5321 / RFC 5322 simplified email pattern.
    // More restrictive than [EmailAddress] attribute to catch common typos.
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+\-]+@[a-zA-Z0-9.\-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    // Voucher code: alphanumeric and hyphens only, 3–50 characters.
    private static readonly Regex VoucherCodeRegex = new(
        @"^[A-Z0-9\-]{3,50}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase,
        TimeSpan.FromMilliseconds(100));

    /// <summary>
    /// Validates a Philippine mobile phone number.
    /// Accepts formats: <c>+639XXXXXXXXX</c> or <c>09XXXXXXXXX</c>.
    /// Total length: 12 characters with +63 prefix, or 11 with 09 prefix.
    /// </summary>
    /// <param name="phone">The phone number string to validate.</param>
    /// <returns>
    /// <c>true</c> if the phone number matches a valid Philippine mobile format;
    /// <c>false</c> otherwise or if <paramref name="phone"/> is null or whitespace.
    /// </returns>
    public static bool IsValidPhilippinePhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        string normalized = phone.Trim().Replace(" ", "").Replace("-", "");
        return PhilippinePhoneRegex.IsMatch(normalized);
    }

    /// <summary>
    /// Validates a Philippine postal code.
    /// Must be exactly 4 numeric digits (e.g. 1000 for Manila, 4117 for Batangas).
    /// </summary>
    /// <param name="postalCode">The postal code string to validate.</param>
    /// <returns>
    /// <c>true</c> if the postal code is exactly 4 digits;
    /// <c>false</c> otherwise or if <paramref name="postalCode"/> is null or whitespace.
    /// </returns>
    public static bool IsValidPostalCode(string? postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
            return false;

        return PostalCodeRegex.IsMatch(postalCode.Trim());
    }

    /// <summary>
    /// Validates an email address using a stricter pattern than the standard
    /// <c>[EmailAddress]</c> data annotation.
    /// Checks for a valid local part, @ symbol, domain, and TLD of at least 2 characters.
    /// </summary>
    /// <param name="email">The email address string to validate.</param>
    /// <returns>
    /// <c>true</c> if the email address is valid;
    /// <c>false</c> otherwise or if <paramref name="email"/> is null or whitespace.
    /// </returns>
    public static bool IsValidEmail(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        return EmailRegex.IsMatch(email.Trim());
    }

    /// <summary>
    /// Validates that a voucher code contains only permitted characters.
    /// Accepts uppercase letters, digits, and hyphens between 3 and 50 characters.
    /// Does not validate whether the code exists or is active — that is done by
    /// <c>VoucherService.ValidateAsync</c>.
    /// </summary>
    /// <param name="code">The voucher code string to validate.</param>
    /// <returns>
    /// <c>true</c> if the voucher code format is valid;
    /// <c>false</c> otherwise or if <paramref name="code"/> is null or whitespace.
    /// </returns>
    public static bool IsValidVoucherCode(string? code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return false;

        return VoucherCodeRegex.IsMatch(code.Trim());
    }

    /// <summary>
    /// Validates that a quantity value is a positive integer within an
    /// acceptable range for a cart or order line item.
    /// </summary>
    /// <param name="quantity">The quantity to validate.</param>
    /// <param name="maxAllowed">
    /// The maximum quantity allowed per line item. Defaults to 999.
    /// </param>
    /// <returns>
    /// <c>true</c> if <paramref name="quantity"/> is between 1 and
    /// <paramref name="maxAllowed"/> inclusive; <c>false</c> otherwise.
    /// </returns>
    public static bool IsValidQuantity(int quantity, int maxAllowed = 999)
    {
        return quantity >= 1 && quantity <= maxAllowed;
    }
}