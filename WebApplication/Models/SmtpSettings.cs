// WebApplication/Models/SmtpSettings.cs

namespace WebApplication.Models;

/// <summary>
/// Configuration for the Gmail SMTP email sender.
/// Bound from the "SmtpSettings" section in appsettings.json.
/// <para>
/// <b>Gmail App Password:</b> Use a Gmail App Password (not your regular password).
/// Generate one at https://myaccount.google.com/apppasswords — requires 2FA enabled.
/// Store the password in User Secrets or environment variables, never in appsettings.json.
/// </para>
/// </summary>
public sealed class SmtpSettings
{
    /// <summary>SMTP server host (e.g. "smtp.gmail.com").</summary>
    public string Host { get; set; } = "smtp.gmail.com";

    /// <summary>SMTP server port. 587 for STARTTLS (recommended for Gmail).</summary>
    public int Port { get; set; } = 587;

    /// <summary>Gmail address used as the sender (e.g. "taurusbikeshop@gmail.com").</summary>
    public string SenderEmail { get; set; } = "taurusbikeshop89@gmail.com";

    /// <summary>Display name for the sender (e.g. "Taurus Bike Shop").</summary>
    public string SenderName { get; set; } = "Taurus Bike Shop";

    /// <summary>Gmail App Password for SMTP authentication.</summary>
    public string Password { get; set; } = "qvdl pltn fazd arnf";
}
