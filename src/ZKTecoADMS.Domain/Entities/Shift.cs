using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class Shift : AuditableEntity<Guid>
{
    [Required]
    public Guid EmployeeUserId { get; set; }
    
    [Required]
    public DateTime StartTime { get; set; }
    
    [Required]
    public DateTime EndTime { get; set; }
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public ShiftStatus Status { get; set; } = ShiftStatus.Pending;
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    // Navigation Properties
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;

    public virtual Employee Employee { get; set; } = null!;

    public virtual Leave? Leave { get; set; }
}
