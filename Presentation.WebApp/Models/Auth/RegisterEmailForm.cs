using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Auth;

public class RegisterEmailForm
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress (ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email Address", Prompt ="Enter Email Address")]
    public string Email { get; set; } = null!;

}
