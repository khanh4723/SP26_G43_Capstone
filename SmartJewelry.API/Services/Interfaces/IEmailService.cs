namespace SmartJewelry.API.Services.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true);
    Task<bool> SendPasswordResetEmailAsync(string toEmail, string username, string resetLink, string token);
    Task<bool> SendWelcomeEmailAsync(string toEmail, string username);
    Task<bool> SendEmailVerificationAsync(string toEmail, string username, string verificationLink, string token);
    Task<bool> SendPasswordChangedNotificationAsync(string toEmail, string username);
}
