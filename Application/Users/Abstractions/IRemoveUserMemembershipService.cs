using Application.Common.Results;

namespace Application.Users.Abstractions
{
    public interface IRemoveUserMemembershipService
    {
        Task<Result> ExecuteAsync(string userid, string membershipid, CancellationToken ct = default);
    }
}