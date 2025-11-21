using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;

public record GetShiftsByEmployeeQuery(Guid ApplicationUserId, ShiftStatus? Status) : IQuery<AppResponse<List<ShiftDto>>>;
