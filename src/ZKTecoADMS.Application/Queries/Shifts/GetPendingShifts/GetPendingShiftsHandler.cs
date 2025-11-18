using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;

public class GetPendingShiftsHandler(IShiftService shiftService) 
    : IQueryHandler<GetPendingShiftsQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetPendingShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftService.GetPendingShiftsAsync(cancellationToken);
        
        return AppResponse<List<ShiftDto>>.Success(shifts.Adapt<List<ShiftDto>>());
    }
}
