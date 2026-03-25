using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class UserController : Controller
    {
        public IActionResult My()
        {
            return View();
        }
    }
}
