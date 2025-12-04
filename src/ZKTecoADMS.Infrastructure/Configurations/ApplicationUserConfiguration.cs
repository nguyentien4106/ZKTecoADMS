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
        
        // One-to-One relationship with Employee (configured from ApplicationUser side)
        // When ApplicationUser is deleted, the Employee's ApplicationUserId will be set to null
        builder.HasOne(u => u.Employee)
            .WithOne(e => e.ApplicationUser)
            .HasForeignKey<Employee>(e => e.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
        
        // Many-to-One relationship with Manager (ApplicationUser)
        // One manager can manage many employees
        
        builder.HasOne(e => e.Manager)
            .WithMany(m => m.ManagedEmployees)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

    }
}
