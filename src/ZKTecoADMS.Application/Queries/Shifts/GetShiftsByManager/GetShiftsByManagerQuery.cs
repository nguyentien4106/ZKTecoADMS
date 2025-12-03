using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public record GetShiftsByManagerQuery(Guid ManagerId, PaginationRequest PaginationRequest) : IQuery<AppResponse<PagedResult<ShiftDto>>>;
