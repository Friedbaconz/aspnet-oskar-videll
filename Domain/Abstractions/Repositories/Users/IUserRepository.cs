using Domain.Aggregates.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions.Repositories.Users;

public interface IUserRepository : IRepositoryBase<User, Guid>
{
}
