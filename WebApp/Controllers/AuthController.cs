using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

public class AuthController : Controller
{
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp(string username)
    {
        return View();
    }

    public IActionResult SignIn()
    {
        return View();
    }

    public new IActionResult SignOut()
    {
        return RedirectToAction("Index", "Home");
    }
}
