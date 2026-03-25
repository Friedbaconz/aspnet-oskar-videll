using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;

namespace Infrastructure.Persistence.Repositories.Memberships;

public sealed class MembershipRepository(CoreFitnessDbContext context) : RepositoryBase<Membership, Guid, MembershipEntity, CoreFitnessDbContext>(context), IMembershipRepository
{

    public override Guid GetId(Membership model)
    {
        return model.Id;
    }

    public override MembershipEntity ToEntity(Membership model)
    {
        var entity = new MembershipEntity
        {
            MembershipID = model.Id,
            Name = model.Name,
            Description = model.Description,
            Benefits = null,
            Type = model.Type,
            Status = model.Status,
            Pricing = model.Pricing,
            DurationInMonths = model.MonthlyDuration,
            Users = null
        };

        return entity;
    }

    protected override void ApplyPropertyUpdates(MembershipEntity entity, Membership model)
    {
        
    }

    protected override Membership ToDomainModel(MembershipEntity entity)
    {



        var membership = Membership.Create(
            entity.MembershipID,
            entity.Name,
            entity.Description,
            null,
            entity.Type,
            entity.Status,
            entity.Pricing,
            entity.DurationInMonths,
            null
        );

        return membership;
    }
}
