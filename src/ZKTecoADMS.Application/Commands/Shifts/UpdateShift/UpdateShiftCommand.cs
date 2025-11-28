using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.UpdateShift;

public record UpdateShiftCommand(
    Guid Id,
    DateTime StartTime,
    DateTime EndTime,
    int MaximumAllowedLateMinutes,
    int MaximumAllowedEarlyLeaveMinutes,
    string? Description) : ICommand<AppResponse<ShiftDto>>;
