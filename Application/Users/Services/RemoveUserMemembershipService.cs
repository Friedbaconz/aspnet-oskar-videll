using Application.Common.Results;
using Application.Users.Abstractions;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Users;

namespace Application.Users.Services;

public class RemoveUserMemembershipService(IUserRepository userRepository, IMembershipRepository repo) : IRemoveUserMemembershipService
{
    public async Task<Result> ExecuteAsync(string userid, string membershipid, CancellationToken ct = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userid))
                throw new ArgumentException("User ID must be provided");
            var user = await userRepository.GetUserByUserIdAsync(userid, ct);
            if (user is null)
                return Result.NotFound("User not found");
            var deleted = string.Empty;

            user.UpdateProfile(user.FirstName, user.LastName, user.Phonenumber, user.ProfileImageUri, deleted, user.WorkoutsId);
            
            var results = await userRepository.UpdateAsync(user);

            var membership = await repo.GetByIdAsync(membershipid);

            membership.Update(membership.Name, membership.Description, membership.Benefits, membership.Status, membership.Type, membership.Pricing, membership.MonthlyDuration, membership.Userid, membership.Users);

            var result = await repo.UpdateAsync(membership);

            return result
                ? Result.Ok()
                : Result.Error("Failed to delete user profile");
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message);
        }
    }
}