using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public class GetShiftsByManagerHandler(
    IRepositoryPagedQuery<Shift> repository
    ) : IQueryHandler<GetShiftsByManagerQuery, AppResponse<PagedResult<ShiftDto>>>
{
    public async Task<AppResponse<PagedResult<ShiftDto>>> Handle(GetShiftsByManagerQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await repository.GetPagedResultWithIncludesAsync(
            request.PaginationRequest,
            filter: s => s.EmployeeUser.ManagerId == request.ManagerId,
            includes: q => q.Include(s => s.EmployeeUser),
            cancellationToken);
        
        var response = new PagedResult<ShiftDto>(pagedResult.Items.Adapt<List<ShiftDto>>(), pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
        
        return AppResponse<PagedResult<ShiftDto>>.Success(response);
    }
}
