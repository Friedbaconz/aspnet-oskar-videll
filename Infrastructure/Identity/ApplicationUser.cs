using Infrastructure.Persistence.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public UserEntity? Member { get; set; }

    public static ApplicationUser Create(string email, bool confirmed= true)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty.", nameof(email));
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = confirmed
        };

        return user;
    }

    public async Task<IdentityResult> SetPasswordAsync(UserManager<ApplicationUser> userManager, string password, CancellationToken ct = default)
    {
        if (userManager is null)
        {
            throw new ArgumentNullException(nameof(userManager));
        }
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));
        }
        var token = await userManager.GeneratePasswordResetTokenAsync(this);
        return await userManager.ResetPasswordAsync(this, token, password);
    }
}
