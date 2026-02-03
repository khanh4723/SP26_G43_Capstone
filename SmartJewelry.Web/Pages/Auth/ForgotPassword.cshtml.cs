using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class ForgotPasswordModel : PageModel
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ForgotPasswordModel> _logger;

    public ForgotPasswordModel(
        IHttpClientFactory httpClientFactory,
        ILogger<ForgotPasswordModel> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public string? ErrorMessage { get; set; }
    public bool EmailSent { get; set; } = false;

    public class InputModel
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;
    }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");

            var forgotPasswordData = new
            {
                Email = Input.Email
            };

            var content = new StringContent(
                JsonSerializer.Serialize(forgotPasswordData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/forgot-password", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, options);

            // Always show success to prevent email enumeration
            _logger.LogInformation("Password reset requested for {Email}", Input.Email);

            // Redirect to verify reset code page
            return RedirectToPage("/Auth/VerifyResetCode", new { email = Input.Email });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for {Email}", Input.Email);
            ErrorMessage = "Có lỗi xảy ra. Vui lòng thử lại sau.";
            return Page();
        }
    }
}
