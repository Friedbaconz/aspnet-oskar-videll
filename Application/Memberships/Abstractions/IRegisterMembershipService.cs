using Application.Common.Results;
using Application.Memberships.Inputs;
using Application.Users.Inputs;

namespace Application.Memberships.Abstractions
{
    public interface IRegisterMembershipService
    {
        Task<Result<string?>> ExecuteAsync(RegisterMemebershipInput input, CancellationToken ct = default);
    }
}