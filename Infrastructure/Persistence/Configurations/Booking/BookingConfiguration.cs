using Infrastructure.Persistence.Entities.Booking;
using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Booking;

internal class BookingConfiguration : IEntityTypeConfiguration<BookingEntity>
{
    public void Configure(EntityTypeBuilder<BookingEntity> builder)
    {
        builder.HasOne(e => e.User)
        .WithMany()
        .HasForeignKey(e => e.UserID)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Bookings_UserID");

        builder.HasOne(e => e.Workout)
        .WithMany()
        .HasForeignKey(e => e.WorkoutID)
        .OnDelete(DeleteBehavior.Cascade)
        .HasConstraintName("FK_Bookings_WorkoutID");

        builder.ToTable("Bookings");
        builder.HasKey(b => new { b.BookingID}).HasName("PK_Bookings_IDUQ");
        builder.Property(b => b.BookingID)
            .IsRequired()
            .ValueGeneratedOnAdd();
        builder.Property(b => b.UserID)
            .IsRequired();
        builder.Property(b => b.WorkoutID)
            .IsRequired();
    }
}