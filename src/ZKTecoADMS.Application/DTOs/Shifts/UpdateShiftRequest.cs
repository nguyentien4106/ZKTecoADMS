using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.Shifts;

public class UpdateShiftRequest
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Description { get; set; }
    
    public LeaveStatus? Status { get; set; }
}
