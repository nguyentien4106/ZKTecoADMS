// ZKTeco.Domain/Entities/Device.cs
using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities;

namespace ZKTeco.Domain.Entities;

public class Device
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string SerialNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string DeviceName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? Model { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    public int? Port { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    [MaxLength(50)]
    public string Timezone { get; set; } = "UTC";

    public bool IsActive { get; set; } = true;

    public DateTime? LastOnline { get; set; }

    [MaxLength(50)]
    public string? FirmwareVersion { get; set; }

    [MaxLength(50)]
    public string? Platform { get; set; }

    [MaxLength(20)]
    public string DeviceStatus { get; set; } = "Offline";

    public int? MaxUsers { get; set; }
    public int? MaxFingerprints { get; set; }
    public int? MaxFaces { get; set; }

    public bool SupportsPushSDK { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
    public virtual ICollection<DeviceCommand> DeviceCommands { get; set; } = new List<DeviceCommand>();
    public virtual ICollection<SyncLog> SyncLogs { get; set; } = new List<SyncLog>();
    public virtual ICollection<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();
    public virtual ICollection<UserDeviceMapping> UserDeviceMappings { get; set; } = new List<UserDeviceMapping>();
}