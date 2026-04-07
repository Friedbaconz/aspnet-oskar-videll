using Infrastructure.Identity;
using Infrastructure.Persistence.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations.User;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.Id).HasName("PK_ProfileUsers_Id");

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.Property(e => e.Firstname)
            .HasMaxLength(100);

        builder.Property(e => e.Lastname)
            .HasMaxLength(100);

        builder.Property(e => e.Phonenumber)
            .IsUnicode(false)
            .IsRequired(false)
            .HasMaxLength(20);

        builder.Property(e => e.ProfileImageUri)
            .IsUnicode(false)
            .IsRequired(false)
            .HasMaxLength(255);

        builder.Property(e => e.MembershipStatus)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.Property(e => e.MembershipID)
            .IsRequired(false);

        builder.Property(e => e.workoutId)
            .IsRequired(false);

        builder.HasOne(e => e.Membership)
            .WithMany(m => m.Users)
            .HasForeignKey("MembershipID")
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Membership_ID");

        builder.HasIndex(e => e.UserId)
            .IsUnique();

        builder.HasOne(x => x.User)
            .WithOne(x => x.Member)
            .HasForeignKey<UserEntity>(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
