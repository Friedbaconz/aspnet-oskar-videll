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
        var bookings = await service.GetBookingsAsync();
        var model = new BookingViewModel()
        {
            Bookings = bookings
        };
        return View(model);
    }


    [HttpPost("RegisterBooking")]
    public async Task<IActionResult> RegisterBooking(string id, CancellationToken ct = default)
    {

        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var user = userManager.GetUserId(User);

        if(user == null)
        {
            ViewData["ErrorMessage"] = "An error occurred during registration.";
            return View();
        }

        var booking = new RegisterBookingInput
            (
                workoutId: id,
                userId: user
            );

        var result = await register.ExecuteAsync(booking, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
            return View();
        }

        return RedirectToAction("Index", "CostumerService");
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
            return View();
        }

        var booking = await service.GetAllBookingByUserIdAsync(userprofile.Value.Id, ct);

        if(booking == null) { 
            ViewData["ErrorMessage"] = "An error occurred during Deletion.";
            return View();
        }

        foreach(var bookings in booking)
        {
            if(bookings.WorkoutId == id)
            {
                var result = await Delete.ExecuteAsync(bookings.Id, ct);

                if (!result.Success)
                {
                    ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
                    return View();
                }

                return View();
            }

        }

        return View();
    }

    [HttpPost("UpdateBooking")]
    public async Task<IActionResult> UpdateBooking(MyAccountViewModel form, CancellationToken ct = default)
    {

        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        var booking = new UpdateBookingInput
            (
                Id: form.MyBookingViewModel.BookingViewModel.UpdateBookingForm.Id,
                WorkoutId: form.MyBookingViewModel.BookingViewModel.UpdateBookingForm.WorkoutId,
                UserId: form.MyBookingViewModel.BookingViewModel.UpdateBookingForm.UserId
            );

        var result = await update.ExecuteAsync(booking, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
            return View();
        }

        return View();
    }
}
