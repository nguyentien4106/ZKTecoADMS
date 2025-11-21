using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetMyLeaves;

public class GetMyLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetMyLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetMyLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            filter: l => l.ApplicationUserId == request.ApplicationUserId,
            orderBy: query => query.OrderByDescending(l => l.CreatedAt),
            includeProperties: new[] { nameof(Leave.ApplicationUser), nameof(Leave.ApprovedByUser) },
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
