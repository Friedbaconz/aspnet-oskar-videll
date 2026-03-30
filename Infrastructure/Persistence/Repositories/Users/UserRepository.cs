

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
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public string GetUserId(User model)
    {
        return model.UserId;
    }

    public override string GetId(User model)
    {
        return model.Id;
    }

    public async Task<User?> GetUserByUserIdAsync(string UserId, CancellationToken ct = default)
    {
        try
        {
            var entity = await Set.FirstOrDefaultAsync(e => e.UserID == UserId, ct);
            return entity is null ? default : ToDomainModel(entity);
        }
        catch
        {
            throw;
        }
    }
    protected override void ApplyPropertyUpdates(UserEntity entity, User model)
    {

        entity.Firstname = model.FirstName;
        entity.Lastname = model.LastName;
        entity.Phonenumber = model.Phonenumber;
        entity.MembershipStatus = model.Status;
        entity.ProfileImageUri = model.ProfileImageUri;
        entity.MembershipID = model.MembershipId;
    }



    protected override User ToDomainModel(UserEntity entity)
    {

        var model = User.Create
            (
            entity.Id,
            entity.UserID,
            entity.Firstname,
            entity.Lastname,
            entity.Phonenumber,
            entity.MembershipStatus,
            entity.ProfileImageUri,
            entity.MembershipID
            );

        return model;
    }

    public override UserEntity ToEntity(User model)
    {

        var entity = new UserEntity
        {
            Id = model.Id,
            UserID = model.UserId,
            Firstname = model.FirstName,
            Lastname = model.LastName,
            Phonenumber = model.Phonenumber,
            MembershipStatus = model.Status,
            ProfileImageUri = model.ProfileImageUri,
            MembershipID = model.MembershipId
        };
        
        return entity;
    }

}
