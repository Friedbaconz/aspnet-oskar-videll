using Domain.Aggregates.Memberships;
namespace Application.Memberships.Abstractions;

public interface IMembershipService
{
    Task<IReadOnlyList<Membership>> GetMembershipsAsync(CancellationToken ct= default);
    Task<Membership?> GetMembershipByIdAsync(string id, CancellationToken ct = default);

}
