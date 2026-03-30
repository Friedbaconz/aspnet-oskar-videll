using Infrastructure.Identity;
using Infrastructure.Persistence.Configurations.User;
using Infrastructure.Persistence.Entities.Booking;
using Infrastructure.Persistence.Entities.Memberships;
using Infrastructure.Persistence.Entities.Users;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public class CoreFitnessDbContext(DbContextOptions<CoreFitnessDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CoreFitnessDbContext).Assembly);
    }

    public DbSet<UserEntity> UserEntites => Set<UserEntity>();

    public DbSet<WorkoutEntity> Workouts => Set<WorkoutEntity>();

    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();

    public DbSet<MembershipBenefitEntity> MembershipBenefits => Set<MembershipBenefitEntity>();

    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();
}
