using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.ShiftTemplates.CreateShiftTemplate;

public record CreateShiftTemplateCommand(
    Guid ManagerId,
    string Name,
    TimeSpan StartTime,
    TimeSpan EndTime) : ICommand<AppResponse<ShiftTemplateDto>>;
