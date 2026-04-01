

using Application.Common.Results;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;

namespace Application.Abstractions.Identity;

public interface IidentityService
{
    Task<Result<string?>> CreateUserInAsync(string email, string password, CancellationToken ct = default);
    Task<Result> PasswordSignInAsync(string email, string password, bool rememberMe, CancellationToken ct = default);
    Task SignOutAsync(CancellationToken ct = default);
}
