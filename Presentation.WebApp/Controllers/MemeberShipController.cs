using Microsoft.AspNetCore.Mvc;

namespace Presentation.WebApp.Controllers
{
    public class MemeberShipController : Controller
    {
        public IActionResult NewMembership()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewMembership(string username)
        {
            return View();
        }

        public IActionResult ConnectMembership()
        {
            return View();
        }

        public IActionResult MembershipStatus()
        {
            return View();
        }
    }
}
