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
        
        // Many-to-One relationship with Manager (ApplicationUser)
        // One manager can manage many employees
        
        builder.HasOne(e => e.Manager)
            .WithMany(m => m.ManagedEmployees)
            .HasForeignKey(e => e.ManagerId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
