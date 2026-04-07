using Application.Common.Results;

namespace Application.Users.Abstractions
{
    public interface IRemoveUserService
    {
        Task<Result> ExecuteAsync(string userId, CancellationToken ct = default);
    }
}