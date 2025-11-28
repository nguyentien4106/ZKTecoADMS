using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public record CreateShiftCommand(
    Guid EmployeeUserId,
    DateTime StartTime,
    DateTime EndTime,
    int MaximumAllowedLateMinutes = 30,
    int MaximumAllowedEarlyLeaveMinutes = 30,
    string? Description = null,
    bool IsManager = false) : ICommand<AppResponse<ShiftDto>>;
