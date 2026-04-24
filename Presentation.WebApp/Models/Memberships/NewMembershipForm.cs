using Domain.Aggregates.Memberships;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Memberships;

public class NewMembershipForm
{
    [Required(ErrorMessage = "Name is required.")]
    [Display(Name = "Membership Name", Prompt = "Enter Membership Name")]
    public string MembershipName { get; set; } = null!;


    [Required(ErrorMessage = "Description is required.")]
    [Display(Name = "Description", Prompt = "Enter Description")]
    public string description { get; set; } = null!;

    public List<NewMembershipBenefitForm> Benefits { get; set; } = new List<NewMembershipBenefitForm>();


    [Required(ErrorMessage = "pricing is required.")]
    [Display(Name = "Pricing", Prompt = "Enter Pricing")]
    public decimal pricing {  get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    [Display(Name = "Monthly Duration", Prompt = "Enter Monthly Duration in months")]
    public int monthlyDuration { get; set; }
}
