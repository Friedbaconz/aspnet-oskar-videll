using Application.Common.Results;
using Application.Users.Abstractions;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Users;

namespace Application.Users.Services;

public class GetUserProfileService(IUserRepository userRepository) : IGetUserProfileService
{
    public async Task<Result<User>> ExecuteAsync(string userid, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userid))
                throw new ArgumentException("User ID must be provided");

            var user = await userRepository.GetUserByUserIdAsync(userid, ct);
            return user is null
                ? Result<User>.NotFound("User not found")
                : Result<User>.Ok(user);


        }
        catch (Exception ex)
        {
            return Result<User>.Error(ex.Message);
        }
    }

}
