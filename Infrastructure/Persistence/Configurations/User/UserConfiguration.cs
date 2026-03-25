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

        builder.HasKey(e => e.Id).HasName("PK_Users_ID");

        builder.Property(e => e.Id)
            .IsRequired();

        builder.Property(e => e.UserID)
            .IsRequired();

        builder.HasOne<ApplicationUser>()
            .WithOne(u => u.User)
            .HasForeignKey<UserEntity>(e => e.UserID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.Firstname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Lastname)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.Password)
            .IsRequired()
            .HasMaxLength(255);

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

        builder.HasOne(e => e.Membership)
            .WithMany(m => m.Users)
            .HasForeignKey("MembershipID")
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Users_ID");

        builder.HasIndex(e => e.Email, "UQ_Users_Email")
            .IsUnique();

        builder.HasIndex(e => e.UserID)
            .IsUnique();

        builder.ToTable(tb => tb.HasCheckConstraint("CK_User_Email_NotEmpty", "LTRIM(RTRIM('Email')) <> ''"));
    }
}
