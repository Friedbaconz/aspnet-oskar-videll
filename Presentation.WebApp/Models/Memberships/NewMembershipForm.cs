using Domain.Aggregates.Memberships;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Memberships;

public class NewMembershipForm
{
    [Required]
    public string MembershipName { get; set; } = null!;

    [Required]
    public string description { get; set; } = null!;

    public List<NewMembershipBenefitForm> Benefits { get; set; } = new List<NewMembershipBenefitForm>();

    [Required]
    public decimal pricing {  get; set; }

    [Required]
    public int monthlyDuration { get; set; }
}
