using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.UpdateShiftTemplate;

public record UpdateShiftTemplateCommand(
    Guid Id,
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime,
    int MaximumAllowedLateMinutes,
    int MaximumAllowedEarlyLeaveMinutes,
    int BreakTimeMinutes,
    bool IsActive) : ICommand<AppResponse<ShiftTemplateDto>>;
