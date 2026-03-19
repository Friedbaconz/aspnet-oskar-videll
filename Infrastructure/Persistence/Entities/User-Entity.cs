
namespace Infrastructure.Persistence.Entities;

public class UserEntity
{
    public Guid UserID { get; set; } = Guid.NewGuid();

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public string? MembershipStatus { get; set; }

    public MembershipEntity? Membership { get; set; }

    public ICollection<WorkoutEntity> Workouts = [];

}
