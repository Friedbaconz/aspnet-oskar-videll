using Domain.Aggregates.Memberships;
namespace Application.Memberships.Services;

public interface IMembershipService
{
    Task<IReadOnlyList<Membership>> GetMembershipsAsync(CancellationToken ct= default);
    Task<Membership?> GetMembershipByIdAsync(string id, CancellationToken ct = default);

}
