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
    public async Task<IActionResult> RegisterWorkout(MyAccountViewModel form, CancellationToken ct = default)
    {

            if (form == null)
            {
                throw new ArgumentNullException(nameof(form));
            }

            var workout = new RegisterWorkoutInput
                (
                Name: form.MyBookingViewModel.workoutViewModels.RegisterWorkoutForm.Name,
                Category: form.MyBookingViewModel.workoutViewModels.RegisterWorkoutForm.Category,
                Instructions: form.MyBookingViewModel.workoutViewModels.RegisterWorkoutForm.Instructions,
                Date: form.MyBookingViewModel.workoutViewModels.RegisterWorkoutForm.Date,
                Time: form.MyBookingViewModel.workoutViewModels.RegisterWorkoutForm.Time
                );

            var result = await register.ExecuteAsync(workout, ct);

            if (!result.Success)
            {
                ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during registration.";
                return View(form);
            }

            return View();
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

        return View();
    }

    [HttpPost("UpdateWorkout")]
    public async Task<IActionResult> UpdateWorkout(MyAccountViewModel form, CancellationToken ct = default)
    {

        if (form == null)
        {
            throw new ArgumentNullException(nameof(form));
        }

        var workout = new UpdateWorkoutInput
            (
            Id: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Id,
            Name: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Name,
            Category: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Category,
            Instructions: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Instructions,
            Date: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Date,
            Time: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Time,
            Users: form.MyBookingViewModel.workoutViewModels.UpdateWorkoutForm.Users
            );

        var result = await Update.ExecuteAsync(workout, ct);

        if (!result.Success)
        {
            ViewData["ErrorMessage"] = result.ErrorMessage ?? "An error occurred during Deletion";
            return View();
        }

        return View();
    }
}