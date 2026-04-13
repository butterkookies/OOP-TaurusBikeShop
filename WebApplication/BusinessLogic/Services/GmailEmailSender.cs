// WebApplication/BusinessLogic/Services/GmailEmailSender.cs

using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using WebApplication.BusinessLogic.Interfaces;
using WebApplication.Models;

namespace WebApplication.BusinessLogic.Services;

/// <summary>
/// Sends emails via Gmail SMTP using MailKit.
/// Configured through <see cref="SmtpSettings"/> (bound from appsettings "SmtpSettings" section).
/// <para>
/// <b>Authentication:</b> Uses a Gmail App Password with STARTTLS on port 587.
/// Regular Gmail passwords will not work — 2FA must be enabled and an App Password generated.
/// </para>
/// </summary>
public sealed class GmailEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<GmailEmailSender> _logger;

    public GmailEmailSender(IOptions<SmtpSettings> settings, ILogger<GmailEmailSender> logger)
    {
        _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger   = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public async Task SendAsync(string toEmail, string subject, string htmlBody, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(toEmail))
            throw new ArgumentException("Recipient email must not be empty.", nameof(toEmail));

        MimeMessage message = new();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject ?? "(No Subject)";

        BodyBuilder bodyBuilder = new()
        {
            HtmlBody = htmlBody
        };
        message.Body = bodyBuilder.ToMessageBody();

        using SmtpClient client = new();

        try
        {
            await client.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, cancellationToken);
            await client.AuthenticateAsync(_settings.SenderEmail, _settings.Password, cancellationToken);
            await client.SendAsync(message, cancellationToken);

            _logger.LogInformation("Email sent to {Recipient} — Subject: {Subject}", toEmail, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient} — Subject: {Subject}", toEmail, subject);
            throw;
        }
        finally
        {
            await client.DisconnectAsync(quit: true, cancellationToken);
        }
    }
}
