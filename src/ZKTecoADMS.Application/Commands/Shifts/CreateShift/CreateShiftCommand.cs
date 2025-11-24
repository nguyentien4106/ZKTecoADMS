using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public record CreateShiftCommand(
    Guid EmployeeUserId,
    DateTime StartTime,
    DateTime EndTime,
    string? Description,
    bool IsManager) : ICommand<AppResponse<ShiftDto>>;
