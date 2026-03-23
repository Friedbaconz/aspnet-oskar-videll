

using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class WorkoutConfiguration : IEntityTypeConfiguration<WorkoutEntity>
{
    public void Configure(EntityTypeBuilder<WorkoutEntity> builder)
    {
        builder.ToTable("Workouts");

        builder.HasKey(e => e.WorkoutID).HasName("PK_Workouts_ID");

        builder.Property(e => e.WorkoutID)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.WorkoutName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Category)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Instructor)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.Time)
            .IsRequired();
    }
}

internal class BookingConfiguration : IEntityTypeConfiguration<WorkoutEntity>
{
    public void Configure(EntityTypeBuilder<WorkoutEntity> builder)
    {
        builder.HasMany(m => m.Users)
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