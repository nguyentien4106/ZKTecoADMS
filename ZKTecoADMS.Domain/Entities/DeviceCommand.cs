using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Domain.Entities;
// ZKTeco.Domain/Entities/DeviceCommand.cs
public class DeviceCommand
{
    [Key]
    public int Id { get; set; }

    public int DeviceId { get; set; }

    [Required]
    [MaxLength(50)]
    public string CommandType { get; set; } = string.Empty;

    public string? CommandData { get; set; }
    public int Priority { get; set; } = 1;

    [MaxLength(20)]
    public string Status { get; set; } = "Pending";

    public string? ResponseData { get; set; }

    [MaxLength(500)]
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? SentAt { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
}