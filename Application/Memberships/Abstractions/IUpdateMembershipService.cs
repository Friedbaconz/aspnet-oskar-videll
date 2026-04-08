using Domain.Aggregates.Memberships;
using Application.Memberships.Inputs;

namespace Application.Memberships.Abstractions
{
    public interface IUpdateMembershipService
    {
        Task<bool> ConnectMembershipWithUserAsync(IConnectMembershipWithUserInput input, CancellationToken ct = default);
    }
}