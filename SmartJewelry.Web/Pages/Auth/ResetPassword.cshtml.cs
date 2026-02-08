using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class ResetPasswordModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ResetPasswordModel> _logger;

    public ResetPasswordModel(
        IHttpClientFactory httpClientFactory,
        ILogger<ResetPasswordModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public string? Token { get; set; }

    public string? ErrorMessage { get; set; }
    public bool TokenValid { get; set; } = true;
    public bool ResetSuccess { get; set; } = false;

    public class InputModel
    {
        [Required(ErrorMessage = "Mật khẩu mới là bắt buộc")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất {2} ký tự", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    public IActionResult OnGet()
    {
        if (string.IsNullOrEmpty(Token))
        {
            TokenValid = false;
            return Page();
        }

        // Token is already validated in VerifyResetCode step
        TokenValid = true;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Token))
        {
            TokenValid = false;
            return Page();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var resetPasswordData = new
            {
                Token = Token,
                NewPassword = Input.NewPassword,
                ConfirmPassword = Input.ConfirmPassword
            };

            var content = new StringContent(
                JsonSerializer.Serialize(resetPasswordData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/reset-password", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                ResetSuccess = true;
                _logger.LogInformation("Password reset successful");
                return Page();
            }
            else
            {
                if (result?.Message?.Contains("hết hạn") == true || result?.Message?.Contains("không hợp lệ") == true)
                {
                    TokenValid = false;
                }
                else
                {
                    ErrorMessage = result?.Message ?? "Có lỗi xảy ra. Vui lòng thử lại.";
                }
                return Page();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during password reset");
            ErrorMessage = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return Page();
        }
    }
}
