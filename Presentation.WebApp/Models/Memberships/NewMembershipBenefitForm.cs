using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Memberships;

public class NewMembershipBenefitForm
{
    public string? id { get; set; }

    [Required(ErrorMessage = "Benefit description is required.")]
    [Display(Name = "Benefit Description", Prompt = "Enter Benefit Description")]
    public string benefit { get; set; } = null!;
}
