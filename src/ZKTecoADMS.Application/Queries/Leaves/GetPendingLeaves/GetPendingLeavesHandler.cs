using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;

public class GetPendingLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetPendingLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetPendingLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            filter: l => l.Status == LeaveStatus.Pending,
            orderBy: query => query.OrderBy(l => l.StartDate),
            includeProperties: new[] { nameof(Leave.ApplicationUser), nameof(Leave.ApprovedByUser) },
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
