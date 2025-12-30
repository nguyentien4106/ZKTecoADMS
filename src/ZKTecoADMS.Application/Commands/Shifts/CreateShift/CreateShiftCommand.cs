using FluentValidation;
using ZKTecoADMS.Application.DTOs;
using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public class CreateShiftCommand : ICommand<AppResponse<ShiftDto>>
{
    public Guid EmployeeId { get; set; }
    public List<WorkingDay> WorkingDays { get; set; } = [];

    public int BreakTimeMinutes { get; set; } = 60;
    
    public string? Description { get; set; }
}
