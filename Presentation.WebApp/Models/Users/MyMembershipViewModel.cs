using Presentation.WebApp.Models.Memberships;
using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Users;

public class MyMembershipViewModel
{
    [Required(ErrorMessage = "Membership ID is required.")]
    [Display(Name = "Membership ID", Prompt = "Enter Membership ID")]
    public string MembershipId { get; set; } = null!;

    [Required(ErrorMessage = "Membership Name is required.")]
    [Display(Name = "Membership Name", Prompt = "Enter Membership Name")]
    public string MembershipName { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    [Display(Name = "Description", Prompt = "Enter Description")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "Status is required.")]
    [Display(Name = "Status", Prompt = "Enter Status")]
    public string status { get; set; } = null!;


    public IEnumerable<string> Benefits { get; set; } = new List<string>();

    public MembershipViewModel MembershipViewModel { get; set; } = new MembershipViewModel();
}
