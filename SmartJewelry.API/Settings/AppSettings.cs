namespace SmartJewelry.API.Settings;

public class AppSettings
{
    public string FrontendUrl { get; set; } = string.Empty;
    public int PasswordResetTokenExpirationMinutes { get; set; } = 30;
    
    // Dev mode settings - Auto verify emails for testing
    public bool IsDevMode { get; set; } = false;
    public List<string> AutoVerifyEmails { get; set; } = new List<string>();
}
