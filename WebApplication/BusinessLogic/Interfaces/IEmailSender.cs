// WebApplication/BusinessLogic/Interfaces/IEmailSender.cs

namespace WebApplication.BusinessLogic.Interfaces;

/// <summary>
/// Contract for sending emails via SMTP.
/// Implementations handle the actual dispatch to an SMTP gateway (e.g. Gmail).
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends a single email message.
    /// </summary>
    /// <param name="toEmail">Recipient email address.</param>
    /// <param name="subject">Email subject line.</param>
    /// <param name="htmlBody">Email body (HTML supported).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default);
}
