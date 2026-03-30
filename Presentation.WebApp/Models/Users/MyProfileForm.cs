using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Users;

public class MyProfileForm
{
    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name", Prompt = "Enter First Name")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name", Prompt = "Enter Last Name")]
    public string LastName { get; set; } = null!;

    [Phone(ErrorMessage = "Please enter a valid phone number.")]
    [Display(Name = "Phone Number", Prompt = "(Optional) Enter Phone Number")]
    public string? PhoneNumber { get; set; }

    [Url(ErrorMessage = "Please enter a valid URL.")]
    [Display(Name = "Profile Image", Prompt = "Upload Profile Image")]
    public string? ProfileImageUri { get; set; }
}
