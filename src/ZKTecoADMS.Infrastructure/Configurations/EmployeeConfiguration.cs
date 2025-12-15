using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Pin);
        builder.HasIndex(e => e.ApplicationUserId)
            .IsUnique();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        
        builder.HasOne(i => i.Device)
            .WithMany(i => i.Employees)
            .HasForeignKey(i => i.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-One relationship configured in ApplicationUserConfiguration
        builder.HasOne(e => e.ApplicationUser)
            .WithOne(u => u.Employee)
            .HasForeignKey<Employee>(u => u.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-Many relationship with SalaryProfiles is configured in EmployeeSalaryProfileConfiguration
    }
}