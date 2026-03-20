using Domain.Aggregates.Memberships;
namespace Application.Memberships;

public interface IMembershipService
{
    Task<IReadOnlyList<Membership>> GetMembershipsAsync(CancellationToken ct= default);

}
