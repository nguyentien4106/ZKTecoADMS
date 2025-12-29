using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.HasKey(e => e.Id);

        // Identity Information
        builder.Property(e => e.EmployeeCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.CompanyEmail)
            .IsRequired()
            .HasMaxLength(200);

        // Create unique index on EmployeeCode
        builder.HasIndex(e => e.EmployeeCode)
            .IsUnique()
            .HasDatabaseName("IX_Employees_EmployeeCode");

        // Create unique index on CompanyEmail
        builder.HasIndex(e => e.CompanyEmail)
            .IsUnique()
            .HasDatabaseName("IX_Employees_CompanyEmail");

        // Configure enums to be stored as integers
        builder.Property(e => e.WorkStatus)
            .HasConversion<int>();

        builder.Property(e => e.EmploymentType)
            .HasConversion<int>();

        // One manager (ApplicationUser) can manage many employees
        // Each employee must have a manager
        builder.HasOne(e => e.Manager)
            .WithMany() // Manager doesn't need navigation back to all employees they manage through Employee entity
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete of employees when manager is deleted
            .IsRequired(true);

        // DeviceUser relationship is configured in DeviceUserConfiguration.cs

        // Configure many-to-many relationship with Benefits through EmployeeBenefit
        builder.HasMany(e => e.EmployeeBenefits)
            .WithOne(eb => eb.Employee)
            .HasForeignKey(eb => eb.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
