using Application.Common.Results;
using Application.Users.Abstractions;
using Application.Users.Inputs;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Users;

namespace Application.Users.Services;

public class UpdateUserService(IUserRepository userRepository) : IUpdateUserService
{
    public async Task<Result<User>> ExecuteAsync(UpdateUserProfileInput input, CancellationToken ct = default)
    {
        try
        {
            if (input is null)
                throw new ArgumentException("input must be provided");

            var user = await userRepository.GetUserByUserIdAsync(input.UserId, ct);
            if (user is null)
                return Result<User>.NotFound($"User with user id '{input.UserId}' was not found");

            user.UpdateProfile(input.Firstname, input.Lastname, input.Phonenumber, input.ProfileImageUri);
            var result = await userRepository.UpdateAsync(user, ct);

            return !result
                ? Result<User>.NotFound($"User with user id '{user.UserId}' was not found")
                : Result<User>.Ok(user);

        }
        catch (Exception ex)
        {
            return Result<User>.Error(ex.Message);
        }
    }

}
