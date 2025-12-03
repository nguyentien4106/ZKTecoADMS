using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Leaves.GetAllLeaves;

public class GetAllLeavesHandler(
    IRepositoryPagedQuery<Leave> repository
    )
    : IQueryHandler<GetAllLeavesQuery, AppResponse<PagedResult<LeaveDto>>>
{
    public async Task<AppResponse<PagedResult<LeaveDto>>> Handle(GetAllLeavesQuery request, CancellationToken cancellationToken)
    {

        var pagedResult = await repository.GetPagedResultWithIncludesAsync(
            request: request.PaginationRequest,
            filter: request.IsManager
                ? l => l.ManagerId == request.UserId
                : l => l.EmployeeUserId == request.UserId,
            includes: q => q.Include(l => l.Shift).Include(s => s.EmployeeUser),
            cancellationToken: cancellationToken
        );
        
        return AppResponse<PagedResult<LeaveDto>>.Success(new PagedResult<LeaveDto>(pagedResult.Items.Adapt<List<LeaveDto>>(), pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize));
    }
}
