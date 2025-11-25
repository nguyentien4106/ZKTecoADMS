using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;

public class GetPendingLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetPendingLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetPendingLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            filter: l => l.Status == LeaveStatus.Pending && (request.IsManager ? l.ManagerId == request.UserId : l.EmployeeUserId == request.UserId),
            orderBy: query => query.OrderBy(l => l.CreatedAt),
            includeProperties: [nameof(Leave.EmployeeUser), nameof(Leave.Shift)],
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
