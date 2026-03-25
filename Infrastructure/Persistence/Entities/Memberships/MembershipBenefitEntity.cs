namespace Infrastructure.Persistence.Entities.Memberships;

public sealed class MembershipBenefitEntity
{
    public int MembershipBenefitID { get; set; }
    public Guid MembershipID { get; set; }
    public string Benefit { get; set; } = null!;
    public MembershipEntity Membership { get; set; } = null!;
}
