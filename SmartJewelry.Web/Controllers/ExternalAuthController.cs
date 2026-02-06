using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using System.Text;
using SmartJewelry.Web.Models;

namespace SmartJewelry.Web.Controllers;

public class ExternalAuthController : Controller
{
    private readonly ILogger<ExternalAuthController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;

    public ExternalAuthController(ILogger<ExternalAuthController> logger, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    // GET: /ExternalAuth/Login?provider=Google
    [HttpGet("/ExternalAuth/Login")]
    public IActionResult Login(string provider, string? returnUrl = null)
    {
        // Validate provider
        if (string.IsNullOrEmpty(provider) || (provider != "Google" && provider != "Facebook"))
        {
            TempData["ErrorMessage"] = "Provider không hợp lệ";
            return RedirectToAction("Login", "Auth");
        }

        // Store returnUrl in TempData
        if (!string.IsNullOrEmpty(returnUrl))
        {
            TempData["ReturnUrl"] = returnUrl;
        }

        // Configure the redirect URL and challenge
        var authenticationProperties = new AuthenticationProperties
        {
            RedirectUri = Url.Action("Callback", "ExternalAuth", new { provider }),
            Items =
            {
                { "scheme", provider }
            }
        };

        return Challenge(authenticationProperties, provider);
    }

    // GET: /ExternalAuth/Callback?provider=Google
    [HttpGet("/ExternalAuth/Callback")]
    public async Task<IActionResult> Callback(string provider)
    {
        try
        {
            // Authenticate with the external provider
            var authenticateResult = await HttpContext.AuthenticateAsync(provider);

            if (!authenticateResult.Succeeded)
            {
                _logger.LogWarning("External authentication failed for provider: {Provider}", provider);
                TempData["ErrorMessage"] = "Đăng nhập thất bại. Vui lòng thử lại.";
                return RedirectToAction("Login", "Auth");
            }

            // Get user info from claims
            var claims = authenticateResult.Principal.Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            var nameIdentifier = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(nameIdentifier))
            {
                _logger.LogWarning("Missing required claims from {Provider}", provider);
                TempData["ErrorMessage"] = "Không thể lấy thông tin từ " + provider;
                return RedirectToAction("Login", "Auth");
            }

            // Get access token from the authentication result
            var accessToken = authenticateResult.Properties?.GetTokenValue("access_token");

            if (string.IsNullOrEmpty(accessToken))
            {
                _logger.LogWarning("No access token received from {Provider}", provider);
                TempData["ErrorMessage"] = "Không nhận được access token từ " + provider;
                return RedirectToAction("Login", "Auth");
            }

            // Call our API to handle social login
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            var socialLoginDto = new
            {
                Provider = provider,
                AccessToken = accessToken,
                Email = email,
                FullName = name
            };

            var content = new StringContent(
                JsonSerializer.Serialize(socialLoginDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("Auth/social-login", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (result?.Success == true && result.Data?.Token != null)
                {
                    // Log token information for debugging
                    var hasToken = !string.IsNullOrEmpty(result.Data.Token.AccessToken);
                    _logger.LogInformation("Social login response - Has Token: {HasToken}", hasToken);
                    
                    if (hasToken)
                    {
                        _logger.LogInformation("Social login - Token (first 20 chars): {Token}", 
                            result.Data.Token.AccessToken.Substring(0, Math.Min(20, result.Data.Token.AccessToken.Length)));
                    }

                    // Create authentication cookie for the web app
                    var userId = result.Data.User?.UserId;
                    var userName = !string.IsNullOrEmpty(result.Data.User?.Username) ? result.Data.User?.Username : result.Data.User?.FullName;
                    
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userId?.ToString() ?? ""),
                        new Claim(ClaimTypes.Email, result.Data.User?.Email ?? ""),
                        new Claim(ClaimTypes.Name, userName ?? "")
                    };

                    // Add AccessToken to claims (IMPORTANT!)
                    if (!string.IsNullOrEmpty(result.Data.Token.AccessToken))
                    {
                        userClaims.Add(new Claim("AccessToken", result.Data.Token.AccessToken));
                        _logger.LogInformation("Added AccessToken claim for social login");
                    }

                    if (!string.IsNullOrEmpty(result.Data.Token.RefreshToken))
                    {
                        userClaims.Add(new Claim("RefreshToken", result.Data.Token.RefreshToken));
                    }

                    // Add roles
                    if (result.Data.User?.Roles != null && result.Data.User.Roles.Any())
                    {
                        foreach (var role in result.Data.User.Roles)
                        {
                            userClaims.Add(new Claim(ClaimTypes.Role, role));
                        }
                    }
                    else if (!string.IsNullOrEmpty(result.Data.User?.Role))
                    {
                        userClaims.Add(new Claim(ClaimTypes.Role, result.Data.User.Role));
                    }

                    _logger.LogInformation("Total claims created for social login: {Count}", userClaims.Count);

                    var claimsIdentity = new ClaimsIdentity(userClaims, "Cookies");
                    var principal = new ClaimsPrincipal(claimsIdentity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = result.Data.Token.ExpiresAt
                    };

                    await HttpContext.SignInAsync("Cookies", principal, authProperties);

                    _logger.LogInformation("User {Email} logged in via {Provider} with {ClaimCount} claims", 
                        result.Data.User?.Email, provider, userClaims.Count);

                    // Get return URL - redirect to HomePage after social login
                    var returnUrl = TempData["ReturnUrl"] as string ?? Url.Content("~/");
                    return Redirect(returnUrl);
                }
            }

            _logger.LogError("Social login API call failed: {Response}", responseContent);
            TempData["ErrorMessage"] = "Đăng nhập thất bại. Vui lòng thử lại.";
            return RedirectToAction("Login", "Auth");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during external authentication callback");
            TempData["ErrorMessage"] = "Có lỗi xảy ra. Vui lòng thử lại.";
            return RedirectToAction("Login", "Auth");
        }
    }
}
