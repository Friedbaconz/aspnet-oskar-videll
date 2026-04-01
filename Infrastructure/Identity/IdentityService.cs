

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
        var user = await userManager.FindByEmailAsync(email);
        if (user is not null) 
        { 
            return Result<string?>.Conflict($"A user with the email {email} already exists.");
        }

        var newUser = ApplicationUser.Create(email);

        var result = await userManager.CreateAsync(newUser, password);
        return !result.Succeeded 
            ? Result<string?>.Error("Failed to create user.") 
            : Result<string?>.Ok(newUser.Id);

    }

    public Task<Result> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default)
    {
        var result = signInManager.PasswordSignInAsync(email, password, rememberMe, false);
        return !result.Result.Succeeded 
            ? Task.FromResult(Result.Error("Invalid login attempt.")) 
            : Task.FromResult(Result.Ok());
    }



    public Task SignOutAsync(CancellationToken ct = default)
    {
        return signInManager.SignOutAsync();
    }
}
