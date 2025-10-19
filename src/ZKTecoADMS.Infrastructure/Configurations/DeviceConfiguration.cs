using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class DeviceConfiguration : IEntityTypeConfiguration<Device>
{
    public void Configure(EntityTypeBuilder<Device> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.SerialNumber).IsUnique();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
        builder.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        
    }
}