using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Configurations;

public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.DeviceId);
        builder.HasIndex(e => e.UserId);
        builder.HasIndex(e => e.PIN);

        builder.HasOne(e => e.Device)
            .WithMany(d => d.AttendanceLogs)
            .HasForeignKey(e => e.DeviceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.User)
            .WithMany(u => u.AttendanceLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);

    }
}