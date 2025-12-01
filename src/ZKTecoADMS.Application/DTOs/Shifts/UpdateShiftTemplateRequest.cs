namespace ZKTecoADMS.Application.DTOs.Shifts;

public class UpdateShiftTemplateRequest
{
    public string Name { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int MaximumAllowedLateMinutes { get; set; } = 30;
    public int MaximumAllowedEarlyLeaveMinutes { get; set; } = 30;
    public bool IsActive { get; set; }
}
