using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class EmployeeSalaryProfileConfiguration : IEntityTypeConfiguration<EmployeeSalaryProfile>
{
    public void Configure(EntityTypeBuilder<EmployeeSalaryProfile> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.EffectiveDate)
            .IsRequired();

        builder.Property(x => x.EndDate)
            .IsRequired(false)
            .HasDefaultValue(null);

        builder.Property(x => x.IsActive)
            .IsRequired();

        builder.Property(x => x.Notes)
            .HasMaxLength(500);

    }
}
