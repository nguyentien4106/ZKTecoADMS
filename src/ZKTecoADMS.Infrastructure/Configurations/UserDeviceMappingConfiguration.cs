using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class UserDeviceMappingConfiguration : IEntityTypeConfiguration<UserDeviceMapping>
{
    public void Configure(EntityTypeBuilder<UserDeviceMapping> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => new { e.UserId, e.DeviceId }).IsUnique();

        builder.HasOne(e => e.User)
            .WithMany(u => u.UserDeviceMappings)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Device)
            .WithMany(d => d.UserDeviceMappings)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}