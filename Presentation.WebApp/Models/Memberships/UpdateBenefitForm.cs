using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Memberships
{
    public class UpdateBenefitForm
    {
        public int id { get; set; } = 0;

        [Required(ErrorMessage = "Benefit is required.")]
        [Display(Name = "Benefit", Prompt = "Enter Benefit Description")]
        public string benefit { get; set; } = null!;

        public string MembershipId { get; set; } = null!;
    }
}
