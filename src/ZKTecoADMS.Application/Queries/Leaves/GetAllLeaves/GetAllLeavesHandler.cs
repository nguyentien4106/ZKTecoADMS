using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;

public class GetAllLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetAllLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetAllLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            filter: request.IsManager
                ? l => l.ManagerId == request.UserId
                : l => l.EmployeeUserId == request.UserId,
            orderBy: query => query.OrderByDescending(l => l.CreatedAt),
            includeProperties: [nameof(Leave.EmployeeUser), nameof(Leave.Shift)],
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
