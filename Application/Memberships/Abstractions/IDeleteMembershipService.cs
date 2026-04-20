using Application.Common.Results;

namespace Application.Memberships.Abstractions
{
    public interface IDeleteMembershipService
    {
        Task<Result> ExecuteAsync(string id, CancellationToken ct = default);

        Task<Result> DeleteBenefit(string id, CancellationToken ct = default);
    }
}