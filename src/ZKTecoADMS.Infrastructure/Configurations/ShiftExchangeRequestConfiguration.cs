using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class ShiftExchangeRequestConfiguration : IEntityTypeConfiguration<ShiftExchangeRequest>
{
    public void Configure(EntityTypeBuilder<ShiftExchangeRequest> builder)
    {
        builder.ToTable("ShiftExchangeRequests");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(e => e.RejectionReason)
            .HasMaxLength(500);

        builder.Property(e => e.Status)
            .IsRequired();

        // Relationships
        builder.HasOne(e => e.Shift)
            .WithMany()
            .HasForeignKey(e => e.ShiftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Requester)
            .WithMany()
            .HasForeignKey(e => e.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.TargetEmployee)
            .WithMany()
            .HasForeignKey(e => e.TargetEmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(e => e.ShiftId);
        builder.HasIndex(e => e.RequesterId);
        builder.HasIndex(e => e.TargetEmployeeId);
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.CreatedAt);
    }
}
