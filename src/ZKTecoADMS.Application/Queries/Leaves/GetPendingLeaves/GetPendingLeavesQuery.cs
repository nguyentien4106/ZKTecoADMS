using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;

public record GetPendingLeavesQuery(Guid UserId, bool IsManager) : IQuery<AppResponse<List<LeaveDto>>>;
