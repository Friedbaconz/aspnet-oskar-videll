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
using Presentation.WebApp.Models.Users;

namespace Presentation.WebApp.Controllers;

public class CostumerServiceController(IWorkoutService service, IRegisterWorkoutService register, IDeleteWorkoutService Delete, IUpdateWorkoutService Update) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct = default)
    {
        var workouts = await service.GetWorkoutsAsync();
        var model = new WorkoutViewModel()
        {
            Workouts = workouts
        };
        return View(model);
    }

    [HttpGet("RegisterWorkout")]
    public IActionResult RegisterWorkout(CancellationToken ct = default)
    {
        var model = new RegisterWorkoutForm
        {
            Name = string.Empty,
            Category = string.Empty,
            Instructions = string.Empty,
            Date = DateTime.Now,
            Time = TimeSpan.Zero,
        };
        return View(model);
    }

    [HttpPost("RegisterWorkout")]
    public async Task<IActionResult> RegisterWorkout(RegisterWorkoutForm form, CancellationToken ct = default)
    {
            if (!ModelState.IsValid)
                return View(form);

            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var workout = new RegisterWorkoutInput
                (
                Name: form.Name,
                Category: form.Category,
                Instructions: form.Instructions,
                Date: form.Date,
                Time: form.Time
                );

            var result = await register.ExecuteAsync(workout, ct);

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
                return View(form);
            }

            ViewData["Message"] = result.ErrorMessage;
            ViewData["ErrorType"] = "success";

            return View(form);
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
            return RedirectToAction("UpdateWorkout", "CostumerService");
        }

        return RedirectToAction("UpdateWorkout", "CostumerService");
    }

    [HttpGet("UpdateWorkout")]
    public async Task<IActionResult> UpdateWorkout(CancellationToken ct = default) 
    {

        var workouts = await service.GetWorkoutsAsync();

        var model = new WorkoutViewModel()
        {
            Workouts = workouts
        };


        return View(model);
    }

    [HttpPost("UpdateWorkout")]
    public async Task<IActionResult> UpdateWorkout(WorkoutViewModel form, CancellationToken ct = default)
    {

        if (!ModelState.IsValid)
            return View(form);

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
            return View(form);
        }

        return View(form);
    }
}