using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartJewelry.Web.Models;
using SmartJewelry.Web.ViewModels.Orders;

namespace SmartJewelry.Web.Controllers;

[Authorize(Roles = "sales_staff")]
public class SalesStaffController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<SalesStaffController> _logger;

    public SalesStaffController(IHttpClientFactory httpClientFactory, ILogger<SalesStaffController> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> Orders()
    {
        var accessToken = User.Claims.FirstOrDefault(c => c.Type == "AccessToken")?.Value;
        if (string.IsNullOrEmpty(accessToken))
        {
            TempData["ErrorMessage"] = "Bạn cần đăng nhập lại (không tìm thấy AccessToken).";
            return RedirectToAction("Login", "Auth");
        }

        var client = _httpClientFactory.CreateClient("SmartJewelryAPI");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        try
        {
            var resp = await client.GetAsync("orders/my");
            var json = await resp.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var api = JsonSerializer.Deserialize<ApiResponse<List<OrderListItemViewModel>>>(json, options);

            if (!resp.IsSuccessStatusCode || api?.Success != true || api.Data == null)
            {
                ViewData["ErrorMessage"] = api?.Message ?? "Không thể tải danh sách đơn hàng";
                return View(new List<OrderListItemViewModel>());
            }

            return View(api.Data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading staff orders");
            ViewData["ErrorMessage"] = "Có lỗi xảy ra khi gọi API.";
            return View(new List<OrderListItemViewModel>());
        }
    }
}
