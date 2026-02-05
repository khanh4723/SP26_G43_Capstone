using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartJewelry.Web.Controllers;

[Authorize] // Chỉ user đã login mới vào được
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET: /Home/Index - Trang dành cho user đã đăng nhập
    public IActionResult Index()
    {
        return View();
    }
}
