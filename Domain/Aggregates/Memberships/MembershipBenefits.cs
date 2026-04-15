

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{

    public int Id { get; private set; }

    public string MembershipId { get; private set; }

    public string Benefit { get; private set; }


    private MembershipBenefits()
    {

    }

    private MembershipBenefits(int id)
    {
        Id = id;
    }

    public static MembershipBenefits create(string membershipId, string benefit)
    {
        var request = new MembershipBenefits
        {
            Benefit = benefit,
            MembershipId = membershipId,
        };

        return request;
    }


    public static MembershipBenefits Create(int id,string membershipId, string benefit)
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
