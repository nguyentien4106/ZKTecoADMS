using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.CreateLeave;

public record CreateLeaveCommand(
    Guid ApplicationUserId,
    LeaveType Type,
    DateTime StartDate,
    DateTime EndDate,
    bool IsFullDay,
    string Reason) : ICommand<AppResponse<LeaveDto>>;
