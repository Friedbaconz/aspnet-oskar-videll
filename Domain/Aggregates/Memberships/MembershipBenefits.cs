

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{
    public MembershipBenefits(Guid id, Guid membershipId, string benefit, Membership membership)
    {
        Id = RequiredGuid(id, nameof(Id));
        MembershipId = RequiredGuid(membershipId, nameof(MembershipId));
        Benefit = RequiredString(benefit, nameof(Benefit));
        Membership = membership ?? throw new ArgumentNullException(nameof(membership));
    }

    public Guid Id { get; }

    public Guid MembershipId { get; }

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

    public static MembershipBenefits Create(Guid membershipId, string benefit, Membership membership)
    {
        return new MembershipBenefits(Guid.NewGuid(), membershipId, benefit, membership);
    }

    public static MembershipBenefits Rehydrate(Guid id, Guid membershipId, string benefit, Membership membership)
    {
        return new MembershipBenefits(id, membershipId, benefit, membership);
    }

    public void Update(string benefit)
    {
        Benefit = RequiredString(benefit, nameof(Benefit));
    }
}
