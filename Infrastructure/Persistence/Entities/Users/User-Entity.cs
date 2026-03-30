using Infrastructure.Identity;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Workouts;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructure.Persistence.Entities.Users;

public class UserEntity
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Phonenumber { get; set; }

    public string? MembershipStatus { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public string? ProfileImageUri { get; set; }

    public ApplicationUser User {  get; set; } = null!;

    public int? MembershipID { get; set; }

    public MembershipEntity? Membership { get; set; }

    public ICollection<WorkoutEntity>? Workouts = [];

}
