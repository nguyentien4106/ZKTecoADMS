namespace ZKTecoADMS.Application.DTOs.Shifts;

public class ShiftExchangeRequestDto
{
    public Guid Id { get; set; }
    
    public Guid ShiftId { get; set; }
    
    public ShiftDto? Shift { get; set; }
    
    public Guid RequesterId { get; set; }
    
    public string RequesterName { get; set; } = string.Empty;
    
    public Guid TargetEmployeeId { get; set; }
    
    public string TargetEmployeeName { get; set; } = string.Empty;
    
    public string Reason { get; set; } = string.Empty;
    
    public string Status { get; set; } = string.Empty;
    
    public string? RejectionReason { get; set; }
    
    public DateTime? RespondedAt { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
