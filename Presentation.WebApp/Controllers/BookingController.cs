using Application.Bookings.Abstractions;
using Application.Bookings.Inputs;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Workouts;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Bookings;
using Presentation.WebApp.Models.CostumerService;
using Presentation.WebApp.Models.Users;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Presentation.WebApp.Controllers;

public class BookingController(IBookingService service, IDeleteBookingService Delete, IRegisterBookingService register, IUpdateBookingService update, IWorkoutRepository workout) : Controller
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
    public async Task<IActionResult> RegisterBooking(MyAccountViewModel form, CancellationToken ct = default)
    {

        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        var booking = new RegisterBookingInput
            (
                workoutId: form.MyBookingViewModel.BookingViewModel.RegisterBookingForm.WorkoutId,
                userId: form.MyBookingViewModel.BookingViewModel.RegisterBookingForm.UserId
            );

        var result = await register.ExecuteAsync(booking, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
            return View(form);
        }

        return RedirectToAction("MyBooking", "My");
    }

    [HttpPost("DeleteBooking")]
    public async Task<IActionResult> DeleteBooking(string id, CancellationToken ct = default)
    {

        if (id == null)
        {
            throw new ArgumentNullException(nameof(id));
        }

        var result = await Delete.ExecuteAsync(id, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
            return View();
        }

        return RedirectToAction("MyBooking", "My");
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

        return RedirectToAction("MyBooking", "My");
    }
}
