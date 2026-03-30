

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{
    public MembershipBenefits(int id, int membershipId, string benefit, Membership membership)
    {
        Id = RequiredInt(id, nameof(Id));
        MembershipId = RequiredInt(membershipId, nameof(MembershipId));
        Benefit = RequiredString(benefit, nameof(Benefit));
        Membership = membership ?? throw new ArgumentNullException(nameof(membership));
    }

    public int Id { get; }

    public int MembershipId { get; }

    public string Benefit { get; private set; }

    public Membership Membership { get; private set; } = null!;

    private static Guid RequiredGuid(Guid value, string propertyName)
    {
        if (value == Guid.Empty)
            throw new ArgumentException($"{propertyName} is required.", propertyName);
        return value;
    }
    private static string RequiredString(string value, string propertyName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{propertyName} is required.", propertyName);

        return value.Trim();
    }
    private static int RequiredInt(int value, string propertyName)
    {
        if (value <= 0)
            throw new ArgumentException($"{propertyName} must be a positive integer.", propertyName);
        return value;
    }

    public static MembershipBenefits Create(int id,int membershipId, string benefit, Membership membership)
    {
        return new MembershipBenefits(id ,membershipId, benefit, membership);
    }

    public static MembershipBenefits Rehydrate(int id, int membershipId, string benefit, Membership membership)
    {
        return new MembershipBenefits(id, membershipId, benefit, membership);
    }

    public void UpdateBenefit(string newBenefit)
    {
        Benefit = RequiredString(newBenefit, nameof(Benefit));
    }
}
