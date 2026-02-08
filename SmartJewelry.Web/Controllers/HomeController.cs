using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartJewelry.Web.Controllers;

// Guest có thể vào xem, chỉ khi mua hàng mới cần đăng nhập
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    // GET: /Home/Index - Trang home cho tất cả mọi người
    public IActionResult Index()
    {
        return View();
    }
}
