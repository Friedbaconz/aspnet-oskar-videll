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
    public class MemeberShipController(IGetUserProfileService getUserProfileService, IDeleteMembershipService deleteMembershipService,IRegisterMembershipService registerMembership,IMembershipService service, IUpdateMembershipService updateservice, UserManager<ApplicationUser> userManager, IBenefitService benefitService) : Controller
    {
        public async Task<IActionResult> Index( CancellationToken ct = default)
        {
            var message = TempData["ErrorMessage"] as string;

            ViewBag.Message = message;

            var memberships = await service.GetMembershipsAsync(ct);
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
            if(id == null)
            {
                throw new ArgumentNullException("id");
            }

            var user = await userManager.GetUserAsync(User);

            var profile = await getUserProfileService.ExecuteAsync(user.Id, ct);

            if (string.IsNullOrWhiteSpace(profile.Value.FirstName) || string.IsNullOrWhiteSpace(profile.Value.LastName))
            {
                TempData["ErrorMessage"] = "First Name and Last Name on profile is required";
                return RedirectToAction("Index", "MemeberShip");
            }

            if (user == null)
            {
                return NotFound();
            }

            var membership = await service.GetMembershipByIdAsync(id);

            if(membership == null)
            {
                return NotFound();
            }

            var benefitList = new List<UpdateMembershipBenefitInput>();

            foreach (var benefitid in membership.Benefits)
            {
                var exsist = await benefitService.GetBenefitByIdAsync(benefitid.Id, ct);
                if (exsist != null && exsist.Id == benefitid.Id)
                {
                    benefitList.Add(new UpdateMembershipBenefitInput(
                        exsist.Id,
                        exsist.Benefit,
                        exsist.MembershipId
                    ));
                }
            }


            var entity = new UpdateMembershipInput(
                membership.Id,
                membership.Name,
                membership.Description,
                benefitList,
                membership.Status,
                membership.Type,
                membership.Pricing,
                membership.MonthlyDuration,
                user.Id,
                membership.Users
            );

            var result = await updateservice.ExecuteAsync(entity, ct);

            if (!result.Success)
            {
                return RedirectToAction("Index", "MemeberShip");
            }

            return RedirectToAction("Index", "MemeberShip");
        }

        [HttpGet("UpdateMembership")]
        public async Task<IActionResult> UpdateMembership(CancellationToken ct = default)
        {
            var memberships = await service.GetMembershipsAsync(ct);
            var benefits = await benefitService.GetBenefitsAsync(ct);

            var model = new MembershipViewModel
            {
                Benefits = benefits,
                Memberships = memberships,
                MembershipIDs = memberships.Select(m => m.Id),

                UpdateMembershipForm = new UpdateMemberShipForm
                {
                    Benefits = new List<NewMembershipBenefitForm>(),
                    MembershipName = string.Empty,
                    description = string.Empty,
                    monthlyDuration = 0,
                    pricing = 0
                }
            };

            return View(model);
        }

        [HttpPost("UpdateMembership")]
        public async Task<IActionResult> UpdateMembership(MembershipViewModel form, string id, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
            {
                var input = form.UpdateMembershipForm;

                var memberships = await service.GetMembershipsAsync(ct);

                if (memberships == null)
                {
                    throw new ArgumentNullException(nameof(input));
                }

                foreach (var membership1 in memberships)
                {
                    if(membership1.Id == id) 
                    {
                        foreach (var benefits in membership1.Benefits)
                        {
                            foreach (var benefit in form.UpdateMembershipForm.Benefits)
                            {
                                benefits.UpdateBenefit
                                (
                                newBenefit: benefit.benefit
                                );
                            }
                        }
                    }

                    if (membership1.Id == id)
                    {

                        membership1.Update(
                            name: input.MembershipName,
                            description: input.description,
                            benefits: membership1.Benefits,
                            status: membership1.Status,
                            type: membership1.Type,
                            pricing: input.pricing,
                            monthlyDuration: input.monthlyDuration,
                            userid: membership1.Userid,
                            users: membership1.Users
                        );

                        break;
                    }
                }

                form.Memberships = memberships;

                return View(form);
            }

            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var update = await service.GetMembershipByIdAsync(id, ct);

            var benefitlist = new List<UpdateMembershipBenefitInput>();

            foreach ( var benefitid in form.UpdateMembershipForm.Benefits)
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
                        name: form.UpdateMembershipForm.MembershipName,
                        description: form.UpdateMembershipForm.description,
                        benefits: benefitlist,
                        status: update.Status,
                        type: update.Type,
                        pricing: form.UpdateMembershipForm.pricing,
                        monthlyDuration: form.UpdateMembershipForm.monthlyDuration,
                        userid: update.Userid,
                        users: update.Users
                    );

            var result = await updateservice.ExecuteAsync(membership);

            if (!result.Success)
            {
                return RedirectToAction("MyMembership", "User", form);
            }

            return RedirectToAction("MyMembership", "User", form);
        }


        [HttpPost("DeleteMembership")]

        public async Task<IActionResult> DeleteMembership( string id, CancellationToken ct = default)
        {
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }

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
            var benefitList = new List<UpdateMembershipBenefitInput>();

            foreach (var benefitid in membership.Benefits)
            {
                var exsist = await benefitService.GetBenefitByIdAsync(benefitid.Id, ct);

                if (exsist.Id == benefitid.Id)
                {
                    benefitList.Add(new UpdateMembershipBenefitInput
                     (
                      exsist.Id,
                      exsist.Benefit,
                      exsist.MembershipId
                     ));
                }

            }

            var entity = new UpdateMembershipInput
            (
            membership.Id,
            membership.Name,
            membership.Description,
            benefitList,
            status,
            membership.Type,
            membership.Pricing,
            membership.MonthlyDuration,
            user.Id,
            membership.Users
            );

            var result = await updateservice.ExecuteAsync(entity, ct);
            if (!result.Success)
            {
                return RedirectToAction("MyMembership", "User");
            }

            return RedirectToAction("MyMembership", "User");
        }

        [HttpPost("RemoveMembership")]
        public async Task<IActionResult> RemoveMembership(string id, CancellationToken ct = default)
        {

            var result = await deleteMembershipService.ExecuteAsync(id);
            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
                return RedirectToAction("MyMembership", "User");
            }

            return RedirectToAction("MyMembership", "User");

        }

        [HttpGet("CreateMembership")]
        public async Task<IActionResult> CreateMembership(CancellationToken ct = default)
        {
            var model = new NewMembershipForm
            {
                MembershipName = string.Empty,
                description = string.Empty,
                pricing = 0,
                monthlyDuration = 0,
                Benefits = new List<NewMembershipBenefitForm>()
            };

            return View(model);
        }

        [HttpPost("CreateMembership")]
        public async Task<IActionResult> CreateMembership(NewMembershipForm form, CancellationToken ct = default)
        {
            if (!ModelState.IsValid)
                return View(form);

            if (form is null)
            {
                throw new ArgumentNullException("form is empty, please redo");
            }

            var membership = new RegisterMemebershipInput
                    (
                        name: form.MembershipName,
                        description: form.description,
                        benefits: new List<RegisterBenfitsInput>(),
                        status: "Active",
                        type: "Monthly",
                        pricing: form.pricing,
                        monthlyDuration: form.monthlyDuration
                    );
            foreach (var benefit in form.Benefits) {
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
                return RedirectToAction("MyMembership", "User", form);
            }

            return RedirectToAction("MyMembership", "User", form);
        }
    }
}
