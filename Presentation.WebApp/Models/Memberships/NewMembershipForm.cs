using Domain.Aggregates.Memberships;

namespace Presentation.WebApp.Models.Memberships;

public class NewMembershipForm
{
    public string MembershipId { get; set; }

    public IEnumerable<MembershipBenefits> Benefits { get; set; } = [];
}
