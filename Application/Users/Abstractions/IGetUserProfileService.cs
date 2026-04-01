using Application.Common.Results;
using Domain.Aggregates.Users;

namespace Application.Users.Abstractions
{
    public interface IGetUserProfileService
    {
        Task<Result<User>> ExecuteAsync(string userid, CancellationToken ct = default);
    }

    public interface IGetUserProfileEmailService
    {
        Task<Result<User>> ExecuteAsync(string email, CancellationToken ct = default);
    }
}