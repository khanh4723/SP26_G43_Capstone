using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SmartJewelry.Web.Pages.Auth;

public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;

    public LogoutModel(ILogger<LogoutModel> logger)
    {
        _logger = logger;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        return await PerformLogout();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return await PerformLogout();
    }

    private async Task<IActionResult> PerformLogout()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            _logger.LogInformation("User {Email} logged out", User.Identity.Name);
        }

        await HttpContext.SignOutAsync("Cookies");
        
        return RedirectToPage("/Auth/Login", new { message = "Bạn đã đăng xuất thành công" });
    }
}
