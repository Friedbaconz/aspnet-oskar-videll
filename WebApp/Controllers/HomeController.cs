using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index()
    {
        ViewData["Title"] = "Home";

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
