using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.UpdateShift;

public record UpdateShiftCommand(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    string? Description) : ICommand<AppResponse<ShiftDto>>;
