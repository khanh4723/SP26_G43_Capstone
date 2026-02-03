using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class ExternalLoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(IHttpClientFactory httpClientFactory, ILogger<ExternalLoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public string? Provider { get; set; }
    public string? ErrorMessage { get; set; }
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet(string provider, string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(provider))
        {
            ErrorMessage = "Provider không được chỉ định";
            return Page();
        }

        Provider = provider;
        ReturnUrl = returnUrl ?? Url.Content("~/");

        // Redirect to external authentication provider
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl = ReturnUrl });
        var properties = new AuthenticationProperties 
        { 
            RedirectUri = redirectUrl,
            Items =
            {
                { "LoginProvider", provider }
            }
        };
        
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!string.IsNullOrEmpty(remoteError))
        {
            ErrorMessage = $"Lỗi từ nhà cung cấp: {remoteError}";
            return Page();
        }

        // Get the external login info
        var authenticateResult = await HttpContext.AuthenticateAsync();
        if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
        {
            ErrorMessage = "Không thể xác thực với nhà cung cấp bên ngoài";
            return Page();
        }

        var externalUser = authenticateResult.Principal;
        var provider = authenticateResult.Properties?.Items["LoginProvider"] ?? "External";
        Provider = provider;

        // Get user info from external provider
        var email = externalUser.FindFirstValue(ClaimTypes.Email);
        var name = externalUser.FindFirstValue(ClaimTypes.Name);
        var providerId = externalUser.FindFirstValue(ClaimTypes.NameIdentifier);

        // Get access token from the external provider
        var accessToken = await HttpContext.GetTokenAsync("access_token");

        if (string.IsNullOrEmpty(email))
        {
            ErrorMessage = "Không thể lấy email từ tài khoản của bạn. Vui lòng đảm bảo đã cấp quyền truy cập email.";
            return Page();
        }

        _logger.LogInformation("External login: Provider={Provider}, Email={Email}, Name={Name}", provider, email, name);

        try
        {
            // Call API to register/login user
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var socialLoginData = new
            {
                Provider = provider,
                AccessToken = accessToken ?? providerId ?? Guid.NewGuid().ToString(), // Use providerId as fallback
                Email = email,
                FullName = name ?? email,
                ProviderUserId = providerId
            };

            var content = new StringContent(
                JsonSerializer.Serialize(socialLoginData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/social-login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("API Response: {StatusCode} - {Content}", response.StatusCode, responseContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true && result.Data?.User != null)
            {
                // Create local authentication cookie with user info from API
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.User.UserID.ToString()),
                    new Claim(ClaimTypes.Email, result.Data.User.Email),
                    new Claim(ClaimTypes.Name, result.Data.User.FullName),
                    new Claim("AccessToken", result.Data.Token?.AccessToken ?? ""),
                    new Claim("RefreshToken", result.Data.Token?.RefreshToken ?? ""),
                    new Claim("Provider", provider)
                };

                // Add roles
                foreach (var role in result.Data.User.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = result.Data.Token?.ExpiresAt ?? DateTimeOffset.UtcNow.AddDays(7)
                });

                _logger.LogInformation("User {Email} logged in successfully with {Provider}", email, provider);

                return LocalRedirect(ReturnUrl);
            }
            else
            {
                // API call failed, but we still have user info from external provider
                // Create a basic local session
                _logger.LogWarning("API social login failed: {Message}. Creating local session.", result?.Message);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, name ?? email),
                    new Claim("Provider", provider),
                    new Claim("ProviderId", providerId ?? "")
                };

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
                });

                return LocalRedirect(ReturnUrl);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during social login API call");
            
            // Fallback: Create local session with external provider info
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Name, name ?? email),
                new Claim("Provider", provider),
                new Claim("ProviderId", providerId ?? "")
            };

            var identity = new ClaimsIdentity(claims, "Cookies");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("Cookies", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            });

            return LocalRedirect(ReturnUrl);
        }
    }
}
