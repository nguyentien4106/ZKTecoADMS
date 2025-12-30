using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public record GetShiftsByManagerQuery(
    Guid ManagerId, 
    PaginationRequest PaginationRequest,
    GetManagedShiftRequest Filter) : IQuery<AppResponse<PagedResult<ShiftDto>>>;
