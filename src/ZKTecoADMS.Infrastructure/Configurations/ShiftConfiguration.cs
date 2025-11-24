using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.HasKey(s => s.Id);
        
        builder.HasIndex(s => s.EmployeeUserId);
        
        builder.Property(s => s.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(s => s.UpdatedAt).HasDefaultValueSql("NOW()");
        
        // Relationship: ApplicationUser -> RequestedShifts
        builder.HasOne(s => s.ApplicationUser)
            .WithMany(u => u.RequestedShifts)
            .HasForeignKey(s => s.EmployeeUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}
