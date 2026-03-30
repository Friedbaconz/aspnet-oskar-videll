

using Infrastructure.Persistence.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public UserEntity? User { get; set; }

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
}
