using Application.Common.Results;
using Application.Users.Inputs;

namespace Application.Users.Abstractions
{
    public interface IRegisterUserAccountService
    {
        Task<Result<string?>> ExecuteAsync(RegisterUserAccountInput input, CancellationToken ct = default);
    }
}