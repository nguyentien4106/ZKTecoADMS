using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class SalaryProfileConfiguration : IEntityTypeConfiguration<SalaryProfile>
{
    public void Configure(EntityTypeBuilder<SalaryProfile> builder)
    {
        builder.ToTable("SalaryProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Description)
            .HasMaxLength(500);

        builder.Property(x => x.RateType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(x => x.Rate)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(x => x.Currency)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(x => x.OvertimeMultiplier)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.HolidayMultiplier)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.NightShiftMultiplier)
            .HasColumnType("decimal(5,2)");

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.HasIndex(x => x.Name);
        builder.HasIndex(x => x.IsActive);
    }
}
