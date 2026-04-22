using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Users;

public class MyAccountViewModel
{
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;
    public MyProfileForm ProfileForm { get; set; } = null!;

    public MyBookingViewModel? MyBookingViewModel { get; set; } = null;

    public MyMembershipViewModel? MyMembershipViewModel { get; set; } = new MyMembershipViewModel();
}
