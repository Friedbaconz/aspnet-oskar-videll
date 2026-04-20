

using Domain.Aggregates.Users;

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{

    public string Id { get; private set; }

    public string MembershipId { get; private set; }

    public string Benefit { get; private set; }


    private MembershipBenefits()
    {

    }

    private MembershipBenefits(string id)
    {
        Id = id;
    }

    public static MembershipBenefits Create(string id)
    {

        var benefit = new MembershipBenefits
            (
                Guid.NewGuid().ToString()
            );

        return benefit;
    }


    public static MembershipBenefits Create(string id, string membershipId, string benefit)
    {
        var membershipbenefit = new MembershipBenefits(id)
        {
            MembershipId = membershipId,
            Benefit = benefit
        };

        return membershipbenefit;
    }

    public void UpdateBenefit(string newBenefit)
    {
        Benefit = newBenefit;
    }
}
