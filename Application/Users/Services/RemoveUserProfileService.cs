using Application.Common.Results;
using Application.Users.Abstractions;
using Domain.Abstractions.Repositories.Users;

namespace Application.Users.Services;

public class RemoveUserProfileService(IUserRepository userRepository) : IRemoveUserService
{
    public async Task<Result> ExecuteAsync(string userid, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userid))
                throw new ArgumentException("User ID must be provided");
            var user = await userRepository.GetUserByUserIdAsync(userid, ct);
            if (user is null)
                return Result.NotFound("User not found");
            var success = await userRepository.DeleteAsync(user, ct);
            return success
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}
