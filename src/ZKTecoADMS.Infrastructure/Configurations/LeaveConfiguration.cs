using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.HasKey(l => l.Id);
        
        builder.HasIndex(l => l.EmployeeUserId);
        builder.HasIndex(l => l.ShiftId).IsUnique();
        
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt).HasDefaultValueSql("NOW()");
        
        // Relationship: ApplicationUser -> RequestedLeaves
        builder.HasOne(l => l.EmployeeUser)
            .WithMany(u => u.RequestedLeaves)
            .HasForeignKey(l => l.EmployeeUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship: Manager -> ManagedLeaves
        builder.HasOne(l => l.Manager)
            .WithMany(u => u.ManagedLeaves)
            .HasForeignKey(l => l.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Shift -> Leave (One-to-One Optional)
        builder.HasOne(l => l.Shift)
            .WithOne(s => s.Leave)
            .HasForeignKey<Leave>(l => l.ShiftId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
