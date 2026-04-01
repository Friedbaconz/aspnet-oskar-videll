namespace Presentation.WebApp.Models.Auth;

public class ResetPasswordForm
{
    public string Email { get; set; } = null!;

    public string NewPassword { get; set; } = null!;
}
