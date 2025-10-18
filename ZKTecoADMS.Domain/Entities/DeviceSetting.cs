using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTeco.Domain.Entities;

namespace ZKTecoADMS.Domain.Entities
;
public class DeviceSetting
{
    [Key]
    public int Id { get; set; }

    public int DeviceId { get; set; }

    [Required]
    [MaxLength(100)]
    public string SettingKey { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? SettingValue { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
}