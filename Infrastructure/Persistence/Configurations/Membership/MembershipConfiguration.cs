
using Domain.Aggregates.Memberships;
using Infrastructure.Persistence.Entities.Memberships;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Infrastructure.Persistence.Configurations.Membership;

internal class MembershipConfiguration : IEntityTypeConfiguration<MembershipEntity>
{
    public void Configure(EntityTypeBuilder<MembershipEntity> entity)
    {
            entity.ToTable("Memberships");

            entity.HasKey(e => e.MembershipID).HasName("PK_Memberships_ID");

            entity.Property(e => e.MembershipID)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Pricing)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.DurationInMonths)
                .IsRequired();

            entity.HasIndex(e => e.Name, "UQ_Memberships_Name")
                .IsUnique();

    }
}

internal class MembershipBenefitsConfiguration : IEntityTypeConfiguration<MembershipBenefitEntity>
{
    public void Configure(EntityTypeBuilder<MembershipBenefitEntity> entity)
    {
        entity.ToTable("MembershipBenefits");
        entity.HasKey(e => e.MembershipBenefitID).HasName("PK_MembershipBenefits_ID");

        entity.Property(e => e.MembershipBenefitID)
            .ValueGeneratedOnAdd();

        entity.Property(e => e.MembershipID)
            .IsRequired();

        entity.Property(e => e.Benefit)
            .IsRequired()
            .HasMaxLength(500);

        entity.HasOne(e => e.Membership)
            .WithMany(m => m.Benefits)
            .HasForeignKey(e => e.MembershipID)
            .OnDelete(DeleteBehavior.Cascade)
            .HasConstraintName("FK_MembershipBenefits_MembershipID");

    }
}
