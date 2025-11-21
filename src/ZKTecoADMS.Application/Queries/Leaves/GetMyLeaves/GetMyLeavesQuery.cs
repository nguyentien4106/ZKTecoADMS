using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetMyLeaves;

public record GetMyLeavesQuery(Guid ApplicationUserId) : IQuery<AppResponse<List<LeaveDto>>>;
