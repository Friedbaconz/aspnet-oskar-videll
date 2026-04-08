

using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Domain.Aggregates.Workouts;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Users;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users;

public class UserRepository(CoreFitnessDbContext context) : RepositoryBase<User, string, UserEntity, CoreFitnessDbContext>(context), IUserRepository
{
    public string GetUserId(User model)
    {
        return model.UserId;
    }

    public override string GetId(User model)
    {
        return model.Id;
    }

    public async Task<bool> DeleteAsync(User model, CancellationToken ct = default)
    {
        try
        {
            var id = GetId(model);
            var entity = await Set.FindAsync([id], ct);
            if (entity is null)
                return false;
            Set.Remove(entity);
            await _Context.SaveChangesAsync(ct);
            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<User?> GetUserByUserIdAsync(string UserId, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FirstOrDefaultAsync(e => e.UserId == UserId, ct);
            return entity is null ? default : ToDomainModel(entity);
        }
        catch
        {
            throw;
        }
    }
    protected override void ApplyPropertyUpdates(UserEntity entity, User model)
    {
        var MembershipEntity = context.Memberships.FirstOrDefault(m => m.MembershipID == model.MembershipId);

        if (MembershipEntity != null) {

            entity.Membership = MembershipEntity;
            entity.MembershipID = MembershipEntity.MembershipID;
        }

        var Workouts = new List<WorkoutEntity>();

        foreach (var workout in model.WorkoutsId)
        {
            var existing = context.Workouts.FirstOrDefault(e => e.WorkoutID == workout);

            if (existing != null)
            {
                Workouts.Add(existing);
            }
        }

        entity.Firstname = model.FirstName;
        entity.Lastname = model.LastName;
        entity.Phonenumber = model.Phonenumber;
        entity.MembershipStatus = model.Status;
        entity.ProfileImageUri = model.ProfileImageUri;
        entity.Workouts = Workouts;

    }



    protected override User ToDomainModel(UserEntity entity)
    {
        var MembershipId = context.UserEntites.Where(e => e.MembershipID == entity.MembershipID).Select(e => e.MembershipID).FirstOrDefault();

        var Workoutsid = new List<string>();

        foreach ( var workout in entity.Workouts)
        {
            var existing = context.UserEntites.Where(e => e.workoutId == workout.WorkoutID).Select(e => e.workoutId).FirstOrDefault();

            if (existing != null)
            {
                Workoutsid.Add(existing);
            }
        }

        var model = User.Create
            (
            entity.Id,
            entity.UserId,
            entity.Firstname,
            entity.Lastname,
            entity.Phonenumber,
            entity.MembershipStatus,
            entity.ProfileImageUri,
            MembershipId,
            Workoutsid
            );

        return model;
    }

    public override UserEntity ToEntity(User model)
    {
        var MembershipEntity = context.Memberships.FirstOrDefault(m => m.MembershipID == model.MembershipId);

        var Workouts = new List<WorkoutEntity>();

        foreach (var workout in model.WorkoutsId)
        {
            var existing = context.Workouts.FirstOrDefault(e => e.WorkoutID == workout);

            if (existing != null)
            {
                Workouts.Add(existing);
            }
        }


        var entity = new UserEntity
        {
            Id = model.Id,
            UserId = model.UserId,
            Firstname = model.FirstName,
            Lastname = model.LastName,
            Phonenumber = model.Phonenumber,
            MembershipStatus = model.Status,
            ProfileImageUri = model.ProfileImageUri,
            MembershipID = MembershipEntity.MembershipID,
            Workouts = Workouts,
            Membership = MembershipEntity
        };
        
        return entity;
    }

}
