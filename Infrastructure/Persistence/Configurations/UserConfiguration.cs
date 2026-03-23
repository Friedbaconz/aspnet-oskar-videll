
using Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(e => e.UserID).HasName("PK_Users_ID");

        builder.Property(entity => entity.UserID)
            .ValueGeneratedOnAdd();

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

        builder.Property(e => e.MembershipStatus)
            .IsRequired(false)
            .HasMaxLength(50);

        builder.HasOne(e => e.Membership)
            .WithMany(m => m.Users)
            .HasForeignKey("MembershipID")
            .OnDelete(DeleteBehavior.SetNull)
            .HasConstraintName("FK_Users_ID");

        builder.HasIndex(e => e.Email, "UQ_Users_Email")
            .IsUnique();

        builder.ToTable(tb => tb.HasCheckConstraint("CK_User_Email_NotEmpty", "LTRIM(RTRIM('Email')) <> ''"));
    }
}
