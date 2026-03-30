using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;

namespace Infrastructure.Persistence.Repositories.Memberships;

public sealed class MembershipRepository(CoreFitnessDbContext context) : RepositoryBase<Membership, int, MembershipEntity, CoreFitnessDbContext>(context), IMembershipRepository
{

    public override int GetId(Membership model)
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
        var users = new List<User>();

        foreach (var userEntity in entity.Users)
        {
            users.Add(User.Create(
                userEntity.Id,
                userEntity.UserId,
                userEntity.Firstname,
                userEntity.Lastname,
                userEntity.Phonenumber,
                userEntity.MembershipStatus,
                userEntity.CreatedAt,
                userEntity.ProfileImageUri,
                userEntity.MembershipID
            ));
        }


        var benefits = new List<MembershipBenefits>();

        foreach (var benefitEntity in entity.Benefits)
        {
            benefits.Add(MembershipBenefits.Create(
                benefitEntity.MembershipBenefitID,
                benefitEntity.MembershipID,
                benefitEntity.Benefit,
                benefitEntity.Membership != null ? ToDomainModel(benefitEntity.Membership) : null
            ));
        }


        var membership = Membership.Create(
            entity.MembershipID,
            entity.Name,
            entity.Description,
            benefits,
            entity.Type,
            entity.Status,
            entity.Pricing,
            entity.DurationInMonths,
            users
        );

        return membership;
    }
}
