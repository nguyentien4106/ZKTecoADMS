using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;

public record GetShiftsByEmployeeQuery(PaginationRequest PaginationRequest, Guid EmployeeUserId, ShiftStatus? Status) : IQuery<AppResponse<PagedResult<ShiftDto>>>;
