using Infrastructure.Persistence.Entities.Users;
using Infrastructure.Persistence.Entities.Workouts;

namespace Infrastructure.Persistence.Entities.Booking;

public class BookingEntity
{
    public int BookingID { get; set; }

    public string UserID { get; set; } = null!;

    public int WorkoutID { get; set; }

    public UserEntity User { get; set; } = null!;

    public WorkoutEntity Workout { get; set; } = null!;

}
