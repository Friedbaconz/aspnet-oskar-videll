using Infrastructure.Persistence.Entities.Workouts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.Workout;

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
