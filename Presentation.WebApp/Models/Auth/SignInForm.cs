using System.ComponentModel.DataAnnotations;

namespace Presentation.WebApp.Models.Auth;

public class SignInForm
{
    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    [Display(Name = "Email Address", Prompt = "Enter Email Address")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password", Prompt = "Enter Password")]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }

}