using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class AttendanceLogConfiguration : IEntityTypeConfiguration<AttendanceLog>
{
    public void Configure(EntityTypeBuilder<AttendanceLog> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.DeviceId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.AttendanceTime);
        builder.HasIndex(e => e.PIN);

        builder.HasOne(e => e.Device)
            .WithMany(d => d.AttendanceLogs)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.User)
            .WithMany(u => u.AttendanceLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Add precision for Temperature
        builder.Property(e => e.Temperature)
            .HasPrecision(4, 1); // 4 total digits, 1 decimal place (e.g., 36.5)
    }
}