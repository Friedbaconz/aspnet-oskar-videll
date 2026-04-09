

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{
    public MembershipBenefits(int id, string membershipId, string benefit)
    {
        Id = RequiredInt(id, nameof(Id));
        MembershipId = RequiredString(membershipId, nameof(MembershipId));
        Benefit = RequiredString(benefit, nameof(Benefit));
    }

    public int Id { get; }

    public string MembershipId { get; }

    public string Benefit { get; private set; }

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

    public static MembershipBenefits Create(int id,string membershipId, string benefit)
    {
        return new MembershipBenefits(id ,membershipId, benefit);
    }

    public static MembershipBenefits Rehydrate(int id, string membershipId, string benefit)
    {
        return new MembershipBenefits(id, membershipId, benefit);
    }

    public void UpdateBenefit(string newBenefit)
    {
        Benefit = RequiredString(newBenefit, nameof(Benefit));
    }
}
