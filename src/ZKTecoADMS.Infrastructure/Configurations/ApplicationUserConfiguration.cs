using ZKTecoADMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(50);
        
        // Many-to-One relationship: Manager hierarchy
        // An ApplicationUser can have one manager (another ApplicationUser)
        // A manager (ApplicationUser) can manage many other ApplicationUsers
        builder.HasOne(u => u.Manager)
            .WithMany(m => m.ManagedEmployees)
            .HasForeignKey(u => u.ManagerId)
            .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete
            .IsRequired(false); // Manager is optional

        // One-to-One relationship with Employee
        // An ApplicationUser can be associated with one Employee record
        // This is configured from the Employee side, but we define the inverse here
        builder.HasOne(u => u.Employee)
            .WithOne(e => e.ApplicationUser)
            .HasForeignKey<ApplicationUser>(u => u.EmployeeId)
            .OnDelete(DeleteBehavior.SetNull) // If Employee is deleted, just clear the reference
            .IsRequired(false); // Employee association is optional (user might just be a manager)
    }
}
