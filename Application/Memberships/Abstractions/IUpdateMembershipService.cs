using Application.Common.Results;
using Application.Memberships.Inputs;
using Domain.Aggregates.Memberships;

namespace Application.Memberships.Abstractions
{
    public interface IUpdateMembershipService
    {
        Task<Result<Membership>> ExecuteAsync(UpdateMembershipInput input, List<UpdateMembershipBenefitInput> benefits, CancellationToken ct = default);
    }
}