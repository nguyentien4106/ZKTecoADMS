using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.DTOs.Shifts;

public class ShiftDto
{
    public Guid Id { get; set; }
    public string EmployeeName { get; set; } = null!;
    public string EmployeeCode { get; set; } = null!;
    public Guid EmployeeId { get; set; }
    public DateTime Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int BreakTimeMinutes { get; set; } = 60;
    public string? Description { get; set; }
    public ShiftStatus Status { get; set; }
    public string? RejectionReason { get; set; }
    public double TotalHours
    {
        get
        {
            var totalMinutes = (EndTime - StartTime).TotalMinutes;
            return Math.Round(totalMinutes / 60, 2);
        }
    }
}
