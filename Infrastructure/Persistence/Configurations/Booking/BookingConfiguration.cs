using Infrastructure.Persistence.Entities.Booking;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Booking;

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

                    e.HasKey(b => new { b.UserID, b.WorkoutID, b.BookingID }).HasName("PK_Bookings_IDUQ");

                    e.Property(b => b.BookingID)
                        .IsRequired();

                    e.Property(b => b.UserID);

                    e.Property(b => b.WorkoutID);

                }
            );
    }
}