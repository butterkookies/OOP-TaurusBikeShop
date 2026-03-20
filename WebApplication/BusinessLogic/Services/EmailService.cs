using System.Net;
using System.Net.Mail;
using WebApplication.BusinessLogic.Interfaces;

namespace WebApplication.BusinessLogic.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var host     = _config["Email:SmtpHost"]     ?? "smtp.gmail.com";
            var port     = int.Parse(_config["Email:SmtpPort"] ?? "587");
            var sender   = _config["Email:SenderEmail"]  ?? string.Empty;
            var password = _config["Email:SenderPassword"] ?? string.Empty;

            var subject = "Your Taurus Bike Shop Verification Code";
            var body    = $@"
<!DOCTYPE html>
<html>
<body style=""margin:0;padding:0;background:#0B0B0B;font-family:'Segoe UI',Arial,sans-serif;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"">
    <tr>
      <td align=""center"" style=""padding:40px 20px;"">
        <table width=""480"" cellpadding=""0"" cellspacing=""0""
               style=""background:#111111;border:1px solid #2A2A2A;border-radius:8px;"">
          <tr>
            <td style=""padding:32px 40px;border-bottom:1px solid #2A2A2A;"">
              <span style=""font-size:22px;font-weight:700;color:#E10600;letter-spacing:3px;"">
                ▲ TAURUS
              </span>
              <span style=""font-size:13px;color:#888888;margin-left:8px;"">Bike Shop</span>
            </td>
          </tr>
          <tr>
            <td style=""padding:36px 40px;"">
              <p style=""margin:0 0 8px;color:#888888;font-size:13px;text-transform:uppercase;
                         letter-spacing:1px;"">Verification Code</p>
              <p style=""margin:0 0 28px;color:#F0F0F0;font-size:15px;line-height:1.6;"">
                Use the code below to complete your registration. It expires in
                <strong style=""color:#F0F0F0;"">5 minutes</strong>.
              </p>
              <div style=""background:#1A1A1A;border:1px solid #E1060040;border-radius:6px;
                           padding:20px;text-align:center;margin-bottom:28px;"">
                <span style=""font-size:40px;font-weight:700;letter-spacing:12px;color:#E10600;
                              font-family:'Courier New',monospace;"">{otp}</span>
              </div>
              <p style=""margin:0;color:#555555;font-size:12px;line-height:1.6;"">
                If you did not request this code, you can safely ignore this email.
              </p>
            </td>
          </tr>
          <tr>
            <td style=""padding:20px 40px;border-top:1px solid #2A2A2A;"">
              <p style=""margin:0;color:#444444;font-size:11px;"">
                &copy; Taurus Bike Shop &mdash; taurusbikeshop89@gmail.com
              </p>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
</html>";

            using var client = new SmtpClient(host, port)
            {
                Credentials    = new NetworkCredential(sender, password),
                EnableSsl      = true,
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            using var message = new MailMessage
            {
                From       = new MailAddress(sender, "Taurus Bike Shop"),
                Subject    = subject,
                Body       = body,
                IsBodyHtml = true
            };
            message.To.Add(toEmail);

            await client.SendMailAsync(message);
            _logger.LogInformation("[OTP] Sent to {Email}", toEmail);
        }
    }
}
