using Application.Bookings.Abstractions;
using Application.Bookings.Inputs;
using Application.Users.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Workouts;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Bookings;
using Presentation.WebApp.Models.CostumerService;
using Presentation.WebApp.Models.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Presentation.WebApp.Controllers;

public class BookingController(IGetUserProfileService getUserProfileService, UserManager<ApplicationUser> userManager, IBookingService service, IDeleteBookingService Delete, IRegisterBookingService register, IUpdateBookingService update, IWorkoutRepository workout) : Controller
{
    public async Task<IActionResult> Index()
    {
        var newworkout = await workout.GetAllAsync();

        var bookings = new RegisterBookingForm
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            PhoneNumber = string.Empty,
            Email = string.Empty,
            Message = string.Empty,
            WorkoutId = string.Empty,
            Workouts = newworkout.ToList()
        };

        return View(bookings);
    }

    [HttpGet("RegisterBooking")]
    public async Task<IActionResult> RegisterBooking(CancellationToken ct = default)
    {
        var newworkout = await workout.GetAllAsync(ct);

        var message = TempData["ErrorMessage"] as string;

        ViewBag.Message = message;

        var bookings = new RegisterBookingForm
        {
            FirstName = string.Empty,
            LastName = string.Empty,
            PhoneNumber = string.Empty,
            Email = string.Empty,
            Message = string.Empty,
            WorkoutId = string.Empty,
            Workouts = newworkout.ToList()
        };

        return View(bookings);
    }

    [HttpPost("RegisterBooking")]
    public async Task<IActionResult> RegisterBooking(RegisterBookingForm form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
        {
            var newworkout = await workout.GetAllAsync(ct);

            form.Workouts = newworkout;

            return View(form);
        }

        var user = await userManager.GetUserAsync(User);

        var userprofile = await getUserProfileService.ExecuteAsync(user.Id, ct);

        if (string.IsNullOrWhiteSpace(userprofile.Value.FirstName) || string.IsNullOrWhiteSpace(userprofile.Value.LastName))
        {
            TempData["ErrorMessage"] = "First Name and Last Name on profile is required";
            return RedirectToAction("RegisterBooking", "Booking");
        }

        if (form.WorkoutId == null)
        {
            ViewData["ErrorMessage"] = "An error occurred during registration.";
            return View(form);
        }


        var booking = new RegisterBookingInput
            (
                workoutId: form.WorkoutId,
                userId: userprofile.Value.UserId
            );

        var result = await register.ExecuteAsync(booking, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
            return View(form);
        }

        return View(form);
    }

    [HttpPost("DeleteBooking")]
    public async Task<IActionResult> DeleteBooking(string id, CancellationToken ct = default)
    {

        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var getuser = userManager.GetUserId(User);

        var userprofile = await getUserProfileService.ExecuteAsync(getuser, ct);

        if(userprofile == null)
        {
            ViewData["ErrorMessage"] = "An error occurred during Deletion.";
            return RedirectToAction("MyBooking", "User");
        }

        var booking = await service.GetAllBookingByUserIdAsync(userprofile.Value.Id, ct);

        if(booking == null) { 
            ViewData["ErrorMessage"] = "An error occurred during Deletion.";
            return RedirectToAction("MyBooking", "User");
        }

        foreach(var bookings in booking)
        {
            if(bookings.WorkoutId == id)
            {
                var result = await Delete.ExecuteAsync(bookings.Id, ct);

                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
                    return RedirectToAction("MyBooking", "User");
                }

                return RedirectToAction("MyBooking", "User");
            }

        }

        return RedirectToAction("MyBooking", "User");
    }

    [HttpPost("UpdateBooking")]
    public async Task<IActionResult> UpdateBooking(BookingViewModel form, CancellationToken ct = default)
    {
        if (!ModelState.IsValid)
            return View(form);

        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        var booking = new UpdateBookingInput
            (
                Id: form.UpdateBookingForm.Id,
                WorkoutId: form.UpdateBookingForm.WorkoutId,
                UserId: form.UpdateBookingForm.UserId
            );

        var result = await update.ExecuteAsync(booking, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
            return View();
        }

        return View(form);
    }
}
