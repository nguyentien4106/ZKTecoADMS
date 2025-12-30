namespace ZKTecoADMS.Application.DTOs.Shifts;

public class CreateShiftRequest
{
    public List<WorkingDay> WorkingDays { get; set; } = [];
    public int BreakTimeMinutes { get; set; } = 60;
    public string? Description { get; set; }
}
