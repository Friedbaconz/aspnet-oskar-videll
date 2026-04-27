using Infrastructure.Persistence.Entities.Users;

namespace Infrastructure.Persistence.Entities.Workouts;

public class WorkoutEntity
{
    public string WorkoutID { get; set; } = null!;

    public string WorkoutName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Instructor { get; set; } = null!;

    public string Date { get; set; } = null!;

    public string Time { get; set; } = null!;

    public ICollection<UserEntity> Users = [];
}
