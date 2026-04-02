using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Error;
using System.Diagnostics;

namespace Presentation.WebApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        ViewData["Title"] = "Home";

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
