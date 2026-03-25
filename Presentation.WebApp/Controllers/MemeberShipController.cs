using Application.Memberships.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Memberships;

namespace Presentation.WebApp.Controllers
{
    public class MemeberShipController(IMembershipService service) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var memberships = await service.GetMembershipsAsync();
            var model = new MembershipViewModel()
            {
                Memberships = memberships
            };
            return View(model);
        }

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
