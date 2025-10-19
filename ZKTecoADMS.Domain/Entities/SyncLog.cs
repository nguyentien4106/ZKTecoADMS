using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Domain.Entities;
// ZKTeco.Domain/Entities/SyncLog.cs
public class SyncLog : BaseEntity
{
    public Guid DeviceId { get; set; }

    [Required]
    [MaxLength(50)]
    public string SyncType { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    public string Direction { get; set; } = string.Empty;

    public string? RequestData { get; set; }
    public string? ResponseData { get; set; }

    public bool IsSuccess { get; set; } = true;

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public int? DurationMs { get; set; }

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
}