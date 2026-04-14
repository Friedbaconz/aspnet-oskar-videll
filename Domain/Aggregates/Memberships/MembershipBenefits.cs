

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

    public static MembershipBenefits Create(int id)
    {

        var membership = new MembershipBenefits
            (
                id = 1
            );

        return membership;
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
