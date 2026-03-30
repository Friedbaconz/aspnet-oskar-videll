

using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Users.Abstractions;
using Application.Users.Inputs;

namespace Application.Users.Services;

public class SignInUserService(IidentityService identityService) : ISignInUserService
{
    public async Task<Result<string?>> ExecuteAsync(SignInInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentNullException("Input must be provided");

            var signInResult = await identityService.PasswordSignInAsync(input.Email, input.Password, input.RememberMe, ct);
            return !signInResult.Success
                ? Result<string?>.BadRequest("Invalid email or password")
                : Result<string?>.Ok("User signed in successfully");


        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }

}
