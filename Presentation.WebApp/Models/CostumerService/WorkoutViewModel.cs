using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;

namespace Presentation.WebApp.Models.CostumerService;

public class WorkoutViewModel
{
    public IEnumerable<Workout> Workouts { get; set; } = [];

    public IEnumerable<string> WorkoutIDs { get; set; } = [];

    public IEnumerable<Booking> Bookings { get; set; } = [];

    public RegisterWorkoutForm RegisterWorkoutForm { get; set; } = null!;

    public UpdateWorkoutForm UpdateWorkoutForm { get; set; } = null!;

    public MyWorkoutViewModel MyWorkout { get; set; } = null!;
}
