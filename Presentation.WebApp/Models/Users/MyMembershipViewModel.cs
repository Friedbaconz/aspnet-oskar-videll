using Presentation.WebApp.Models.Memberships;

namespace Presentation.WebApp.Models.Users;

public class MyMembershipViewModel
{
    public string MembershipId { get; set; } = null!;

    public string MembershipName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string status { get; set; } = null!;
    public IEnumerable<string> Benefits { get; set; } = new List<string>();

    public MembershipViewModel MembershipViewModel { get; set; } = new MembershipViewModel();
}
