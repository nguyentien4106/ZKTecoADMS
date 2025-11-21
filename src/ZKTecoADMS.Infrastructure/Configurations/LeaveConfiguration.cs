using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class LeaveConfiguration : IEntityTypeConfiguration<Leave>
{
    public void Configure(EntityTypeBuilder<Leave> builder)
    {
        builder.HasKey(l => l.Id);
        
        builder.HasIndex(l => l.ApplicationUserId);
        
        builder.Property(l => l.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(l => l.UpdatedAt).HasDefaultValueSql("NOW()");
        
        // Relationship: ApplicationUser -> RequestedLeaves
        builder.HasOne(l => l.ApplicationUser)
            .WithMany(u => u.RequestedLeaves)
            .HasForeignKey(l => l.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Relationship: ApprovedByUser -> ApprovedLeaves
        builder.HasOne(l => l.ApprovedByUser)
            .WithMany(u => u.ApprovedLeaves)
            .HasForeignKey(l => l.ApprovedByUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}
