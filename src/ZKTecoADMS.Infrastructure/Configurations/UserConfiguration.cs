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
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        
        builder.HasAlternateKey(e => new { e.DeviceId, e.PIN });
    }
}