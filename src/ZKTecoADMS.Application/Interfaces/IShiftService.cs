using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IShiftService
{
    Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByApplicationUserIdAsync(Guid applicationUserId, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByStatusAsync(ShiftStatus status, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    Task<Shift> CreateShiftAsync(Shift shift, CancellationToken cancellationToken = default);
    Task<Shift> UpdateShiftAsync(Shift shift, CancellationToken cancellationToken = default);
    Task<bool> DeleteShiftAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Shift> ApproveShiftAsync(Guid shiftId, Guid approvedByUserId, CancellationToken cancellationToken = default);
    Task<Shift> RejectShiftAsync(Guid shiftId, Guid rejectedByUserId, string rejectionReason, CancellationToken cancellationToken = default);
}
