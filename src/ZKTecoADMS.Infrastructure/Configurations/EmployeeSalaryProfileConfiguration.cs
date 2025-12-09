using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class EmployeeSalaryProfileConfiguration : IEntityTypeConfiguration<EmployeeSalaryProfile>
{
    public void Configure(EntityTypeBuilder<EmployeeSalaryProfile> builder)
    {
        builder.ToTable("EmployeeSalaryProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.EffectiveDate)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

        builder.HasOne(x => x.Employee)
            .WithMany(x => x.SalaryProfiles)
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.SalaryProfile)
            .WithMany(x => x.EmployeeSalaryProfiles)
            .HasForeignKey(x => x.SalaryProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.EmployeeId, x.IsActive });
        builder.HasIndex(x => x.SalaryProfileId);
        builder.HasIndex(x => x.EffectiveDate);
    }
}
