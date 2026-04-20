using Domain.Aggregates.Memberships;

namespace Presentation.WebApp.Models.Memberships;

public class NewMembershipForm
{

    public string MembershipName { get; set; } = null!;

    public string description { get; set; } = null!;

    public List<NewMembershipBenefitForm> Benefits { get; set; } = new List<NewMembershipBenefitForm>();

    public decimal pricing {  get; set; }

    public int monthlyDuration { get; set; }
}
