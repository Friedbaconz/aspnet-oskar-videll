using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Workouts;

namespace Infrastructure.Persistence.Entities.Users;

public class UserEntity
{
    public Guid UserID { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phonenumber { get; set; }

    public string? MembershipStatus { get; set; }

    public MembershipEntity? Membership { get; set; }

    public ICollection<WorkoutEntity> Workouts = [];

}
