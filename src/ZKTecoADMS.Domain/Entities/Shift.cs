using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class Shift : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeId { get; set; }
    
    public virtual Employee Employee { get; set; } = null!;
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public int BreakTimeMinutes { get; set; } = 60;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public ShiftStatus Status { get; set; } = ShiftStatus.Pending;
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }

    public Guid? CheckInAttendanceId { get; set; }

    public Guid? CheckOutAttendanceId { get; set; }

    public virtual Attendance? CheckInAttendance { get; set; }

    public virtual Attendance? CheckOutAttendance { get; set; }
    
    // Navigation Properties

    public virtual Leave? Leave { get; set; }
}
