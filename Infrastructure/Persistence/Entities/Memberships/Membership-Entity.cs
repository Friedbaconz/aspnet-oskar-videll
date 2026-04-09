using Infrastructure.Persistence.Entities.Users;

namespace Infrastructure.Persistence.Entities.Memberships;

public sealed class MembershipEntity
{
    public string MembershipID { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public ICollection<MembershipBenefitEntity> Benefits = [];

    public string Type { get; set; } = null!;

    public string Status { get; set; } = null!;

    public decimal Pricing { get; set; }

    public int DurationInMonths { get; set; }

    public ICollection<UserEntity> Users = [];

}
