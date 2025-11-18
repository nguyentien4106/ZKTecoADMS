namespace ZKTecoADMS.Application.DTOs.Shifts;

public class CreateShiftTemplateRequest
{
    public string Name { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}
