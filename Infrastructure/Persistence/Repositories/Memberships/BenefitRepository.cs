using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Persistence.Repositories.Memberships;

public sealed class BenefitRepository(CoreFitnessDbContext context) : RepositoryBase<MembershipBenefits, int, MembershipBenefitEntity, CoreFitnessDbContext>(context), IBenefitRepository
{
    public override int GetId(MembershipBenefits model)
    {
        return model.Id;
    }

    public override MembershipBenefitEntity ToEntity(MembershipBenefits model)
    {

        var entity = new MembershipBenefitEntity
        {
            MembershipBenefitID = model.Id,
            Benefit = model.Benefit,
            MembershipID = model.MembershipId,
            Membership = context.Memberships.FirstOrDefault(m => m.MembershipID == model.MembershipId)
        };

        return entity;
    }

    protected override void ApplyPropertyUpdates(MembershipBenefitEntity entity, MembershipBenefits model)
    {
        var existingMembership = context.Memberships.FirstOrDefault(m => m.MembershipID == model.MembershipId);
        if (existingMembership != null)
        {
            entity.Membership = existingMembership;
        }

        entity.Benefit = model.Benefit;
        entity.MembershipID = model.MembershipId;
        entity.MembershipBenefitID = model.Id;
    }

    protected override MembershipBenefits ToDomainModel(MembershipBenefitEntity entity)
    {

        var model = MembershipBenefits.Create(
            entity.MembershipBenefitID,
            entity.MembershipID,
            entity.Benefit
        );


        return model;
    }
}
