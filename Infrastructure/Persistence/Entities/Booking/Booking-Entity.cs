using Infrastructure.Persistence.Entities.Users;
using Infrastructure.Persistence.Entities.Workouts;

namespace Infrastructure.Persistence.Entities.Booking;

public class BookingEntity
{
    public string BookingID { get; set; } = null!;

    public string UserID { get; set; } = null!;

    public string WorkoutID { get; set; } = null!;

    public UserEntity User { get; set; } = null!;

    public WorkoutEntity Workout { get; set; } = null!;

}
