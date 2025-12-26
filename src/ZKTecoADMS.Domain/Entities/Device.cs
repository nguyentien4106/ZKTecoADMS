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

    [MaxLength(500)]
    public string? Description { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    [MaxLength(200)]
    public string? Location { get; set; }

    public DateTime? LastOnline { get; set; }

    [MaxLength(20)]
    public string DeviceStatus { get; set; } = "Offline";

    public Guid ManagerId { get; set; }
    public Guid DeviceInfoId { get; set; }

    // Navigation Properties

    public virtual ApplicationUser Manager { get; set; } = null!;
    public virtual DeviceInfo DeviceInfo { get; set; } = null!;
    public virtual ICollection<DeviceUser> Employees { get; set; } = new List<DeviceUser>();
    public virtual ICollection<Attendance> AttendanceLogs { get; set; } = new List<Attendance>();
    public virtual ICollection<DeviceCommand> DeviceCommands { get; set; } = new List<DeviceCommand>();
    public virtual ICollection<SyncLog> SyncLogs { get; set; } = new List<SyncLog>();
    public virtual ICollection<DeviceSetting> DeviceSettings { get; set; } = new List<DeviceSetting>();
}