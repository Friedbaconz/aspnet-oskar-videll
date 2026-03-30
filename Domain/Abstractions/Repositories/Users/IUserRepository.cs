using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Repositories.Users;

public interface IUserRepository : IRepositoryBase<User, string>
{
    Task<User?> GetUserByUserIdAsync(string UserId, CancellationToken ct = default);

    string GetUserId(User model);
}
