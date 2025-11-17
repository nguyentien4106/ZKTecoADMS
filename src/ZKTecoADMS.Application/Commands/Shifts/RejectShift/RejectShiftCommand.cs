using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Commands.Shifts.RejectShift;

public record RejectShiftCommand(
    Guid Id,
    Guid RejectedByUserId,
    string RejectionReason) : ICommand<AppResponse<ShiftDto>>;
