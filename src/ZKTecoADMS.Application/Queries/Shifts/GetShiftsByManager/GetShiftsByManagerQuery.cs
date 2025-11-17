using ZKTecoADMS.Application.DTOs.Shifts;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public record GetShiftsByManagerQuery(Guid ManagerId) : IQuery<AppResponse<List<ShiftDto>>>;
