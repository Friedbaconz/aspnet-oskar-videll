namespace Presentation.WebApp.Models.Users;

public class MyMembershipViewModel
{
    public string MembershipId { get; set; } = null!;

    public string MembershipName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string status { get; set; } = null!;

    public List<string> Benefits { get; set; } = null!;
}
