

using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Users;
using System.Globalization;

namespace Application.Users.Services;

public class RegisterUserAccountService(IidentityService identityService, IUserRepository userRepository) : IRegisterUserAccountService
{
    public async Task<Result<string?>> ExecuteAsync(RegisterUserAccountInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentNullException("Input must be provided");

            var identityResult = await identityService.CreateUserInAsync(input.Email, input.Password, ct);
            if (!identityResult.Success || string.IsNullOrWhiteSpace(identityResult.value))
                return Result<string?>.Error(identityResult?.ErrorMessage ?? "Unable to create user account");

            var user = User.Create(identityResult.value);

            await userRepository.AddAsync(user, ct);

            return Result<string?>.Ok(user.Id);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }

}
