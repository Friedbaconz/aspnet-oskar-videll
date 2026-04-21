using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;

namespace Presentation.WebApp.Models.CostumerService;

public class WorkoutViewModel
{
    public IEnumerable<Workout> Workouts { get; set; } = [];

    public RegisterWorkoutForm RegisterWorkoutForm { get; set; } = null!;

    public UpdateWorkoutForm UpdateWorkoutForm { get; set; } = null!;
}
