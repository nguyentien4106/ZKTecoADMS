using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Commands.Leaves.CancelLeave;

public record CancelLeaveCommand(Guid LeaveId, Guid ApplicationUserId) : ICommand<AppResponse<bool>>;
