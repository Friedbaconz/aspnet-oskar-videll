using Domain.Aggregates.Memberships;
using Presentation.WebApp.Models.Users;

namespace Presentation.WebApp.Models.Memberships;

public class MembershipViewModel
{
    public IEnumerable<Membership> Memberships { get; set; } = [];
    public IEnumerable<string> MembershipIDs { get; set; } = [];
    public IEnumerable<MembershipBenefits> Benefits { get; set; } = [];
    public MyMembershipViewModel MyMembership { get; set; } = null!;
    public NewMembershipForm MembershipForm { get; set; } = null!;
    public UpdateMemberShipForm UpdateMembershipForm { get; set; } = null!;
}
