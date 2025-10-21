using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class Attendance : Entity<Guid>
{
    public Guid DeviceId { get; set; }
    public Guid? UserId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string PIN { get; set; } = string.Empty;
    
    public int VerifyMode { get; set; } = 0;
    
    public AttendanceStates AttendanceState { get; set; }
    public DateTime AttendanceTime { get; set; }
    
    [MaxLength(10)]
    public string? WorkCode { get; set; }

    // Navigation Properties
    public virtual Device Device { get; set; } = null!;
    public virtual User? User { get; set; }
}

