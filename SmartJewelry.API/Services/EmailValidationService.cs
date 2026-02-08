using System.Net;
using System.Net.Sockets;

namespace SmartJewelry.API.Services;

public interface IEmailValidationService
{
    Task<(bool IsValid, string? ErrorMessage)> ValidateEmailAsync(string email);
    bool IsDisposableEmail(string email);
}

public class EmailValidationService : IEmailValidationService
{
    private readonly ILogger<EmailValidationService> _logger;

    // Danh sách các domain email tạm thời phổ biến
    private static readonly HashSet<string> DisposableEmailDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        // Email tạm thời phổ biến
        "10minutemail.com", "10minutemail.net", "guerrillamail.com", "guerrillamail.net",
        "sharklasers.com", "grr.la", "guerrillamail.biz", "guerrillamail.de",
        "tempmail.com", "temp-mail.org", "throwaway.email", "getnada.com",
        "maildrop.cc", "mailinator.com", "trashmail.com", "fakeinbox.com",
        "yopmail.com", "yopmail.fr", "cool.fr.nf", "jetable.fr.nf",
        "courriel.fr.nf", "moncourrier.fr.nf", "monemail.fr.nf", "monmail.fr.nf",
        "hide.biz.st", "mymail.infos.st",
        
        // Thêm các domain khác
        "mailnesia.com", "emailondeck.com", "spambog.com", "spambog.de",
        "spambog.ru", "spam4.me", "mintemail.com", "mytrashmail.com",
        "mohmal.com", "throwawaymail.com", "anonbox.net", "anonymbox.com",
        "dispostable.com", "fakemailgenerator.com", "harakirimail.com",
        "jetable.org", "link2mail.net", "owlpic.com", "proxymail.eu",
        "safetymail.info", "spamex.com", "spamfree24.org", "spamgourmet.com",
        "spamspot.com", "tmpeml.info", "trbvm.com", "zehnminutenmail.de",
        
        // Các domain mới
        "33mail.com", "bugmenot.com", "deadaddress.com", "despam.it",
        "disposeamail.com", "dodgeit.com", "emailias.com", "emaillime.com",
        "emailsensei.com", "emailtemporanea.com", "emailtemporanea.net",
        "emailtemporario.com.br", "emailwarden.com", "ephemeral.email",
        "guerrillamailblock.com", "incognitomail.com", "mailcatch.com",
        "mailforspam.com", "mailfreeonline.com", "mailimate.com",
        "mailin8r.com", "mailmoat.com", "mailscrap.com", "mailshell.com",
        "mailsiphon.com", "mailslite.com", "mailtemp.info", "mailtothis.com",
        "no-spam.ws", "noclickemail.com", "nospamfor.us", "nwldx.com",
        "oneoffemail.com", "pookmail.com", "rejectmail.com", "rmqkr.net",
        "sharklasers.com", "shiftmail.com", "slaskpost.se", "sneakemail.com",
        "sogetthis.com", "soodonims.com", "spam.la", "spambox.us",
        "spamcowboy.com", "spamday.com", "spameater.com", "spamevader.com",
        "spamgourmet.org", "spamherelots.com", "spamhereplease.com",
        "spamhole.com", "spamify.com", "spaminator.de", "spamkill.info",
        "spaml.com", "spaml.de", "spammotel.com", "spamobox.com",
        "spamoff.de", "spamslicer.com", "spamthis.co.uk", "spamthisplease.com",
        "spamtrail.com", "speed.1s.fr", "supergreatmail.com", "suremail.info",
        "tagyourself.com", "tempe-mail.com", "tempemail.biz", "tempemail.com",
        "tempemail.net", "tempinbox.co.uk", "tempinbox.com", "tempmail.eu",
        "tempmail.it", "tempmail2.com", "tempomail.fr", "temporarily.de",
        "tempthe.net", "thankyou2010.com", "thisisnotmyrealemail.com",
        "thrma.com", "tilien.com", "tittbit.in", "tmail.ws", "tmailinator.com",
        "tradermail.info", "trash-amil.com", "trash-mail.at", "trash-mail.com",
        "trash-mail.de", "trash2009.com", "trashdevil.com", "trashemail.de",
        "trashmail.at", "trashmail.de", "trashmail.me", "trashmail.net",
        "trashmail.org", "trashmail.ws", "trashmailer.com", "trashymail.com",
        "trialmail.de", "trillianpro.com", "turual.com", "twinmail.de",
        "uggsrock.com", "upliftnow.com", "venompen.com", "veryrealemail.com",
        "viditag.com", "viewcastmedia.com", "viewcastmedia.net",
        "viewcastmedia.org", "webm4il.info", "wegwerfadresse.de",
        "wegwerfemail.com", "wegwerfemail.de", "wegwerfmail.de", "wegwerfmail.net",
        "wegwerfmail.org", "wh4f.org", "whyspam.me", "willselfdestruct.com",
        "winemaven.info", "wronghead.com", "wuzup.net", "wuzupmail.net",
        "www.e4ward.com", "www.gishpuppy.com", "www.mailinator.com",
        "wwwnew.eu", "x.ip6.li", "xagloo.com", "xemaps.com", "xents.com",
        "xmaily.com", "xoxy.net", "yapped.net", "yopmail.net", "you-spam.com",
        "yuurok.com", "zehnminuten.de", "zippymail.info", "zoaxe.com",
        "zoemail.com", "zomg.info"
    };

    // Danh sách domain được tin cậy (whitelist)
    private static readonly HashSet<string> TrustedEmailDomains = new(StringComparer.OrdinalIgnoreCase)
    {
        // Email providers lớn
        "gmail.com", "googlemail.com", "outlook.com", "hotmail.com", "live.com",
        "yahoo.com", "yahoo.co.uk", "yahoo.fr", "ymail.com",
        "icloud.com", "me.com", "mac.com",
        "aol.com", "protonmail.com", "proton.me", "tutanota.com",
        
        // Email doanh nghiệp VN
        "vietcombank.com.vn", "fpt.com.vn", "vnpt.vn", "viettel.com.vn",
        "vinaphone.com.vn", "mobifone.vn",
        
        // Các ISP và edu
        "edu", "ac.uk", "edu.vn"
    };

    public EmailValidationService(ILogger<EmailValidationService> logger)
    {
        _logger = logger;
    }

    public async Task<(bool IsValid, string? ErrorMessage)> ValidateEmailAsync(string email)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Email không được để trống");
            }

            // 1. Validate format cơ bản
            if (!IsValidEmailFormat(email))
            {
                return (false, "Định dạng email không hợp lệ");
            }

            var domain = email.Split('@')[1].ToLower();

            // 2. Check disposable email
            if (IsDisposableEmail(email))
            {
                _logger.LogWarning("Disposable email detected: {Email}", email);
                return (false, "Email tạm thời không được phép. Vui lòng sử dụng email thật.");
            }

            // 3. Check MX records (optional - có thể tốn thời gian)
            var hasMxRecord = await HasMxRecordAsync(domain);
            if (!hasMxRecord)
            {
                _logger.LogWarning("No MX record for domain: {Domain}", domain);
                return (false, "Domain email không tồn tại hoặc không thể nhận email");
            }

            return (true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating email: {Email}", email);
            // Nếu có lỗi, vẫn cho qua để không block user thật
            return (true, null);
        }
    }

    public bool IsDisposableEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
        {
            return false;
        }

        var domain = email.Split('@')[1].ToLower();

        // Check nếu nằm trong whitelist → cho qua
        if (TrustedEmailDomains.Contains(domain))
        {
            return false;
        }

        // Check nếu nằm trong blacklist → chặn
        return DisposableEmailDomains.Contains(domain);
    }

    private bool IsValidEmailFormat(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private async Task<bool> HasMxRecordAsync(string domain)
    {
        try
        {
            // Simple DNS check - kiểm tra domain có IP không
            var hostEntry = await Dns.GetHostEntryAsync(domain);
            return hostEntry.AddressList.Length > 0;
        }
        catch (SocketException)
        {
            // Domain không tồn tại
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error checking MX record for domain: {Domain}", domain);
            // Nếu có lỗi, cho qua để không block user thật
            return true;
        }
    }
}
