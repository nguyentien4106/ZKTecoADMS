using System.ComponentModel.DataAnnotations;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Domain.Entities;

// ZKTeco.Domain/Entities/UserDeviceMapping.cs
public class UserDeviceMapping
{
    [Key]
    public int Id { get; set; }

    public int UserId { get; set; }
    public int DeviceId { get; set; }

    public bool IsSynced { get; set; } = false;
    public DateTime? LastSyncedAt { get; set; }

    [MaxLength(20)]
    public string SyncStatus { get; set; } = "Pending";

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Device Device { get; set; } = null!;
}