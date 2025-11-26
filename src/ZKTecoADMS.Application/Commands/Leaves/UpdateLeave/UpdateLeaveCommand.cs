using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.UpdateLeave;

public record UpdateLeaveCommand(
    Guid LeaveId,
    Guid CurrentUserId,
    bool IsManager,
    Guid ShiftId,
    DateTime StartDate,
    DateTime EndDate,
    LeaveType Type,
    bool IsHalfShift,
    string Reason,
    LeaveStatus? Status) : ICommand<AppResponse<LeaveDto>>;
