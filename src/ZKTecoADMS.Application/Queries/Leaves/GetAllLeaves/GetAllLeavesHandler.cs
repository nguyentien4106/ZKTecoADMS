using ZKTecoADMS.Application.DTOs.Leaves;

namespace ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;

public class GetAllLeavesHandler(IRepository<Leave> repository)
    : IQueryHandler<GetAllLeavesQuery, AppResponse<List<LeaveDto>>>
{
    public async Task<AppResponse<List<LeaveDto>>> Handle(GetAllLeavesQuery request, CancellationToken cancellationToken)
    {
        var leaves = await repository.GetAllAsync(
            orderBy: query => query.OrderByDescending(l => l.CreatedAt),
            includeProperties: new[] { nameof(Leave.ApplicationUser), nameof(Leave.ApprovedByUser) },
            cancellationToken: cancellationToken);

        return AppResponse<List<LeaveDto>>.Success(leaves.Adapt<List<LeaveDto>>());
    }
}
