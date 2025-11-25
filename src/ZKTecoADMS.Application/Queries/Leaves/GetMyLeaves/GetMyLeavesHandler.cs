using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetMyLeaves;

public class GetMyLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetMyLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetMyLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            filter: l => l.EmployeeUserId == request.ApplicationUserId,
            orderBy: query => query.OrderByDescending(l => l.CreatedAt),
            includeProperties: [nameof(Leave.EmployeeUser), nameof(Leave.Shift)],
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
