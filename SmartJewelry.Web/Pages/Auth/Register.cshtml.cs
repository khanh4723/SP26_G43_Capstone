using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class RegisterModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<RegisterModel> _logger;

    public RegisterModel(
        IHttpClientFactory httpClientFactory,
        ILogger<RegisterModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public string? ReturnUrl { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(255, ErrorMessage = "Họ tên không được quá 255 ký tự")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bạn phải đồng ý với điều khoản dịch vụ")]
        [Display(Name = "Đồng ý điều khoản")]
        public bool AgreeTerms { get; set; }
    }

    public void OnGet(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!Input.AgreeTerms)
        {
            ModelState.AddModelError("Input.AgreeTerms", "Bạn phải đồng ý với điều khoản dịch vụ");
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var registerData = new
            {
                Email = Input.Email,
                Password = Input.Password,
                ConfirmPassword = Input.ConfirmPassword,
                FullName = Input.FullName,
                PhoneNumber = Input.PhoneNumber
            };

            var content = new StringContent(
                JsonSerializer.Serialize(registerData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/register", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true && result.Data?.User != null)
            {
                // Auto login after registration
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, result.Data.User.UserID.ToString()),
                    new Claim(ClaimTypes.Email, result.Data.User.Email),
                    new Claim(ClaimTypes.Name, result.Data.User.FullName),
                    new Claim("AccessToken", result.Data.Token?.AccessToken ?? ""),
                    new Claim("RefreshToken", result.Data.Token?.RefreshToken ?? "")
                };

                foreach (var role in result.Data.User.Roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var identity = new ClaimsIdentity(claims, "Cookies");
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = result.Data.Token?.ExpiresAt ?? DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("Cookies", principal, authProperties);

                _logger.LogInformation("New user registered: {Email}", Input.Email);

                return LocalRedirect(ReturnUrl);
            }
            else
            {
                ErrorMessage = result?.Message ?? "Đăng ký không thành công. Vui lòng thử lại.";
                
                if (result?.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}", Input.Email);
            ErrorMessage = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return Page();
        }
    }
}
