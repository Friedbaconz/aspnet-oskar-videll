using Domain.Abstractions.Repositories.Memberships;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Memberships;

public sealed class MembershipRepository(CoreFitnessDbContext context) : RepositoryBase<Membership, int, MembershipEntity, CoreFitnessDbContext>(context), IMembershipRepository
{
    public async Task<Membership> Connectwithuserasync(string userid, Membership Membership)
    {

        var list = new List<string>();

        list = context.UserEntites
            .Where(u => u.MembershipID == Membership.Id)
            .Select(u => u.UserId)
            .ToList();

        list.Add(userid);

        var model = Membership.Create(
            Membership.Id,
            Membership.Name,
            Membership.Description,
            Membership.Benefits,
            Membership.Status,
            Membership.Type,
            Membership.Pricing,
            Membership.MonthlyDuration,
            list
        );

        return model;
    }

    public override int GetId(Membership model)
    {
        return model.Id;
    }

    public override MembershipEntity ToEntity(Membership model)
    {
        var benefits = new List<MembershipBenefitEntity>();

        foreach (var benefit in model.Benefits)
        {
            var existing = context.MembershipBenefits.FirstOrDefault(e => e.MembershipID == model.Id && e.Benefit == benefit);
            
            if (existing != null)
            {
                benefits.Add(existing);
            }
        }

        var members = new List<UserEntity>();

        foreach (var member in model.Users)
        {
            var existing = context.UserEntites.FirstOrDefault(e => e.UserId == member);

            if (existing != null)
            {
                members.Add(existing);
            }
        }

        var entity = new MembershipEntity
        {
            MembershipID = model.Id,
            Name = model.Name,
            Description = model.Description,
            Benefits = benefits,
            Type = model.Type,
            Status = model.Status,
            Pricing = model.Pricing,
            DurationInMonths = model.MonthlyDuration,
            Users = members
        };


        return entity;
    }

    protected override void ApplyPropertyUpdates(MembershipEntity entity, Membership model)
    {
        var NewEntity = context.Memberships.FirstOrDefault(m => m.MembershipID == entity.MembershipID);
        var users = context.UserEntites.Where(u => u.MembershipID == entity.MembershipID).ToList();
        if (NewEntity != null)
        {
            entity.Benefits = NewEntity.Benefits;
            entity.Users = NewEntity.Users;
        }
        else
        {
            entity.Benefits = new List<MembershipBenefitEntity>();
            entity.Users = new List<UserEntity>();
        }

        entity.Name = model.Name;
        entity.Description = model.Description;
        entity.Status = model.Status;
        entity.Type = model.Type;
        entity.Pricing = model.Pricing;
        entity.DurationInMonths = model.MonthlyDuration;
    }

    protected override Membership ToDomainModel(MembershipEntity entity)
    {
        var benefits = context.MembershipBenefits
            .Where(b => b.MembershipID == entity.MembershipID)
            .Select(b => b.Benefit)
            .ToList();

        var users = context.UserEntites
            .Where(u => u.MembershipID == entity.MembershipID)
            .Select(u => u.UserId)
            .ToList();


        var model = Membership.Create(
            entity.MembershipID,
            entity.Name,
            entity.Description,
            benefits,
            entity.Status,
            entity.Type,
            entity.Pricing,
            entity.DurationInMonths,
            users
        );

        return model;
    }
}
