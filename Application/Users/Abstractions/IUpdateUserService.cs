using Application.Common.Results;
using Application.Users.Inputs;
using Domain.Aggregates.Users;

namespace Application.Users.Abstractions
{
    public interface IUpdateUserService
    {
        Task<Result<User>> ExecuteAsync(UpdateUserProfileInput input, CancellationToken ct = default);
    }
}