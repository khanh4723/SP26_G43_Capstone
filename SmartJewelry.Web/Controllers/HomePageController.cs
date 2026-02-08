using Microsoft.AspNetCore.Mvc;

namespace SmartJewelry.Web.Controllers;

public class HomePageController : Controller
{
    private readonly ILogger<HomePageController> _logger;

    public HomePageController(ILogger<HomePageController> logger)
    {
        _logger = logger;
    }

    // GET: /HomePage/Index or /
    public IActionResult Index()
    {
        return View();
    }
}
