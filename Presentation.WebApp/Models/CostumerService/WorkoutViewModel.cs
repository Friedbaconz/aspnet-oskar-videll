using Domain.Aggregates.Bookings;
using Domain.Aggregates.Workouts;
using Presentation.WebApp.Models.Users;

namespace Presentation.WebApp.Models.CostumerService;

public class WorkoutViewModel
{
    public IEnumerable<Workout> Workouts { get; set; } = [];

    public MyBookingViewModel workoutprofile { get; set; } = null!;

    public RegisterWorkoutForm RegisterWorkoutForm { get; set; } = null!;

    public UpdateWorkoutForm UpdateWorkoutForm { get; set; } = null!;
}
