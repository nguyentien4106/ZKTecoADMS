using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTeco.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;
// ZKTeco.Domain/Entities/DeviceCommand.cs
public class DeviceCommand : BaseEntity
{
    public Guid DeviceId { get; set; }

    public long CommandId { get; set; } = DateTime.UtcNow.Ticks;

    [Required]
    [MaxLength(50)]
    public string CommandType { get; set; } = string.Empty;

    public string? CommandData { get; set; }
    public int Priority { get; set; } = 1;

    [MaxLength(20)]
    public CommandStatus Status { get; set; } = CommandStatus.Created;

    public string? ResponseData { get; set; }

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime? SentAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
}