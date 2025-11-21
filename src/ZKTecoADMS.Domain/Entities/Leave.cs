using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class Leave : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeUserId { get; set; }

    [Required]
    public Guid ManagerId { get; set; }
    
    [Required]
    public LeaveType Type { get; set; }

    [Required]
    public Guid ShiftId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public bool IsHalfShift { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;
    
    [Required]
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    // Navigation Properties
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public virtual ApplicationUser Manager { get; set; } = null!;
    public virtual Shift Shift { get; set; } = null!;
}
