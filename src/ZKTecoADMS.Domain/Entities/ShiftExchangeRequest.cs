using System.ComponentModel.DataAnnotations;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Entities;

public class ShiftExchangeRequest : AuditableEntity<Guid>
{
    [Required]
    public Guid ShiftId { get; set; }
    
    public virtual Shift Shift { get; set; } = null!;
    
    [Required]
    public Guid RequesterId { get; set; }
    
    public virtual Employee Requester { get; set; } = null!;
    
    [Required]
    public Guid TargetEmployeeId { get; set; }
    
    public virtual Employee TargetEmployee { get; set; } = null!;
    
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
    
    [Required]
    public ShiftExchangeStatus Status { get; set; } = ShiftExchangeStatus.Pending;
    
    [MaxLength(500)]
    public string? RejectionReason { get; set; }
    
    public DateTime? RespondedAt { get; set; }
}
