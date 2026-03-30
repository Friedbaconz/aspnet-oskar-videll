using Application.Common.Results;
using Application.Users.Inputs;

namespace Application.Users.Abstractions
{
    public interface IRemoveUserProfileService
    {
        Task<Result<string?>> ExecuteAsync(RemoveUserInput input, CancellationToken ct = default);
    }
}