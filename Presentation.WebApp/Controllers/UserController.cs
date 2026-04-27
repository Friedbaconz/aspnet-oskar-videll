using Application.Abstractions.Identity;
using Application.Bookings.Abstractions;
using Application.Common.Results;
using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Application.Users.Services;
using Application.Workouts.Abstractions;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Presentation.WebApp.Models.Bookings;
using Presentation.WebApp.Models.CostumerService;
using Presentation.WebApp.Models.Memberships;
using Presentation.WebApp.Models.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Presentation.WebApp.Controllers;

[Authorize]
[Route("User")]
public class UserController(UserManager<ApplicationUser> userManager, IGetUserProfileService getUserProfileService, IUpdateUserService updateUserService, IRemoveUserService removeUserService, IidentityService iidentityService, IMembershipService membershipservice, IWorkoutService workoutService, IBookingService bookingService) : Controller
{
    private readonly UserManager<ApplicationUser> userManager = userManager;

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
        if (!ModelState.IsValid)
            return View(viewmodel);

        var user = await userManager.GetUserAsync(User);
        if (user is null)
        {
            return Challenge();
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

        var roleresult = await userManager.AddToRoleAsync(user, "User");

        if(!roleresult.Succeeded)
        {
            ViewData["ErrorMessage"] = "Profile updated, but failed to assign user role.";
            ViewData["ErrorType"] = "warning";
            return View(viewmodel);
        }

        ViewData["Message"] = result.ErrorMessage;
        ViewData["ErrorType"] = "success";

        return View(viewmodel);
    }

    [HttpGet("MyMembership")]
    public async Task<IActionResult> MyMembership(CancellationToken ct = default)
    {
        var getmemberships = await membershipservice.GetMembershipsAsync(ct);

        var Viewmodels = new MyMembershipViewModel
        {
            MembershipName = string.Empty,
            Benefits = new List<string>(),
            MembershipViewModel = new MembershipViewModel
            {
                Memberships = getmemberships
            }
        };

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

        var memberships = await membershipservice.GetMembershipByIdAsync(profile.Value.MembershipId, ct);

        if (memberships == null)
        {
            return View(Viewmodels);
        }
        else
        {
            Viewmodels.MembershipId = memberships.Id;
            Viewmodels.MembershipName = memberships.Name;
            Viewmodels.Description = memberships.Description;
            Viewmodels.status = memberships.Status.ToString();
            Viewmodels.Benefits = memberships.Benefits.Select(b => b.Benefit);

            return View(Viewmodels);
        }
    }

    [HttpGet("MyBooking")]
    public async Task<IActionResult> MyBookings(CancellationToken ct = default)
    {
        var user = await userManager.GetUserAsync(User);
        var getbookings = await bookingService.GetBookingsAsync(ct);
        var getworkouts = await workoutService.GetWorkoutsAsync(ct);
        var viewmodels = new MyBookingViewModel
        {
            MyWorkouts = new List<Workout>(),
            BookingViewModel = new BookingViewModel
            {
                Bookings = getbookings,
            },
            workoutViewModels = new WorkoutViewModel
            {
                Workouts = getworkouts,
            }

        };

        if (user is null)
        {
            return Challenge();
        }

        var profile = await getUserProfileService.ExecuteAsync(user.Id, ct);

        if (profile is null)
        {
            return NotFound();
        }

        var bookings = await bookingService.GetAllBookingByUserIdAsync(profile.Value.Id, ct);

        if (bookings.Count() == 0)
        {
            return View(viewmodels);
        }
        else
        {
            foreach (var booking in bookings)
            {
                var workout = await workoutService.GetWorkoutByIdAsync(booking.WorkoutId, ct);

                viewmodels.MyWorkouts.Add(workout);
            }

            return View(viewmodels);
        }
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
