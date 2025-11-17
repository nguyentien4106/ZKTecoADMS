using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;

public record GetPendingShiftsQuery() : IQuery<AppResponse<List<ShiftDto>>>;
