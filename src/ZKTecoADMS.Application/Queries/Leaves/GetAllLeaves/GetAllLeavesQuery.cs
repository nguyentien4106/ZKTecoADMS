using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;

public record GetAllLeavesQuery(Guid UserId, bool IsManager) : IQuery<AppResponse<List<LeaveDto>>>;
