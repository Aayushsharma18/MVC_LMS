using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using firstmvc.Models;

namespace firstmvc.Controllers;

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

    public IActionResult HelloWorld()
    {
        var str = "Hello world message";
        return View("HElloWorld", str);
    }

    public IActionResult CAll()
    {
        var str = "This is call action method.";
        return View("Call",str);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
