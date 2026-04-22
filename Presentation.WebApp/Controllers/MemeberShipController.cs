using Application.Abstractions.Identity;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Memberships.Inputs;
using Application.Users.Abstractions;
using Application.Users.Services;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Memberships;
using Presentation.WebApp.Models.Users;
using System.Net.NetworkInformation;

namespace Presentation.WebApp.Controllers
{
    public class MemeberShipController(IDeleteMembershipService deleteMembershipService,IRegisterMembershipService registerMembership,IMembershipService service, IUpdateMembershipService updateservice, UserManager<ApplicationUser> userManager, IBenefitService benefitService) : Controller
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

        [HttpPost("ConnectToUser")]
        public async Task<IActionResult> ConnectMembershiptoUser(string id, CancellationToken ct = default)
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

            var benefitlist = new List<UpdateMembershipBenefitInput>();

            var entity = new UpdateMembershipInput
            (
            membership.Id,
            membership.Name,
            membership.Description,
            null,
            membership.Status,
            membership.Type,
            membership.Pricing,
            membership.MonthlyDuration,
            user.Id,
            membership.Users
            );

            foreach (var benefitid in membership.Benefits)
            {
                var exsist = await benefitService.GetBenefitByIdAsync(benefitid.Id, ct);

                if (exsist.Id == benefitid.Id)
                {
                    entity.benefits.ToList().Add(new UpdateMembershipBenefitInput
                     (
                      exsist.Id,
                      exsist.Benefit,
                      exsist.MembershipId
                     ));
                }

            }

            var result = await updateservice.ExecuteAsync(entity, ct);

            if (!result.Success)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "MemeberShip");
        }

        [HttpPost("UpdateMembership")]
        public async Task<IActionResult> UpdateMembership(MyAccountViewModel form, string id, CancellationToken ct = default)
        {
            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var update = await service.GetMembershipByIdAsync(id, ct);

            var benefitlist = new List<UpdateMembershipBenefitInput>();

            foreach ( var benefitid in form.MyMembershipViewModel.MembershipViewModel.UpdateMembershipForm.Benefits)
            {
                if (benefitid.id == null)
                {
                    var it = new UpdateMembershipBenefitInput
                    (
                        id: string.Empty,
                        benefit: benefitid.benefit,
                        membershipId: update.Id
                    );

                    benefitlist.Add(it);
                }
                else
                {
                    var exsist = await benefitService.GetBenefitByIdAsync(benefitid.id, ct);

                    if (exsist.Id != null)
                    {

                        var it = new UpdateMembershipBenefitInput
                        (
                        id: benefitid.id,
                        benefit: exsist.Benefit,
                        membershipId: update.Id
                        );

                        benefitlist.Add(it);
                    }
                }
            }

            var membership = new UpdateMembershipInput
                    (
                        id: update.Id,
                        name: form.MyMembershipViewModel.MembershipViewModel.UpdateMembershipForm.MembershipName,
                        description: form.MyMembershipViewModel.MembershipViewModel.UpdateMembershipForm.description,
                        benefits: benefitlist,
                        status: update.Status,
                        type: update.Type,
                        pricing: form.MyMembershipViewModel.MembershipViewModel.UpdateMembershipForm.pricing,
                        monthlyDuration: form.MyMembershipViewModel.MembershipViewModel.UpdateMembershipForm.monthlyDuration,
                        userid: update.Userid,
                        users: update.Users
                    );

            var result = await updateservice.ExecuteAsync(membership);

            if (!result.Success)
            {
                return BadRequest();
            }

            return RedirectToAction("MyMembership", "My");
        }


        [HttpPost("DeleteMembership")]

        public async Task<IActionResult> DeleteMembership( string id, CancellationToken ct = default)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var membership = await service.GetMembershipByIdAsync(id);
            if (membership == null)
            {
                return NotFound();
            }

            var status = "Delete";

            var entity = new UpdateMembershipInput
            (
            membership.Id,
            membership.Name,
            membership.Description,
            null,
            status,
            membership.Type,
            membership.Pricing,
            membership.MonthlyDuration,
            user.Id,
            membership.Users
            );

            foreach (var benefitid in membership.Benefits)
            {
                var exsist = await benefitService.GetBenefitByIdAsync(benefitid.Id, ct);

                if (exsist.Id == benefitid.Id)
                {
                    entity.benefits.ToList().Add(new UpdateMembershipBenefitInput
                     (
                      exsist.Id,
                      exsist.Benefit,
                      exsist.MembershipId
                     ));
                }

            }

            var result = await updateservice.ExecuteAsync(entity, ct);
            if (!result.Success)
            {
                return BadRequest();
            }

            return RedirectToAction("MyMembership", "My");
        }

        [HttpPost("RemoveMembership")]
        public async Task<IActionResult> RemoveMembership(string id, CancellationToken ct = default)
        {

            var result = await deleteMembershipService.ExecuteAsync(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
                return View();
            }

            return View("Index", "Home");

        }

        [HttpPost("CreateMembership")]
        public async Task<IActionResult> CreateMembership(MyAccountViewModel form, CancellationToken ct = default)
        {
            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var membership = new RegisterMemebershipInput
                    (
                        name: form.MyMembershipViewModel.MembershipViewModel.MembershipForm.MembershipName,
                        description: form.MyMembershipViewModel.MembershipViewModel.MembershipForm.description,
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "Monthly",
                        pricing: form.MyMembershipViewModel.MembershipViewModel.MembershipForm.pricing,
                        monthlyDuration: form.MyMembershipViewModel.MembershipViewModel.MembershipForm.monthlyDuration
                    );
            foreach (var benefit in form.MyMembershipViewModel.MembershipViewModel.MembershipForm.Benefits) {
                membership.benefits.Add(
                new RegisterBenfitsInput
                (
                benefit: benefit.benefit
                ));
            }

            var result = await registerMembership.ExecuteAsync(membership);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
                return View(form);
            }

            return RedirectToAction("MyMembership", "My");
        }
    }
}
