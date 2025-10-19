using System.ComponentModel.DataAnnotations;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Domain.Entities;

// ZKTeco.Domain/Entities/UserDeviceMapping.cs
public class UserDeviceMapping : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid DeviceId { get; set; }
    public bool IsSynced { get; set; } = false;
    public DateTime? LastSyncedAt { get; set; }

    [MaxLength(20)]
    public string SyncStatus { get; set; } = "Pending";

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    // Navigation Properties
    public virtual User User { get; set; } = null!;
    public virtual Device Device { get; set; } = null!;
}