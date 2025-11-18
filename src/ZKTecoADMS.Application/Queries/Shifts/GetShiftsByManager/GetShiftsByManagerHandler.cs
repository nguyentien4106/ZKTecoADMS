using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public class GetShiftsByManagerHandler(IShiftService shiftService) 
    : IQueryHandler<GetShiftsByManagerQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetShiftsByManagerQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftService.GetShiftsByManagerAsync(request.ManagerId, cancellationToken);
        
        return AppResponse<List<ShiftDto>>.Success(shifts.Adapt<List<ShiftDto>>());
    }
}
