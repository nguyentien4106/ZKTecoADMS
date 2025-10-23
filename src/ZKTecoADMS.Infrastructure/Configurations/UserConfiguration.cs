using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.PIN).IsUnique();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("NOW()");
        
        builder.HasOne(i => i.Device)
            .WithMany(i => i.Users)
            .HasForeignKey(i => i.DeviceId)
            .OnDelete(DeleteBehavior.Restrict); // Change to Restrict to prevent deletion of Device when Users exist
        
    }
}