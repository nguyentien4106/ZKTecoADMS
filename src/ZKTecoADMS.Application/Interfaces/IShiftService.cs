using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IShiftService
{
    Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default);
    Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default);
    Task<Shift> ApproveShiftAsync(Guid shiftId, Guid approvedByUserId, CancellationToken cancellationToken = default);
    Task<Shift> RejectShiftAsync(Guid shiftId, Guid rejectedByUserId, string rejectionReason, CancellationToken cancellationToken = default);
    Task<(Shift? CurrentShift, Shift? NextShift)> GetCurrentShiftAndNextShiftAsync(Guid employeeId, DateTime currentTime, CancellationToken cancellationToken = default);   
    Task UpdateShiftAttendancesAsync(IEnumerable<Attendance> attendances, CancellationToken cancellationToken = default);
}
