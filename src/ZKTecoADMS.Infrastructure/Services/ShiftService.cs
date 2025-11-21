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
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);
    }

    public async Task<List<Shift>> GetShiftsByApplicationUserIdAsync(Guid applicationUserId, CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.ApplicationUserId == applicationUserId,
            orderBy: query => query.OrderByDescending(s => s.CreatedAt),
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<List<Shift>> GetShiftsByManagerAsync(Guid managerId, CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.ApplicationUser != null && s.ApplicationUser.ManagerId == managerId,
            orderBy: query => query.OrderByDescending(s => s.CreatedAt),
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<List<Shift>> GetPendingShiftsAsync(CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.Status == ShiftStatus.Pending,
            orderBy: query => query.OrderBy(s => s.StartTime),
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<List<Shift>> GetShiftsByStatusAsync(ShiftStatus status, CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.Status == status,
            orderBy: query => query.OrderByDescending(s => s.CreatedAt),
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<List<Shift>> GetShiftsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.StartTime >= startDate && s.EndTime <= endDate,
            orderBy: query => query.OrderBy(s => s.StartTime),
            includeProperties: new[] { nameof(Shift.ApplicationUser) },
            cancellationToken: cancellationToken);

        return shifts.ToList();
    }

    public async Task<Shift> CreateShiftAsync(Shift shift, CancellationToken cancellationToken = default)
    {
        ValidateShift(shift);

        // Validate dates
        if (shift.StartTime < DateTime.Now)
        {
            throw new InvalidOperationException("Cannot create shift for past dates");
        }

        shift.Status = ShiftStatus.Pending;
        var createdShift = await repository.AddAsync(shift, cancellationToken);
        
        logger.LogInformation(
            "Created shift for user {UserId} from {StartTime} to {EndTime}",
            shift.ApplicationUserId,
            shift.StartTime,
            shift.EndTime);

        return createdShift;
    }

    public async Task<Shift> UpdateShiftAsync(Shift shift, CancellationToken cancellationToken = default)
    {
        ValidateShift(shift);

        var existingShift = await GetShiftByIdAsync(shift.Id, cancellationToken);
        if (existingShift == null)
        {
            throw new InvalidOperationException($"Shift with ID {shift.Id} not found");
        }

        // Only allow updates to pending shifts
        if (existingShift.Status != ShiftStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot update shift with status {existingShift.Status}");
        }

        existingShift.StartTime = shift.StartTime;
        existingShift.EndTime = shift.EndTime;
        existingShift.Description = shift.Description;

        await repository.UpdateAsync(existingShift, cancellationToken);
        
        logger.LogInformation("Updated shift: {ShiftId}", shift.Id);

        return existingShift;
    }

    public async Task<bool> DeleteShiftAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var shift = await GetShiftByIdAsync(id, cancellationToken);
        if (shift == null)
        {
            logger.LogWarning("Attempted to delete non-existent shift: {ShiftId}", id);
            return false;
        }

        // Only allow deletion of pending shifts
        if (shift.Status != ShiftStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot delete shift with status {shift.Status}");
        }

        await repository.DeleteAsync(shift, cancellationToken);
        
        logger.LogInformation("Deleted shift: {ShiftId}", id);

        return true;
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
        shift.UpdatedAt = DateTime.UtcNow;
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

    private void ValidateShift(Shift shift)
    {
        if (shift.StartTime >= shift.EndTime)
        {
            throw new ArgumentException("Start time must be before end time");
        }
    }
}
