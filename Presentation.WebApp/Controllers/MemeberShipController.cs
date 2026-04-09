using Application.Abstractions.Identity;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Application.Users.Abstractions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Memberships;
using Presentation.WebApp.Models.Users;

namespace Presentation.WebApp.Controllers
{
    public class MemeberShipController(IMembershipService service, IUpdateMembershipService updateservice, UserManager<ApplicationUser> userManager) : Controller
    {
        public async Task<IActionResult> Index()
        {

            var memberships = await service.GetMembershipsAsync();
            var model = new MembershipViewModel()
            {
                MembershipIDs = memberships.Select(m => m.Id),
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
            return RedirectToAction("ConnectMembership");
        }

        [HttpPost("ConnectToUser")]
        public async Task<IActionResult> ConnectMembershiptoUser(MyAccountViewModel viewModel, string id, CancellationToken ct = default)
        {
            var user = await userManager.GetUserAsync(User);
            if(user == null)
            {
                return NotFound();
            }

            var membership = await service.GetMembershipByIdAsync(id);
            if(membership == null)
            {
                return NotFound();
            }

            var entity = new IConnectMembershipWithUserInput
            (
            user.Id,
            membership.Id
            );

            var result = await updateservice.ConnectMembershipWithUserAsync(entity);
            if (!result)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "MemeberShip");
        }

        public IActionResult MembershipStatus()
        {
            return View();
        }
    }
}
