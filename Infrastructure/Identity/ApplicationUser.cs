

using Infrastructure.Persistence.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public UserEntity? User { get; set; }
}
