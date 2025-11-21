using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.Shifts;

public class ShiftDto
{
    public Guid Id { get; set; }
    public Guid ApplicationUserId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
    public ShiftStatus Status { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int TotalHours => EndTime.Subtract(StartTime).Hours;
}
