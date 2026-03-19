
namespace Infrastructure.Persistence.Entities;

public class WorkoutEntity
{
    public Guid WorkoutID { get; set; }

    public string WorkoutName { get; set; } = null!;

    public string Category { get; set; } = null!;

    public string Instructor { get; set; } = null!;

    public DateTime Date { get; set; }

    public TimeSpan Time { get; set; }


    public ICollection<UserEntity> Users = [];
}
