using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKTecoADMS.Domain.Entities.Base;

namespace ZKTecoADMS.Domain.Entities;

// ZKTecoADMS.Domain/Entities/User.cs
public class User : Entity<Guid>
{
    [Required]
    [MaxLength(20)]
    public string PIN { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? CardNumber { get; set; }

    [MaxLength(50)]
    public string? Password { get; set; }

    public int GroupId { get; set; } = 1;
    public int Privilege { get; set; } = 0;
    public int VerifyMode { get; set; } = 0;

    public DateTime? StartDatetime { get; set; }
    public DateTime? EndDatetime { get; set; }

    public bool IsActive { get; set; } = true;

    [MaxLength(100)]
    public string? Email { get; set; }

    [MaxLength(20)]
    public string? PhoneNumber { get; set; }

    [MaxLength(100)]
    public string? Department { get; set; }

    [MaxLength(100)]
    public string? Position { get; set; }
    
    public Guid DeviceId { get; set; }
    public virtual Device Device { get; set; } = null!;

    // Navigation Properties
    public virtual ICollection<FingerprintTemplate> FingerprintTemplates { get; set; } = new List<FingerprintTemplate>();
    public virtual ICollection<FaceTemplate> FaceTemplates { get; set; } = new List<FaceTemplate>();
    public virtual ICollection<AttendanceLog> AttendanceLogs { get; set; } = new List<AttendanceLog>();
}

