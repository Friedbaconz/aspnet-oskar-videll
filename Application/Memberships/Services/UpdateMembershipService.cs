using Application.Memberships.Abstractions;
using Application.Memberships.Inputs;
using Domain.Abstractions.Repositories.Memberships;
using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Memberships;

namespace Application.Memberships.Services;

public sealed class UpdateMembershipService(IMembershipRepository repo, IUserRepository urepo) : IUpdateMembershipService
{

    public async Task<bool> ConnectMembershipWithUserAsync(IConnectMembershipWithUserInput input, CancellationToken ct = default)
    {
        var membership = await repo.GetByIdAsync(input.MembershipId, ct);
        if(membership == null) return false;

        var Update = await repo.Connectwithuserasync(input.UserId, membership);

        Update.Update(membership.Name, membership.Description, membership.Benefits, membership.Status, membership.Type, membership.Pricing, membership.MonthlyDuration, input.UserId, membership.Users);

        var result = await repo.UpdateAsync(Update, ct);
        if(!result) return false;

        var user = await urepo.GetUserByUserIdAsync(input.UserId, ct);
        if(user == null) return false;

        user.UpdateProfile(user.FirstName, user.LastName, user.Phonenumber, user.ProfileImageUri, membership.Id, user.WorkoutsId);

        var uresult = await urepo.UpdateAsync(user, ct);

        if (!uresult) return false;

        return uresult;
    }
}
