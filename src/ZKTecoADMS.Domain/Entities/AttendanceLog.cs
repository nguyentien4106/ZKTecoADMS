using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Entities.Base;

namespace ZKTecoADMS.Domain.Entities;

public class AttendanceLog : Entity<Guid>
{
    public Guid DeviceId { get; set; }
    public Guid? UserId { get; set; }

    [Required]
    [MaxLength(20)]
    public string PIN { get; set; } = string.Empty;

    public int? VerifyType { get; set; }
    public int VerifyState { get; set; } = 0;

    public DateTime AttendanceTime { get; set; }

    [MaxLength(10)]
    public string? WorkCode { get; set; }

    public decimal? Temperature { get; set; }
    public bool? MaskStatus { get; set; }

    public string? RawData { get; set; }

    public bool IsProcessed { get; set; } = false;
    public DateTime? ProcessedAt { get; set; }

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
    public virtual User? User { get; set; }
}

