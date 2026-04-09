using Application.Abstractions.Identity;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Memberships.Services;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Application.Users.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("User")]
public class UserController(IRemoveUserMemembershipService removemembership,UserManager<ApplicationUser> userManager, IGetUserProfileService getUserProfileService, IUpdateUserService updateUserService, IRemoveUserService removeUserService, IidentityService iidentityService, IMembershipService membershipservice) : Controller
{
    [HttpGet("My")]
    public async Task<IActionResult> My(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var profile = await getUserProfileService.ExecuteAsync(user.Id, ct);
        if (profile is null)
        {
            return NotFound();
        }

        var viewmodel = new MyAccountViewModel
        {
            Email = user.Email ?? string.Empty,
            ProfileForm = new MyProfileForm
            {
                FirstName = profile.Value?.FirstName ?? string.Empty,
                LastName = profile.Value?.LastName ?? string.Empty,
                PhoneNumber = profile.Value?.Phonenumber ?? string.Empty,
                ProfileImageUri = profile.Value?.ProfileImageUri ?? string.Empty,
            }
        };


        return View(viewmodel);
    }

    [HttpPost("My")]

    public async Task<IActionResult> My(MyAccountViewModel viewmodel, CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        if (!ModelState.IsValid)
        {
            return View(viewmodel);
        }

        viewmodel.Email = user.Email ?? string.Empty;

        var input = new UpdateUserProfileInput
        (
            user.Id,
            viewmodel.ProfileForm.FirstName,
            viewmodel.ProfileForm.LastName,
            viewmodel.ProfileForm.PhoneNumber,
            viewmodel.ProfileForm.ProfileImageUri
        );

        var result = await updateUserService.ExecuteAsync(input, ct);
        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred while updating your profile.";
            ViewData["ErrorType"] = "error";
            return View(viewmodel);
        }

        ViewData["Message"] = result.ErrorMessage;
        ViewData["ErrorType"] = "success";

        return View(viewmodel);
    }

    [HttpGet("MyMembership")]
    public async Task<IActionResult> MyMembership(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }

        var profile = await getUserProfileService.ExecuteAsync(user.Id, ct);
        if (profile is null)
        {
            return NotFound();
        }

        if(profile.Value.MembershipId == string.Empty)
        {
            var Viewmodel = new MyMembershipViewModel
            {
                MembershipId = null,
                MembershipName = null,
                Description = null,
                Benefits = null
            };
            return View(Viewmodel);
        }
        else
        {
            var membership = await membershipservice.GetMembershipByIdAsync(profile.Value.MembershipId);

            var Benefit = new List<string>();

            foreach (var benefit in membership.Benefits)
            {
                if (benefit is not null)
                {
                    Benefit.Add(benefit);
                }
            }

            var viewmodel = new MyMembershipViewModel
            {
                MembershipId = membership.Id,
                MembershipName = membership.Name,
                Description = membership.Description,
                Benefits = Benefit
            };


            return View(viewmodel);
        }

    }

    [HttpPost("DeleteMembership")]

    public async Task<IActionResult> DeleteMembership(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);

        if (user is null)
        {
            return Challenge();
        }

        var profile = await getUserProfileService.ExecuteAsync(user.Id, ct);

        if (profile is null)
        {
            return NotFound();
        }

        var result = await removemembership.ExecuteAsync(user.Id, profile.Value.MembershipId, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred while deleting your membership.";
            ViewData["ErrorType"] = "error";
            return RedirectToAction("My");
        }


        return RedirectToAction("MyMembership", "User");
    }

    [HttpPost("DeleteUser")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteUser(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
        }
        var result = await removeUserService.ExecuteAsync(user.Id, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred while deleting your account.";
            ViewData["ErrorType"] = "error";
            return RedirectToAction("My");
        }

        var deleteResult = await iidentityService.DeleteUserAsync(user.Email ?? string.Empty, ct);

        if (!deleteResult.Success)
        {
            ViewData["ErrorMessage"] = deleteResult.ErrorMessage ?? "An error occurred while deleting your account from the identity store.";
            ViewData["ErrorType"] = "error";
            return RedirectToAction("My");
        }

        await iidentityService.SignOutAsync(ct);
        return RedirectToAction("Index", "Home");
    }


}
