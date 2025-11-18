namespace ZKTecoADMS.Application.DTOs.Shifts;

public class UpdateShiftTemplateRequest
{
    public string Name { get; set; } = null!;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsActive { get; set; }
}
