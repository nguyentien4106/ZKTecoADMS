using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class Leave : AuditableEntity<Guid>
{
    [Required]
    public Guid ApplicationUserId { get; set; }
    
    [Required]
    public LeaveType Type { get; set; }
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }
    
    [Required]
    public bool IsFullDay { get; set; }
    
    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;
    
    [Required]
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    public Guid? ApprovedByUserId { get; set; }
    
    public DateTime? ApprovedAt { get; set; }
    
    // Navigation Properties
    public virtual ApplicationUser ApplicationUser { get; set; } = null!;
    public virtual ApplicationUser? ApprovedByUser { get; set; }
}
