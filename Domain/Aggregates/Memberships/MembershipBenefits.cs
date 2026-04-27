

using Domain.Aggregates.Users;
using System.Xml.Linq;

namespace Domain.Aggregates.Memberships;

public sealed class MembershipBenefits
{

    public string Id { get; private set; }

    public string MembershipId { get; private set; }

    public string? Benefit { get; private set; }



    private MembershipBenefits()
    {

    }

    private MembershipBenefits(string id)
    {
        Id = id;
    }

    public static MembershipBenefits Create()
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
            Benefit = RequiredString(benefit, nameof(benefit)),
        };

        return membershipbenefit;
    }

    private static string RequiredString(string value, string propertyname)
    {
        if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException($"{propertyname} is required.");

        return value;
    }

    public void UpdateBenefit(string newBenefit)
    {
        Benefit = string.IsNullOrWhiteSpace(newBenefit) ? string.Empty : newBenefit.Trim();
    }
}
