using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.UpdateShiftTemplate;

public record UpdateShiftTemplateCommand(
    Guid Id,
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime,
    bool IsActive) : ICommand<AppResponse<ShiftTemplateDto>>;
