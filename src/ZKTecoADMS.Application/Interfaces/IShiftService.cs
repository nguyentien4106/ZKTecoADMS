using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Interfaces;

public interface IShiftService
{
    Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Shift> ApproveShiftAsync(Guid shiftId, Guid approvedByUserId, CancellationToken cancellationToken = default);
    Task<Shift> RejectShiftAsync(Guid shiftId, Guid rejectedByUserId, string rejectionReason, CancellationToken cancellationToken = default);
    Task<(Shift? CurrentShift, Shift? NextShift)> GetTodayShiftAndNextShiftAsync(Guid employeeId, CancellationToken cancellationToken = default);   
    Task<Shift?> GetShiftByDateAsync(Guid? employeeId, DateTime date, CancellationToken cancellationToken = default);
}
