using Domain.Aggregates.Memberships;

namespace Presentation.WebApp.Models.Memberships;

public class MembershipViewModel
{
    public IEnumerable<Membership> Memberships { get; set; } = [];

    public IEnumerable<MembershipBenefits> Benefits { get; set; } = [];
}
