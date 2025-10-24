// ZKTecoADMS.Domain/Entities/Device.cs
using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Entities.Base;

namespace ZKTecoADMS.Domain.Entities;

public class Device : AuditableEntity<Guid>
{
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

    public Guid ApplicationUserId { get; set; }

    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    public Guid DeviceInfoId { get; set; }

    public virtual DeviceInfo DeviceInfo { get; set; } = null!;

    // Navigation Properties
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public virtual ICollection<Attendance> AttendanceLogs { get; set; } = new List<Attendance>();
    public virtual ICollection<DeviceCommand> DeviceCommands { get; set; } = new List<DeviceCommand>();
    public virtual ICollection<SyncLog> SyncLogs { get; set; } = new List<SyncLog>();
    public virtual ICollection<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();
}