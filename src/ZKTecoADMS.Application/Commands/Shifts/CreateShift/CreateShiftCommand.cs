using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public record CreateShiftCommand(
    Guid ApplicationUserId,
    DateTime StartTime,
    DateTime EndTime,
    string? Description) : ICommand<AppResponse<ShiftDto>>;
