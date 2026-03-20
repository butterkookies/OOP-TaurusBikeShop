namespace WebApplication.BusinessLogic.Interfaces
{
    public interface IEmailService
    {
        Task SendOtpAsync(string toEmail, string otp);
    }
}
