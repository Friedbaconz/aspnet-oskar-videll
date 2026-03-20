

using Domain.Abstractions.Repositories;
using Domain.Aggregates.Memberships;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities;

namespace Infrastructure.Persistence.Repositories;

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
            Type = model.Type,
            Status = model.Status,
            Pricing = model.Pricing,
            DurationInMonths = model.MonthlyDuration
        };

        foreach (var benefit in model.Benefits)
        {
            entity.Benefits.Add(new MembershipBenefitEntity
            {
                Benefit = benefit,
                MembershipID = model.Id
            });
        }

        return entity;
    }

    protected override void ApplyPropertyUpdates(MembershipEntity entity, Membership model)
    {
        throw new NotImplementedException();
    }

    protected override Membership ToDomainModel(MembershipEntity entity)
    {
        var benefits = new List<string>();
        foreach (var benefit in entity.Benefits)
        {
            benefits.Add(benefit.Benefit);
        }

        var membership = Membership.Create(
            entity.MembershipID,
            entity.Name,
            entity.Description,
            benefits,
            entity.Type,
            entity.Status,
            entity.Pricing,
            entity.DurationInMonths
        );

        return membership;
    }
}
