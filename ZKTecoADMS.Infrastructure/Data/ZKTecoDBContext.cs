using Microsoft.EntityFrameworkCore;
using ZKTeco.Domain.Entities;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Infrastructure.Data;

public class ZKTecoDbContext : DbContext
{
    public ZKTecoDbContext(DbContextOptions<ZKTecoDbContext> options) : base(options)
    {
    }

    public DbSet<Device> Devices { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserDeviceMapping> UserDeviceMappings { get; set; }
    public DbSet<FingerprintTemplate> FingerprintTemplates { get; set; }
    public DbSet<FaceTemplate> FaceTemplates { get; set; }
    public DbSet<AttendanceLog> AttendanceLogs { get; set; }
    public DbSet<DeviceCommand> DeviceCommands { get; set; }
    public DbSet<SyncLog> SyncLogs { get; set; }
    public DbSet<DeviceSetting> DeviceSettings { get; set; }
    public DbSet<SystemConfiguration> SystemConfigurations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Device Configuration
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SerialNumber).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.PIN).IsUnique();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETDATE()");
        });

        // UserDeviceMapping Configuration
        modelBuilder.Entity<UserDeviceMapping>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.DeviceId }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(u => u.UserDeviceMappings)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.UserDeviceMappings)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FingerprintTemplate Configuration
        modelBuilder.Entity<FingerprintTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.FingerIndex }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(u => u.FingerprintTemplates)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // FaceTemplate Configuration
        modelBuilder.Entity<FaceTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.FaceIndex }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(u => u.FaceTemplates)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AttendanceLog Configuration
        modelBuilder.Entity<AttendanceLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DeviceId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.AttendanceTime);
            entity.HasIndex(e => e.PIN);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.AttendanceLogs)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany(u => u.AttendanceLogs)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // DeviceCommand Configuration
        modelBuilder.Entity<DeviceCommand>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.DeviceId);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.DeviceCommands)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SyncLog Configuration
        modelBuilder.Entity<SyncLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.DeviceId);
            entity.HasIndex(e => e.CreatedAt);

            entity.HasOne(e => e.Device)
                .WithMany(d => d.SyncLogs)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // DeviceSetting Configuration
        modelBuilder.Entity<DeviceSetting>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.DeviceId, e.SettingKey }).IsUnique();

            entity.HasOne(e => e.Device)
                .WithMany(d => d.DeviceSettings)
                .HasForeignKey(e => e.DeviceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SystemConfiguration Configuration
        modelBuilder.Entity<SystemConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ConfigKey).IsUnique();
        });
    }
}
