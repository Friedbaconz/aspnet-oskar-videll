using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class ActivitiesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
