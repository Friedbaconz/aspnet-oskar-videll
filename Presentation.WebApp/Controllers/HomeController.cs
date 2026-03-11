using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        ViewData["Title"] = "Home test";

        return View();
    }

    public IActionResult MemberShips()
    {
        return View();
    }

    public IActionResult CostumerService()
    {
        return View(); 
    }

    public IActionResult MyAccount()
    {
        return View(); 
    }
}
