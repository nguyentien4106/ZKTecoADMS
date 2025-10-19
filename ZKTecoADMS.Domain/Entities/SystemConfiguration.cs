using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZKTecoADMS.Domain.Entities;
// ZKTeco.Domain/Entities/SystemConfiguration.cs
public class SystemConfiguration : BaseEntity
{
    [Required]
    [MaxLength(100)]
    public string ConfigKey { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? ConfigValue { get; set; }

    [MaxLength(200)]
    public string? Description { get; set; }

}