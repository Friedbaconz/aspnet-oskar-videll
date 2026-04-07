using Application.Abstractions.Identity;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Application.Users.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Users;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("User")]
public class UserController( UserManager<ApplicationUser> userManager, IGetUserProfileService getUserProfileService, IUpdateUserService updateUserService, IRemoveUserService removeUserService, IidentityService iidentityService) : Controller
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
