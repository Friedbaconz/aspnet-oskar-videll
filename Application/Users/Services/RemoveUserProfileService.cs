

using Application.Abstractions.Identity;
using Application.Common.Results;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Domain.Abstractions.Repositories.Users;

namespace Application.Users.Services;

public class RemoveUserProfileService(IidentityService identityService, IUserRepository userRepository) : IRemoveUserProfileService
{
    public async Task<Result<string?>> ExecuteAsync(RemoveUserInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
            {
                throw new ArgumentNullException("input must be provided");
            }

            var identityResult = await identityService.RemoveUserAsync(input.UserId);
            if (!identityResult.Success)
                return Result<string?>.Error(identityResult.ErrorMessage ?? "unable to delete profile");

            var user = await userRepository.GetByIdAsync(input.UserId);

            await userRepository.RemoveAsync(user, ct);

            return Result<string?>.Ok(user?.Id);
        }
        catch (Exception ex)
        {
            return Result<string?>.Error(ex.Message);
        }
    }

}
