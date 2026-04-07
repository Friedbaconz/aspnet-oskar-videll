

using Application.Abstractions.Identity;
using Application.Common.Results;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Workouts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IidentityService
{
    public async Task<Result<string?>> CreateUserInAsync(string email, string password, CancellationToken ct = default)
    {
        var exsistinguser = await userManager.FindByEmailAsync(email);
        if (exsistinguser is not null) 
        { 
            return Result<string?>.Conflict($"A user with the email {email} already exists.");
        }

        var user = ApplicationUser.Create(email);

        var result = await userManager.CreateAsync(user, password);
        return !result.Succeeded 
            ? Result<string?>.Error("Failed to create user.") 
            : Result<string?>.Ok(user.Id);

    }

    public async Task<Result<bool>> DeleteUserAsync(string email, CancellationToken ct = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null) 
        { 
            return Result<bool>.NotFound($"No user found with the email {email}.");
        }

        var result = await userManager.DeleteAsync(user);

        return !result.Succeeded 
            ? Result<bool>.Error("Failed to delete user.") 
            : Result<bool>.Ok(true);
    }

    public async Task<Result<bool>> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default)
    {
        var result = await signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        return !result.Succeeded ? Result<bool>.Error("Invalid email or password.") : Result<bool>.Ok(true);
    }



    public Task SignOutAsync(CancellationToken ct = default)
    {
        return signInManager.SignOutAsync();
    }
}
