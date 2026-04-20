namespace Presentation.WebApp.Models.Memberships
{
    public class UpdateBenefitForm
    {
        public int id { get; set; } = 0;
        public string benefit { get; set; } = null!;
        public string MembershipId { get; set; } = null!;
    }
}
