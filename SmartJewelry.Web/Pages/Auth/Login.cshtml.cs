using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class LoginModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LoginModel> _logger;

    public LoginModel(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<LoginModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }

    public void OnGet(string? returnUrl = null, string? message = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
        
        if (!string.IsNullOrEmpty(message))
        {
            SuccessMessage = message;
        }
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            _logger.LogInformation("Attempting to call API at: {BaseAddress}", client.BaseAddress);
            
            var loginData = new
            {
                Email = Input.Email,
                Password = Input.Password,
                RememberMe = Input.RememberMe
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            _logger.LogInformation("Sending login request for email: {Email}", Input.Email);
            
            var response = await client.PostAsync("auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            _logger.LogInformation("API Response Status: {StatusCode}, Content: {Content}", 
                response.StatusCode, responseContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true && result.Data?.User != null)
            {
                // Create claims for cookie authentication
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.User.UserID.ToString()),
                    new Claim(ClaimTypes.Email, result.Data.User.Email),
                    new Claim(ClaimTypes.Name, result.Data.User.FullName),
                    new Claim("AccessToken", result.Data.Token?.AccessToken ?? ""),
                    new Claim("RefreshToken", result.Data.Token?.RefreshToken ?? "")
                };

                // Add roles
                foreach (var role in result.Data.User.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = Input.RememberMe,
                    ExpiresUtc = result.Data.Token?.ExpiresAt ?? DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("Cookies", principal, authProperties);

                _logger.LogInformation("User {Email} logged in successfully", Input.Email);
                
                return LocalRedirect(ReturnUrl);
            }
            else
            {
                ErrorMessage = result?.Message ?? "Email hoặc mật khẩu không chính xác";
                return Page();
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Error during login for {Email}. Cannot connect to API.", Input.Email);
            ErrorMessage = "Không thể kết nối đến server. Vui lòng kiểm tra API đang chạy tại http://localhost:5000";
            return Page();
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout during login for {Email}", Input.Email);
            ErrorMessage = "Kết nối bị timeout. Vui lòng thử lại.";
            return Page();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}: {Message}", Input.Email, ex.Message);
            ErrorMessage = $"Có lỗi xảy ra: {ex.Message}";
            return Page();
        }
    }
}

// Response DTOs
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }
}

public class AuthResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public TokenInfo? Token { get; set; }
    public UserInfo? User { get; set; }
}

public class TokenInfo
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

public class UserInfo
{
    public int UserID { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public bool EmailVerified { get; set; }
    public List<string> Roles { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
