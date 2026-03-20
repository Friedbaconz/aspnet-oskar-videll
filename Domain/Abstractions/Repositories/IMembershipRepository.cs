
using Domain.Aggregates.Memberships;
namespace Domain.Abstractions.Repositories;

public interface IMembershipRepository : IRepositoryBase<Membership, Guid>
{
}
