using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.CreateShiftTemplate;

public record CreateShiftTemplateCommand(
    Guid ManagerId,
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int MaximumAllowedLateMinutes = 30,
    int MaximumAllowedEarlyLeaveMinutes = 30,
    int BreakTimeMinutes = 60) : ICommand<AppResponse<ShiftTemplateDto>>;
