using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.Web.Models;
using SmartJewelry.Web.ViewModels.Auth;

namespace SmartJewelry.Web.Controllers;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<AuthController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
    }

    // GET: /Auth/Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null, string? message = null)
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl ?? Url.Content("~/");
        
        if (!string.IsNullOrEmpty(message))
        {
            ViewData["SuccessMessage"] = message;
        }

        return View();
    }

    // POST: /Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        ViewData["ReturnUrl"] = model.ReturnUrl ?? Url.Content("~/");

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            var loginData = new
            {
                Email = model.Email,
                Password = model.Password,
                RememberMe = model.RememberMe
            };

            var content = new StringContent(
                JsonSerializer.Serialize(loginData),
                Encoding.UTF8,
                "application/json");

            _logger.LogInformation("Sending login request for email: {Email}", model.Email);
            
            var response = await client.PostAsync("auth/login", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
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
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = result.Data.Token?.ExpiresAt ?? DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync("Cookies", principal, authProperties);

                _logger.LogInformation("User {Email} logged in successfully", model.Email);
                
                return LocalRedirect(model.ReturnUrl ?? Url.Content("~/"));
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Email hoặc mật khẩu không chính xác";
                return View(model);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Error during login for {Email}", model.Email);
            ViewData["ErrorMessage"] = "Không thể kết nối đến server. Vui lòng kiểm tra API đang chạy.";
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for {Email}: {Message}", model.Email, ex.Message);
            ViewData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            return View(model);
        }
    }

    // GET: /Auth/Register
    [HttpGet]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Login"); // Redirect to HomePage
        }

        return View();
    }

    // POST: /Auth/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            var registerData = new
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = model.Password,
                ConfirmPassword = model.ConfirmPassword,
                PhoneNumber = model.PhoneNumber
            };

            var content = new StringContent(
                JsonSerializer.Serialize(registerData),
                Encoding.UTF8,
                "application/json");

            _logger.LogInformation("Sending registration request for email: {Email}", model.Email);
            
            var response = await client.PostAsync("auth/register", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<AuthResponse>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                _logger.LogInformation("User {Email} registered successfully", model.Email);
                
                return RedirectToAction(nameof(Login), new { 
                    message = "Đăng ký thành công! Vui lòng đăng nhập." 
                });
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Đăng ký thất bại. Vui lòng thử lại.";
                
                if (result?.Errors != null && result.Errors.Any())
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                
                return View(model);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP Error during registration for {Email}", model.Email);
            ViewData["ErrorMessage"] = "Không thể kết nối đến server. Vui lòng kiểm tra API đang chạy.";
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for {Email}: {Message}", model.Email, ex.Message);
            ViewData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            return View(model);
        }
    }

    // GET: /Auth/ForgotPassword
    [HttpGet]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: /Auth/ForgotPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            var content = new StringContent(
                JsonSerializer.Serialize(new { Email = model.Email }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/forgot-password", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                return RedirectToAction(nameof(VerifyResetCode), new { email = model.Email });
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Không thể gửi email. Vui lòng thử lại.";
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during forgot password for {Email}", model.Email);
            ViewData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            return View(model);
        }
    }

    // GET: /Auth/VerifyResetCode
    [HttpGet]
    public IActionResult VerifyResetCode(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction(nameof(ForgotPassword));
        }

        var model = new VerifyResetCodeViewModel { Email = email };
        return View(model);
    }

    // POST: /Auth/VerifyResetCode
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> VerifyResetCode(VerifyResetCodeViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            var content = new StringContent(
                JsonSerializer.Serialize(new { Email = model.Email, ResetCode = model.ResetCode }),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/verify-reset-code", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                return RedirectToAction(nameof(ResetPassword), new { 
                    email = model.Email, 
                    resetCode = model.ResetCode 
                });
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Mã xác nhận không hợp lệ.";
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during verify reset code for {Email}", model.Email);
            ViewData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            return View(model);
        }
    }

    // GET: /Auth/ResetPassword
    [HttpGet]
    public IActionResult ResetPassword(string email, string resetCode)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(resetCode))
        {
            return RedirectToAction(nameof(ForgotPassword));
        }

        var model = new ResetPasswordViewModel 
        { 
            Email = email, 
            ResetCode = resetCode 
        };
        
        return View(model);
    }

    // POST: /Auth/ResetPassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
            
            var resetData = new
            {
                Email = model.Email,
                ResetCode = model.ResetCode,
                NewPassword = model.NewPassword,
                ConfirmPassword = model.ConfirmPassword
            };

            var content = new StringContent(
                JsonSerializer.Serialize(resetData),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("auth/reset-password", content);
            var responseContent = await response.Content.ReadAsStringAsync();
            
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, options);

            if (response.IsSuccessStatusCode && result?.Success == true)
            {
                return RedirectToAction(nameof(Login), new { 
                    message = "Đặt lại mật khẩu thành công! Vui lòng đăng nhập." 
                });
            }
            else
            {
                ViewData["ErrorMessage"] = result?.Message ?? "Không thể đặt lại mật khẩu.";
                return View(model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during reset password for {Email}", model.Email);
            ViewData["ErrorMessage"] = $"Có lỗi xảy ra: {ex.Message}";
            return View(model);
        }
    }

    // GET: /Auth/Logout
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("Cookies");
        _logger.LogInformation("User logged out");
        return RedirectToAction("Index", "HomePage"); // Redirect to HomePage after logout
    }

    // GET: /Auth/AccessDenied
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }
}
