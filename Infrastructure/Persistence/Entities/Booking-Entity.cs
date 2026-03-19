
namespace Infrastructure.Persistence.Entities;

public class BookingEntity
{
    public int BookingID { get; set; }

    public Guid UserID { get; set; }

    public Guid WorkoutID { get; set; }

    public UserEntity User { get; set; } = null!;

    public WorkoutEntity Workout { get; set; } = null!;

}
