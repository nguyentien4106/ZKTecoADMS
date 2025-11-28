using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Infrastructure.Services;

public class ShiftService(
    IRepository<Shift> repository,
    UserManager<ApplicationUser> userManager,
    ILogger<ShiftService> logger) : IShiftService
{
    public async Task<Shift?> GetShiftByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await repository.GetSingleAsync(
            s => s.Id == id,
            includeProperties: [nameof(Shift.EmployeeUser)],
            cancellationToken: cancellationToken);
    }

    public async Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.EmployeeUser != null && s.EmployeeUser.ManagerId == managerId,
            orderBy: query => query.OrderByDescending(s => s.CreatedAt),
            includeProperties: new[] { nameof(Shift.EmployeeUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.Status == ShiftStatus.Pending,
            orderBy: query => query.OrderBy(s => s.StartTime),
            includeProperties: new[] { nameof(Shift.EmployeeUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<Shift> ApproveShiftAsync(Guid shiftId, Guid approvedByUserId, CancellationToken cancellationToken = default)
    {
        var shift = await GetShiftByIdAsync(shiftId, cancellationToken);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found");
        }

        if (shift.Status != ShiftStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot approve shift with status {shift.Status}");
        }

        // Verify the approver exists
        var approver = await userManager.FindByIdAsync(approvedByUserId.ToString());
        if (approver == null)
        {
            throw new InvalidOperationException($"Approver with ID {approvedByUserId} not found");
        }

        shift.Status = ShiftStatus.Approved;
        shift.UpdatedAt = DateTime.Now;
        shift.RejectionReason = null;

        await repository.UpdateAsync(shift, cancellationToken);
        
        logger.LogInformation(
            "Shift {ShiftId} approved by user {ApprovedByUserId}",
            shiftId,
            approvedByUserId);

        return shift;
    }

    public async Task<Shift> RejectShiftAsync(Guid shiftId, Guid rejectedByUserId, string rejectionReason, CancellationToken cancellationToken = default)
    {
        var shift = await GetShiftByIdAsync(shiftId, cancellationToken);
        if (shift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shiftId} not found");
        }

        if (shift.Status != ShiftStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot reject shift with status {shift.Status}");
        }

        if (string.IsNullOrWhiteSpace(rejectionReason))
        {
            throw new ArgumentException("Rejection reason is required", nameof(rejectionReason));
        }

        // Verify the rejector exists
        var rejector = await userManager.FindByIdAsync(rejectedByUserId.ToString());
        if (rejector == null)
        {
            throw new InvalidOperationException($"Rejector with ID {rejectedByUserId} not found");
        }

        shift.Status = ShiftStatus.Rejected;
        shift.UpdatedAt = DateTime.Now;
        shift.RejectionReason = rejectionReason;

        await repository.UpdateAsync(shift, cancellationToken);
        
        logger.LogInformation(
            "Shift {ShiftId} rejected by user {RejectedByUserId}. Reason: {RejectionReason}",
            shiftId,
            rejectedByUserId,
            rejectionReason);

        return shift;
    }

    public async Task<(Shift? CurrentShift, Shift? NextShift)> GetCurrentShiftAndNextShiftAsync(Guid employeeId, DateTime currentTime, CancellationToken cancellationToken = default)
    {
        var currentShift = await repository.GetSingleAsync(
            s => s.EmployeeUserId == employeeId &&
                 s.StartTime <= currentTime &&
                 s.EndTime >= currentTime &&
                 s.Status == ShiftStatus.Approved,
            cancellationToken: cancellationToken);

        var nextShift = await repository.GetSingleAsync(
            s => s.EmployeeUserId == employeeId &&
                 s.StartTime > currentTime &&
                 s.Status == ShiftStatus.Approved,
            cancellationToken: cancellationToken);

        return (currentShift, nextShift);
    }

    public async Task UpdateShiftAttendancesAsync(IEnumerable<Attendance> attendances, CancellationToken cancellationToken = default)
    {
        var attendanceDates = attendances.Select(a => a.AttendanceTime.Date).Distinct().ToList();
        foreach (var date in attendanceDates)
        {
            var shiftsByDate = await repository.GetAllAsync(
                filter: s => s.StartTime.Date <= date && s.EndTime.Date >= date,
                cancellationToken: cancellationToken);
            
            
        }
    }
}
