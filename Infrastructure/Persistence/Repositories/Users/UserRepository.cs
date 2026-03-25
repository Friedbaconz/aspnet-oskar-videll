

using Domain.Abstractions.Repositories.Users;
using Domain.Aggregates.Memberships;
using Domain.Aggregates.Users;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories.Users;

public class UserRepository(CoreFitnessDbContext context) : RepositoryBase<User, string, UserEntity, CoreFitnessDbContext>(context), IUserRepository
{
    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    public Guid GetUserId(User model)
    {
        return model.UserId;
    }

    public override string GetId(User model)
    {
        return model.Id;
    }

    public Task<User?> GetUserByUserIdAsync(Guid UserId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
    protected override void ApplyPropertyUpdates(UserEntity entity, User model)
    {
        entity.Firstname = model.FirstName;
        entity.Lastname = model.LastName;
        entity.Email = model.Email;
        entity.Password = model.PasswordHash;
        entity.Phonenumber = model.Phonenumber;
        entity.MembershipStatus = model.Status;
        entity.ProfileImageUri = model.ProfileImageUri;
        entity.MembershipID = model.MembershipId;
        entity.Membership = model.Membership != null ? new MembershipEntity
        {
            MembershipID = model.Membership.Id,
            Name = model.Membership.Name,
            Description = model.Membership.Description,
            Pricing = model.Membership.Pricing
        } : null;
    }



    protected override User ToDomainModel(UserEntity entity)
    {
        var model = User.Create(
            entity.Id,
            entity.UserID,
            entity.Firstname,
            entity.Lastname,
            entity.Email,
            entity.Password,
            entity.Phonenumber,
            entity.MembershipStatus,
            entity.ProfileImageUri,
            entity.MembershipID,
            entity.Membership = context.Memberships.SingleOrDefault(b => b.MembershipID == entity.MembershipID),

    }

    public override UserEntity ToEntity(User model)
    {
        throw new NotImplementedException();
    }

}
