using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
