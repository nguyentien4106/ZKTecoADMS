namespace ZKTecoADMS.Application.DTOs.Shifts;

public class CreateShiftRequest
{
    public Guid? EmployeeUserId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int MaximumAllowedLateMinutes { get; set; } = 30;
    public int MaximumAllowedEarlyLeaveMinutes { get; set; } = 30;
    public string? Description { get; set; }
}
