using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.Web.Models;
using SmartJewelry.Web.ViewModels.Profile;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace SmartJewelry.Web.Controllers;

[Authorize]
public class ProfileController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProfileController> _logger;
    private readonly string _apiBaseUrl;

    public ProfileController(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ProfileController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _logger = logger;
        _apiBaseUrl = _configuration["AppSettings:ApiBaseUrl"] ?? "https://localhost:5001";
    }

    private string? GetAccessToken()
    {
        // Get token from claims (stored during login)
        var tokenClaim = User.FindFirst("AccessToken");
        return tokenClaim?.Value;
    }

    // GET: /Profile
    public async Task<IActionResult> Index()
    {
        try
        {
            // Debug: Check if user is authenticated
            _logger.LogInformation("Profile Index - User authenticated: {IsAuthenticated}", User.Identity?.IsAuthenticated);
            _logger.LogInformation("Profile Index - User name: {UserName}", User.Identity?.Name);
            
            var token = GetAccessToken();
            _logger.LogInformation("Profile Index - Token retrieved: {HasToken}", !string.IsNullOrEmpty(token));
            
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Profile Index - No token found, redirecting to login");
                TempData["ErrorMessage"] = "Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            _logger.LogInformation("Profile Index - Calling API: {ApiUrl}", $"{_apiBaseUrl}/api/Profile");
            var response = await client.GetAsync($"{_apiBaseUrl}/api/Profile");
            
            _logger.LogInformation("Profile Index - API response status: {StatusCode}", response.StatusCode);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Profile Index - API response content length: {Length}", content.Length);
                
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProfileViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    _logger.LogInformation("Profile Index - Successfully loaded profile");
                    return View(apiResponse.Data);
                }
                else
                {
                    _logger.LogWarning("Profile Index - API returned unsuccessful response: {Message}", apiResponse?.Message);
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Profile Index - Unauthorized, redirecting to login");
                TempData["ErrorMessage"] = "Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToAction("Login", "Auth");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Profile Index - API error: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
            }

            TempData["ErrorMessage"] = "Không thể lấy thông tin hồ sơ";
            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile");
            TempData["ErrorMessage"] = $"Có lỗi xảy ra khi tải thông tin hồ sơ: {ex.Message}";
            return RedirectToAction("Index", "Home");
        }
    }

    // GET: /Profile/Edit
    public async Task<IActionResult> Edit()
    {
        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_apiBaseUrl}/api/Profile");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProfileViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    var viewModel = new UpdateProfileViewModel
                    {
                        Username = apiResponse.Data.Username,
                        Phone = apiResponse.Data.Phone,
                        DateOfBirth = apiResponse.Data.DateOfBirth,
                        Gender = apiResponse.Data.Gender
                    };
                    return View(viewModel);
                }
            }

            TempData["ErrorMessage"] = "Không thể lấy thông tin hồ sơ";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading profile for edit");
            TempData["ErrorMessage"] = "Có lỗi xảy ra";
            return RedirectToAction("Index");
        }
    }

    // POST: /Profile/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateProfileViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{_apiBaseUrl}/api/Profile", content);
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Cập nhật thông tin thành công";
                return RedirectToAction("Index");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            TempData["ErrorMessage"] = "Cập nhật thông tin thất bại";
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating profile");
            TempData["ErrorMessage"] = "Có lỗi xảy ra";
            return View(model);
        }
    }

    // GET: /Profile/Addresses
    public async Task<IActionResult> Addresses()
    {
        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync($"{_apiBaseUrl}/api/Profile");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<ProfileViewModel>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Success == true && apiResponse.Data != null)
                {
                    return View(apiResponse.Data.Addresses);
                }
            }

            TempData["ErrorMessage"] = "Không thể lấy danh sách địa chỉ";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading addresses");
            TempData["ErrorMessage"] = "Có lỗi xảy ra";
            return RedirectToAction("Index");
        }
    }

    // POST: /Profile/AddAddress
    [HttpPost]
    public async Task<IActionResult> AddAddress([FromBody] AddressViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var errorMessage = errors.Any() ? string.Join(", ", errors) : "Dữ liệu không hợp lệ";
            return Json(new { success = false, message = errorMessage });
        }

        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/api/Profile/addresses", content);
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Thêm địa chỉ thành công" });
            }

            return Json(new { success = false, message = "Thêm địa chỉ thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding address");
            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }
    }

    // POST: /Profile/UpdateAddress
    [HttpPost]
    public async Task<IActionResult> UpdateAddress(string addressId, [FromBody] AddressViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            var errorMessage = errors.Any() ? string.Join(", ", errors) : "Dữ liệu không hợp lệ";
            return Json(new { success = false, message = errorMessage });
        }

        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PutAsync($"{_apiBaseUrl}/api/Profile/addresses/{addressId}", content);
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Cập nhật địa chỉ thành công" });
            }

            return Json(new { success = false, message = "Cập nhật địa chỉ thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating address");
            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }
    }

    // POST: /Profile/DeleteAddress
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAddress(string addressId)
    {
        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync($"{_apiBaseUrl}/api/Profile/addresses/{addressId}");
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Xóa địa chỉ thành công" });
            }

            return Json(new { success = false, message = "Xóa địa chỉ thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting address");
            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }
    }

    // POST: /Profile/SetDefaultAddress
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetDefaultAddress(string addressId)
    {
        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return Json(new { success = false, message = "Phiên đăng nhập hết hạn" });

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync($"{_apiBaseUrl}/api/Profile/addresses/{addressId}/set-default", null);
            
            if (response.IsSuccessStatusCode)
            {
                return Json(new { success = true, message = "Đặt địa chỉ mặc định thành công" });
            }

            return Json(new { success = false, message = "Đặt địa chỉ mặc định thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting default address");
            return Json(new { success = false, message = "Có lỗi xảy ra" });
        }
    }

    // GET: /Profile/ChangePassword
    public IActionResult ChangePassword()
    {
        return View(new ChangePasswordViewModel());
    }

    // POST: /Profile/ChangePassword
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var token = GetAccessToken();
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonSerializer.Serialize(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"{_apiBaseUrl}/api/Profile/change-password", content);
            
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đổi mật khẩu thành công";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Đổi mật khẩu thất bại. Vui lòng kiểm tra lại mật khẩu hiện tại";
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error changing password");
            TempData["ErrorMessage"] = "Có lỗi xảy ra";
            return View(model);
        }
    }
}
