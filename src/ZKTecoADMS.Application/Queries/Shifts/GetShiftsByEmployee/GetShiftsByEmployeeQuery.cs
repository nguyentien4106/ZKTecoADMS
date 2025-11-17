using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;

public record GetShiftsByEmployeeQuery(Guid ApplicationUserId) : IQuery<AppResponse<List<ShiftDto>>>;
