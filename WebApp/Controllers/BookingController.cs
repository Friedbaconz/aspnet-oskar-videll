using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
