using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Pin);
        builder.HasIndex(e => e.ApplicationUserId)
            .IsUnique()
            .HasFilter("\"ApplicationUserId\" IS NOT NULL");
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        
        builder.HasOne(i => i.Device)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // One-to-One relationship with ApplicationUser (optional)
        builder.HasOne(u => u.ApplicationUser)
            .WithOne(a => a.User)
            .HasForeignKey<User>(u => u.ApplicationUserId)
            .OnDelete(DeleteBehavior.SetNull)
            .IsRequired(false);
    }
}