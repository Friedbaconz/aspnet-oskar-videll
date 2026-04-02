using Application.Bookings.Services;
using Application.Workouts.Services;
using Domain.Abstractions.Repositories.Workouts;
using Domain.Aggregates.Workouts;
using Microsoft.AspNetCore.Mvc;
using Presentation.WebApp.Models.CostumerService;

namespace Presentation.WebApp.Controllers;

public class CostumerServiceController(IWorkoutService workoutservice, IBookingService bookingservice) : Controller
{
    public async Task<IActionResult> Index() {
    {
            var workouts = await workoutservice.GetWorkoutsAsync();
            var model = new WorkoutViewModel()
            {
                Workouts = workouts
            };
            return View(model);
    }
}
}