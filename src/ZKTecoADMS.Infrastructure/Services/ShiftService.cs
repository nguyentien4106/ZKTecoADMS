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
        // Group attendances by employee and date for efficient processing
        var attendancesByEmployee = attendances
            .Where(a => a.Employee?.ApplicationUserId != null)
            .GroupBy(a => new { EmployeeUserId = a.Employee!.ApplicationUserId!.Value, Date = a.AttendanceTime.Date })
            .ToList();

        foreach (var employeeGroup in attendancesByEmployee)
        {
            var employeeUserId = employeeGroup.Key.EmployeeUserId;
            var date = employeeGroup.Key.Date;

            // Get all approved shifts for this employee on this date
            var shifts = await repository.GetAllAsync(
                filter: s => s.EmployeeUserId == employeeUserId &&
                           s.Status == ShiftStatus.Approved &&
                           s.StartTime.Date <= date &&
                           s.EndTime.Date >= date,
                orderBy: query => query.OrderBy(s => s.StartTime),
                cancellationToken: cancellationToken);

            if (!shifts.Any())
            {
                logger.LogWarning(
                    "No approved shifts found for employee {EmployeeUserId} on {Date}",
                    employeeUserId,
                    date);
                continue;
            }

            // Sort attendances by time for this employee/date
            var sortedAttendances = employeeGroup.OrderBy(a => a.AttendanceTime).ToList();

            // Map attendances to shifts
            await MapAttendancesToShifts(shifts.ToList(), sortedAttendances, cancellationToken);
        }
    }

    private async Task MapAttendancesToShifts(
        List<Shift> shifts,
        List<Attendance> attendances,
        CancellationToken cancellationToken)
    {
        foreach (var shift in shifts)
        {
            // Define a time window for check-in and check-out using shift configuration
            // Allow check-in up to MaximumAllowedLateMinutes before and after shift start
            var checkInWindowStart = shift.StartTime.AddMinutes(-shift.MaximumAllowedLateMinutes);
            var checkInWindowEnd = shift.StartTime.AddMinutes(shift.MaximumAllowedLateMinutes);
            
            // Allow check-out from MaximumAllowedEarlyLeaveMinutes before shift end and up to 1 hour after
            var checkOutWindowStart = shift.EndTime.AddMinutes(-shift.MaximumAllowedEarlyLeaveMinutes);
            var checkOutWindowEnd = shift.EndTime.AddHours(1); // 1 hour after shift end

            // Find check-in: First attendance within check-in window
            var checkInAttendance = attendances
                .Where(a => a.AttendanceTime >= checkInWindowStart && a.AttendanceTime <= checkInWindowEnd)
                .OrderBy(a => a.AttendanceTime)
                .FirstOrDefault();

            // Find check-out: Last attendance within check-out window
            var checkOutAttendance = attendances
                .Where(a => a.AttendanceTime >= checkOutWindowStart && a.AttendanceTime <= checkOutWindowEnd)
                .OrderByDescending(a => a.AttendanceTime)
                .FirstOrDefault();

            bool shiftUpdated = false;

            // Update check-in if found and not already set
            if (checkInAttendance != null && shift.CheckInAttendanceId != checkInAttendance.Id)
            {
                shift.CheckInAttendanceId = checkInAttendance.Id;
                shiftUpdated = true;
                
                logger.LogInformation(
                    "Mapped check-in attendance {AttendanceId} at {AttendanceTime} to shift {ShiftId} (start: {ShiftStart})",
                    checkInAttendance.Id,
                    checkInAttendance.AttendanceTime,
                    shift.Id,
                    shift.StartTime);
            }

            // Update check-out if found and not already set
            // Ensure check-out is different from check-in
            if (checkOutAttendance != null && 
                shift.CheckOutAttendanceId != checkOutAttendance.Id &&
                checkOutAttendance.Id != checkInAttendance?.Id)
            {
                shift.CheckOutAttendanceId = checkOutAttendance.Id;
                shiftUpdated = true;
                
                logger.LogInformation(
                    "Mapped check-out attendance {AttendanceId} at {AttendanceTime} to shift {ShiftId} (end: {ShiftEnd})",
                    checkOutAttendance.Id,
                    checkOutAttendance.AttendanceTime,
                    shift.Id,
                    shift.EndTime);
            }

            // Save shift if updated
            if (shiftUpdated)
            {
                await repository.UpdateAsync(shift, cancellationToken);
            }
        }
    }
}
