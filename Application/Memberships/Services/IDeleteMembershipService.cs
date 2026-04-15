using Application.Common.Results;

namespace Application.Memberships.Services
{
    public interface IDeleteMembershipService
    {
        Task<Result> ExecuteAsync(string id, CancellationToken ct = default);
    }
}