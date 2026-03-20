using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts;

public sealed class CoreFitnessDbContext(DbContextOptions<CoreFitnessDbContext> options) : DbContext(options)
{
    public DbSet<UserEntity> Users => Set<UserEntity>();

    public DbSet<WorkoutEntity> Workouts => Set<WorkoutEntity>();

    public DbSet<MembershipEntity> Memberships => Set<MembershipEntity>();

    public DbSet<BookingEntity> Bookings => Set<BookingEntity>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("Users");

            entity.HasKey(e => e.UserID).HasName("PK_Users_ID");

            entity.Property(entity => entity.UserID)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Firstname)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Lastname)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Password)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Phonenumber)
                .IsUnicode(false)
                .IsRequired(false)
                .HasMaxLength(20);

            entity.Property(e => e.MembershipStatus)
                .IsRequired(false)
                .HasMaxLength(50);

            entity.HasOne(e => e.Membership)
                .WithMany(m => m.Users)
                .HasForeignKey("MembershipID")
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_Users_ID");

            entity.HasIndex(e => e.Email, "UQ_Users_Email")
                .IsUnique();

            entity.ToTable(tb => tb.HasCheckConstraint("CK_User_Email_NotEmpty", "LTRIM(RTRIM('Email')) <> ''"));

        });

        modelBuilder.Entity<MembershipEntity>(entity =>
        {
            entity.ToTable("Memberships");

            entity.HasKey(e => e.MembershipID).HasName("PK_Memberships_ID");

            entity.Property(e => e.MembershipID)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(e => e.Name, "UQ_Memberships_Name")
                .IsUnique();
        });

        modelBuilder.Entity<WorkoutEntity>(entity =>
        {
            entity.ToTable("Workouts");

            entity.HasKey(e => e.WorkoutID).HasName("PK_Workouts_ID");

            entity.Property(e => e.WorkoutID)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.WorkoutName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Instructor)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.Time)
                .IsRequired();
        });

        modelBuilder.Entity<WorkoutEntity>()
            .HasMany(m => m.Users)
            .WithMany(r => r.Workouts)
            .UsingEntity<BookingEntity>(

                r => r.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Bookings_UserID"),

                m => m.HasOne(e => e.Workout)
                    .WithMany()
                    .HasForeignKey(e => e.WorkoutID)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Bookings_WorkoutID"),

                e =>
                {
                    e.ToTable("Bookings");

                    e.HasKey(b => new { b.UserID, b.WorkoutID, b.BookingID }).HasName("PK_Bookings_ID");

                    e.Property(b => b.BookingID)
                        .IsRequired()
                        .ValueGeneratedOnAdd();

                    e.Property(b => b.UserID);

                    e.Property(b => b.WorkoutID);

                }
            );
    }
}
