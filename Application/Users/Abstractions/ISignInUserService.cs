using Application.Common.Results;
using Application.Users.Inputs;

namespace Application.Users.Abstractions
{
    public interface ISignInUserService
    {
        Task<Result> ExecuteAsync(SignInInput input, CancellationToken ct = default);
    }
}