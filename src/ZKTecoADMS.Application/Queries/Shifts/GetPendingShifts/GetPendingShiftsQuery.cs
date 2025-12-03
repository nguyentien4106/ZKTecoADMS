using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;

public record GetPendingShiftsQuery(Guid? ManagerId, PaginationRequest PaginationRequest) : IQuery<AppResponse<PagedResult<ShiftDto>>>;
