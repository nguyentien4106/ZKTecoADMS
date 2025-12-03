using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Leaves.GetPendingLeaves;

public class GetPendingLeavesHandler(
    IRepositoryPagedQuery<Leave> repository
    )
    : IQueryHandler<GetPendingLeavesQuery, AppResponse<PagedResult<LeaveDto>>>
{
    public async Task<AppResponse<PagedResult<LeaveDto>>> Handle(GetPendingLeavesQuery request, CancellationToken cancellationToken)
    {
        // var leaves = await repository.GetAllAsync(
        //     filter: l => l.Status == LeaveStatus.Pending && (request.IsManager ? l.ManagerId == request.UserId : l.EmployeeUserId == request.UserId),
        //     orderBy: query => query.OrderBy(l => l.CreatedAt),
        //     includeProperties: [nameof(Leave.EmployeeUser), nameof(Leave.Shift)],
        //     cancellationToken: cancellationToken);

        var pagedResult = await repository.GetPagedResultWithIncludesAsync(
            request.PaginationRequest,
            filter: l  => l.Status == LeaveStatus.Pending &&
                         (request.IsManager ? l.ManagerId == request.UserId : l.EmployeeUserId == request.UserId),
            includes: q => q.Include(l => l.EmployeeUser).Include(l => l.Shift),
            cancellationToken: cancellationToken
        );

        var response = new PagedResult<LeaveDto>(pagedResult.Items.Adapt<List<LeaveDto>>(),  pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
        return AppResponse<PagedResult<LeaveDto>>.Success(response);
    }
}
