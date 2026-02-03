using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class VerifyResetCodeModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<VerifyResetCodeModel> _logger;

    public VerifyResetCodeModel(
        IHttpClientFactory httpClientFactory,
        ILogger<VerifyResetCodeModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Email { get; set; }

    public string? ErrorMessage { get; set; }

    public class InputModel
    {
        [Required(ErrorMessage = "Mã xác thực là bắt buộc")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Mã xác thực phải có đúng 6 ký tự")]
        [Display(Name = "Mã xác thực")]
        public string Code { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(Email))
        {
            return RedirectToPage("/Auth/ForgotPassword");
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email))
        {
            return RedirectToPage("/Auth/ForgotPassword");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var verifyData = new
            {
                Email = Email,
                Code = Input.Code.ToUpper()
            };

            var content = new StringContent(
                JsonSerializer.Serialize(verifyData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/verify-reset-code", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                _logger.LogInformation("Reset code verified for {Email}", Email);
                
                // Redirect to reset password page with token
                return RedirectToPage("/Auth/ResetPassword", new { token = Input.Code.ToUpper() });
            }
            else
            {
                ErrorMessage = result?.Message ?? "Mã xác thực không hợp lệ hoặc đã hết hạn";
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during verify reset code for {Email}", Email);
            ErrorMessage = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return Page();
        }
    }

    public async Task<IActionResult> OnPostResendCodeAsync()
    {
        if (string.IsNullOrEmpty(Email))
        {
            return RedirectToPage("/Auth/ForgotPassword");
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var forgotPasswordData = new
            {
                Email = Email
            };

            var content = new StringContent(
                JsonSerializer.Serialize(forgotPasswordData),
                Encoding.UTF8,
                "application/json");

            await client.PostAsync("auth/forgot-password", content);
            
            _logger.LogInformation("Reset code resent for {Email}", Email);

            TempData["SuccessMessage"] = "Đã gửi lại mã xác thực!";
            return RedirectToPage("/Auth/VerifyResetCode", new { email = Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resending code for {Email}", Email);
            ErrorMessage = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return Page();
        }
    }
}
