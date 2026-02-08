using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartJewelry.API.Services.Interfaces;
using SmartJewelry.API.Settings;

namespace SmartJewelry.API.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
        _emailSettings = emailSettings.Value;
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, bool isHtml = true)
    {
        try
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder();
            if (isHtml)
                builder.HtmlBody = body;
            else
                builder.TextBody = body;

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to {Email}", toEmail);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendPasswordResetEmailAsync(string toEmail, string username, string resetLink, string token)
    {
        var subject = "Smart Jewelry - Đặt lại mật khẩu";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💎 Smart Jewelry</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {username},</h2>
            <p>Chúng tôi nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.</p>
            
            <div style='background: #f8f9fa; padding: 20px; border-radius: 8px; margin: 20px 0;'>
                <p style='margin: 0 0 10px 0; font-weight: bold; color: #667eea;'>Mã xác thực của bạn:</p>
                <p style='text-align: center; margin: 0;'>
                    <span style='font-size: 24px; font-weight: bold; color: #667eea; letter-spacing: 2px;'>{token}</span>
                </p>
            </div>
            
            <p><strong>Làm theo các bước sau:</strong></p>
            <ol style='line-height: 1.8;'>
                <li>Quay lại trang web Smart Jewelry</li>
                <li>Nhập mã xác thực <strong style='color: #667eea;'>{token}</strong> vào trang</li>
                <li>Nhập mật khẩu mới và xác nhận</li>
            </ol>
            
            <p><strong>⏰ Lưu ý:</strong> Mã này sẽ hết hạn sau 30 phút.</p>
            <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
        </div>
        <div class='footer'>
            <p>© 2026 Smart Jewelry. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        return await SendEmailAsync(toEmail, subject, body);
    }

    public async Task<bool> SendWelcomeEmailAsync(string toEmail, string username)
    {
        var subject = "Chào mừng đến với Smart Jewelry! 💎";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .features {{ margin: 20px 0; }}
        .feature {{ padding: 10px 0; border-bottom: 1px solid #eee; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💎 Smart Jewelry</h1>
            <p>Chào mừng bạn đến với thế giới trang sức!</p>
        </div>
        <div class='content'>
            <h2>Xin chào {username}! 🎉</h2>
            <p>Cảm ơn bạn đã đăng ký tài khoản tại Smart Jewelry.</p>
            <p>Với tài khoản của mình, bạn có thể:</p>
            <div class='features'>
                <div class='feature'>💎 Khám phá kho đá quý với hàng nghìn viên đá được chứng nhận</div>
                <div class='feature'>💍 Tùy chỉnh trang sức theo phong cách riêng của bạn</div>
                <div class='feature'>📏 Lưu kích thước ni tay để đặt hàng dễ dàng hơn</div>
                <div class='feature'>🎁 Nhận voucher và ưu đãi độc quyền</div>
            </div>
            <p>Chúc bạn có trải nghiệm mua sắm tuyệt vời!</p>
        </div>
        <div class='footer'>
            <p>© 2026 Smart Jewelry. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        return await SendEmailAsync(toEmail, subject, body);
    }

    public async Task<bool> SendEmailVerificationAsync(string toEmail, string username, string verificationLink, string token)
    {
        var subject = "Smart Jewelry - Xác thực email";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .button {{ display: inline-block; background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 15px 30px; text-decoration: none; border-radius: 5px; margin: 20px 0; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💎 Smart Jewelry</h1>
        </div>
        <div class='content'>
            <h2>Xin chào {username},</h2>
            <p>Vui lòng xác thực địa chỉ email của bạn bằng cách nhấn vào nút bên dưới:</p>
            <p style='text-align: center;'>
                <a href='{verificationLink}' class='button'>Xác thực Email</a>
            </p>
            <p>Hoặc nhập mã xác thực: <strong style='font-size: 18px; color: #667eea;'>{token}</strong></p>
            <p><strong>Lưu ý:</strong> Link này sẽ hết hạn sau 24 giờ.</p>
            <p>Nếu bạn không yêu cầu xác thực, vui lòng bỏ qua email này.</p>
        </div>
        <div class='footer'>
            <p>© 2026 Smart Jewelry. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";

        return await SendEmailAsync(toEmail, subject, body);
    }

    public async Task<bool> SendPasswordChangedNotificationAsync(string toEmail, string username)
    {
        var subject = "Smart Jewelry - Mật khẩu đã được thay đổi";
        var body = $@"
<!DOCTYPE html>
<html>
<head>
    <meta charset='UTF-8'>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .content {{ background: #f9f9f9; padding: 30px; border-radius: 0 0 10px 10px; }}
        .alert {{ background: #fff3cd; border-left: 4px solid #ffc107; padding: 15px; margin: 20px 0; border-radius: 5px; }}
        .footer {{ text-align: center; margin-top: 20px; color: #666; font-size: 12px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>💎 Smart Jewelry</h1>
            <h2>🔐 Thông báo bảo mật</h2>
        </div>
        <div class='content'>
            <h2>Xin chào {username},</h2>
            <p>Mật khẩu của tài khoản <strong>{toEmail}</strong> đã được thay đổi thành công.</p>
            <div class='alert'>
                <p><strong>⚠️ Lưu ý bảo mật:</strong></p>
                <p>Nếu bạn KHÔNG thực hiện thay đổi này, vui lòng liên hệ với chúng tôi ngay lập tức để bảo vệ tài khoản của bạn.</p>
            </div>
            <p>Thời gian thay đổi: <strong>{DateTime.UtcNow.AddHours(7):dd/MM/yyyy HH:mm}</strong> (GMT+7)</p>
            <p>Tất cả các phiên đăng nhập trên các thiết bị khác đã bị đăng xuất để bảo mật.</p>
            <hr style='border: none; border-top: 1px solid #eee; margin: 20px 0;'>
            <p style='color: #666; font-size: 12px;'>
                Nếu bạn cần hỗ trợ, vui lòng liên hệ:<br>
                📧 Email: support@smartjewelry.com<br>
                📞 Hotline: 1900-xxxx
            </p>
        </div>
        <div class='footer'>
            <p>© 2026 Smart Jewelry. All rights reserved.</p>
            <p>Email này được gửi tự động, vui lòng không trả lời.</p>
        </div>
    </div>
</body>
</html>";

        return await SendEmailAsync(toEmail, subject, body);
    }
}
