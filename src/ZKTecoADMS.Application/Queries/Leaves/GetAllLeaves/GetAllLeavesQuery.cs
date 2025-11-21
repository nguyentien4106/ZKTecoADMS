using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;

public record GetAllLeavesQuery() : IQuery<AppResponse<List<LeaveDto>>>;
