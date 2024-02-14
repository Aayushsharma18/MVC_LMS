using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using firstmvc.Models;

namespace firstmvc.Controllers;

// [Route("[controller]/[action]")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult About()
    {
        var str = "Hello world message";
        return View("About", str);
    }

    public IActionResult Contact()
    {
        var str = "This is contact action method.";
        return View("Contact", str);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
