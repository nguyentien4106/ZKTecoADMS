using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.CreateLeave;

public record CreateLeaveCommand(
    Guid EmployeeUserId,
    Guid ManagerId,
    Guid ShiftId,
    DateTime StartDate,
    DateTime EndDate,
    LeaveType Type,
    bool IsHalfShift,
    string Reason) : ICommand<AppResponse<LeaveDto>>;
