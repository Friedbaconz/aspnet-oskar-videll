using Application.Bookings.Abstractions;
using Application.Memberships.Abstractions;
using Application.Users.Abstractions;
using Application.Workouts.Abstractions;
using Application.Workouts.Inputs;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.Bookings;
using Presentation.WebApp.Models.CostumerService;

namespace Presentation.WebApp.Controllers;

public class CostumerServiceController(IWorkoutService service, IRegisterWorkoutService register, IDeleteWorkoutService Delete, IUpdateWorkoutService Update) : Controller
{
    public async Task<IActionResult> Index()
    {
        var workouts = await service.GetWorkoutsAsync();
        var model = new WorkoutViewModel()
        {
            Workouts = workouts
        };
        return View(model);
    }

    [HttpPost("RegisterWorkout")]
    public async Task<IActionResult> RegisterWorkout(WorkoutViewModel form, CancellationToken ct = default)
    {

            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var workout = new RegisterWorkoutInput
                (
                Name: form.RegisterWorkoutForm.Name,
                Category: form.RegisterWorkoutForm.Category,
                Instructions: form.RegisterWorkoutForm.Instructions,
                Date: form.RegisterWorkoutForm.Date,
                Time: form.RegisterWorkoutForm.Time
                );

            var result = await register.ExecuteAsync(workout, ct);

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
                return View(form);
            }

            return RedirectToAction("MyBooking", "My");
    }

    [HttpPost("DeleteWorkout")]
    public async Task<IActionResult> DeleteWorkout(string id, CancellationToken ct = default)
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

    [HttpPost("UpdateWorkout")]
    public async Task<IActionResult> UpdateWorkout(WorkoutViewModel form, CancellationToken ct = default)
    {

        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        var workout = new UpdateWorkoutInput
            (
            Id: form.UpdateWorkoutForm.Id,
            Name: form.UpdateWorkoutForm.Name,
            Category: form.UpdateWorkoutForm.Category,
            Instructions: form.UpdateWorkoutForm.Instructions,
            Date: form.UpdateWorkoutForm.Date,
            Time: form.UpdateWorkoutForm.Time,
            Users: form.UpdateWorkoutForm.Users
            );

        var result = await Update.ExecuteAsync(workout, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
            return View();
        }

        return RedirectToAction("MyBooking", "My");
    }
}