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

            var benefitlist = new List<UpdateMembershipBenefitInput>();
            var exsist = await benefitService.GetBenefitsAsync();

            foreach(var benefit in membership.Benefits)
            {
                foreach(var ex in exsist)
                {
                    if(ex.Benefit == benefit)
                    {
                        benefitlist.Add(new UpdateMembershipBenefitInput
                            (
                            ex.Benefit,
                            membership.Id
                            ));
                    }
                }
            }

            var entity = new UpdateMembershipInput
            (
            membership.Id,
            membership.Name,
            membership.Description,
            membership.Benefits,
            membership.Status,
            membership.Type,
            membership.Pricing,
            membership.MonthlyDuration,
            user.Id,
            membership.Users
            );

            var result = await updateservice.ExecuteAsync(entity, benefitlist);
            if (!result.Success)
            {
                return BadRequest();
            }

            return RedirectToAction("Index", "MemeberShip");
        }

        [HttpPost("UpdateMembership")]
        public async Task<IActionResult> UpdateMembership(MembershipViewModel form, string id, CancellationToken ct = default)
        {
            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var update = await service.GetMembershipByIdAsync(id, ct);
            var benefitlist = new List<UpdateMembershipBenefitInput>();

            var membership = new UpdateMembershipInput
                    (
                        id: update.Id,
                        name: form.UpdateMembershipForm.MembershipName,
                        description: form.UpdateMembershipForm.description,
                        benefits: new List<string>(),
                        status: update.Status,
                        type: update.Type,
                        pricing: form.UpdateMembershipForm.pricing,
                        monthlyDuration: form.UpdateMembershipForm.monthlyDuration,
                        userid: update.Userid,
                        users: update.Users
                    );

            var exsist = await benefitService.GetBenefitsAsync();

            foreach ( var ex in exsist )
            {
                foreach ( var benefit in update.Benefits )
                {
                    if ( ex.MembershipId == update.Id && ex.Benefit == benefit)
                    {
                        var deletebenefit = await deleteMembershipService.DeleteBenefit(ex.Id, ct);
                    }
                }
            }

            foreach (var newbenefit in form.MembershipForm.Benefits)
            {

                benefitlist.Add(new UpdateMembershipBenefitInput
                    (
                    benefit: newbenefit.benefit,
                    membershipId: update.Id
                     )
                    );
            }

            var result = await updateservice.ExecuteAsync(membership, benefitlist);
            if (!result.Success)
            {
                return BadRequest();
            }

            return RedirectToAction("MyMembership", "My");
        }


        [HttpPost("DeleteMembership")]

        public async Task<IActionResult> DeleteMembership(MyAccountViewModel viewModel, string id, CancellationToken ct = default)
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

            var benefitlist = new List<UpdateMembershipBenefitInput>();
            var exsist = await benefitService.GetBenefitsAsync();

            foreach (var benefit in membership.Benefits)
            {
                foreach (var ex in exsist)
                {
                    if (ex.Benefit == benefit)
                    {
                        benefitlist.Add(new UpdateMembershipBenefitInput
                            (
                            ex.Benefit,
                            membership.Id
                            ));
                    }
                }
            }

            var status = "Delete";

            var entity = new UpdateMembershipInput
            (
            membership.Id,
            membership.Name,
            membership.Description,
            membership.Benefits,
            status,
            membership.Type,
            membership.Pricing,
            membership.MonthlyDuration,
            user.Id,
            membership.Users
            );

            var result = await updateservice.ExecuteAsync(entity, benefitlist);
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
        public async Task<IActionResult> CreateMembership(MembershipViewModel form, CancellationToken ct = default)
        {
            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var membership = new RegisterMemebershipInput
                    (
                        name: form.MembershipForm.MembershipName,
                        description: form.MembershipForm.description,
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "Monthly",
                        pricing: form.MembershipForm.pricing,
                        monthlyDuration: form.MembershipForm.monthlyDuration
                    );
            foreach (var benefit in form.MembershipForm.Benefits) {
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
