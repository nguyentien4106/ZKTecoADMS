using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Entities.Base;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Domain.Repositories;

public interface IShiftRepository : IRepository<Shift>
{
    Task<List<Shift>> GetShiftsByApplicationUserIdAsync(Guid applicationUserId, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByStatusAsync(ShiftStatus status, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
}
