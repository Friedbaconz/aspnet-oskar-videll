using Domain.Aggregates.Memberships;
namespace Domain.Abstractions.Repositories.Memberships;

public interface IMembershipRepository : IRepositoryBase<Membership, string>
{
    //connectwithuser

    Task<Membership> Connectwithuserasync(string userid, Membership Membership);
}
