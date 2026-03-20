

using Domain.Abstractions.Repositories;
using Domain.Aggregates.Memberships;

namespace Application.Memberships;

public sealed class MembershipService(IMembershipRepository repo) : IMembershipService
{
    public async Task<IReadOnlyList<Membership>> GetMembershipsAsync(CancellationToken ct = default)
    {
        var memberships = await repo.GetAllAsync(ct);
        return memberships;
    }
}
