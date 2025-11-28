using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.UpdateShift;

public class UpdateShiftHandler(IRepository<Shift> repository) 
    : ICommandHandler<UpdateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate time range
            if (request.StartTime >= request.EndTime)
            {
                return AppResponse<ShiftDto>.Error("Start time must be before end time");
            }

            var shift = await repository.GetSingleAsync(
                s => s.Id == request.Id,
                includeProperties: [nameof(Shift.EmployeeUser)],
                cancellationToken: cancellationToken);
                
            if (shift == null)
            {
                return AppResponse<ShiftDto>.Error("Shift not found");
            }

            // Only allow updates to pending shifts
            if (shift.Status != ShiftStatus.Pending)
            {
                return AppResponse<ShiftDto>.Error($"Cannot update shift with status {shift.Status}");
            }

            // Get all other shifts for the employee on the same date(s), excluding current shift
            var requestStartDate = request.StartTime.Date;
            var requestEndDate = request.EndTime.Date;
            
            var existingShiftsOnDate = await repository.GetAllAsync(
                s => s.EmployeeUserId == shift.EmployeeUserId &&
                     s.Id != request.Id && // Exclude current shift being updated
                     (s.Status == ShiftStatus.Approved || s.Status == ShiftStatus.Pending) &&
                     (s.StartTime.Date == requestStartDate || s.EndTime.Date == requestStartDate ||
                      s.StartTime.Date == requestEndDate || s.EndTime.Date == requestEndDate),
                cancellationToken: cancellationToken
            );

            var existingShiftsList = existingShiftsOnDate.ToList();

            // Check for overlapping shifts
            var overlappingShift = existingShiftsList.FirstOrDefault(s =>
                // New shift starts during an existing shift
                (request.StartTime >= s.StartTime && request.StartTime < s.EndTime) ||
                // New shift ends during an existing shift
                (request.EndTime > s.StartTime && request.EndTime <= s.EndTime) ||
                // New shift completely encompasses an existing shift
                (request.StartTime <= s.StartTime && request.EndTime >= s.EndTime)
            );

            if (overlappingShift != null)
            {
                return AppResponse<ShiftDto>.Error(
                    $"This shift overlaps with an existing shift from {overlappingShift.StartTime:g} to {overlappingShift.EndTime:g}");
            }

            // Check maximum 2 shifts per day
            var shiftsOnSameDay = existingShiftsList
                .Where(s => s.StartTime.Date == requestStartDate || s.EndTime.Date == requestStartDate)
                .ToList();

            if (shiftsOnSameDay.Count >= 2)
            {
                return AppResponse<ShiftDto>.Error(
                    $"Employee already has 2 shifts on {requestStartDate:yyyy-MM-dd}. Maximum 2 shifts per day allowed.");
            }

            // Check minimum 1 hour gap between shifts
            foreach (var existingShift in existingShiftsList)
            {
                // Calculate time gap
                TimeSpan gap;
                
                if (request.StartTime >= existingShift.EndTime)
                {
                    // New shift is after existing shift
                    gap = request.StartTime - existingShift.EndTime;
                }
                else if (request.EndTime <= existingShift.StartTime)
                {
                    // New shift is before existing shift
                    gap = existingShift.StartTime - request.EndTime;
                }
                else
                {
                    // Shifts overlap (already checked above, but just in case)
                    continue;
                }

                if (gap.TotalHours < 1)
                {
                    return AppResponse<ShiftDto>.Error(
                        $"There must be at least 1 hour gap between shifts. Current gap with shift ({existingShift.StartTime:g} - {existingShift.EndTime:g}) is only {gap.TotalMinutes:F0} minutes.");
                }
            }

            shift.StartTime = request.StartTime;
            shift.EndTime = request.EndTime;
            shift.MaximumAllowedLateMinutes = request.MaximumAllowedLateMinutes;
            shift.MaximumAllowedEarlyLeaveMinutes = request.MaximumAllowedEarlyLeaveMinutes;
            shift.Description = request.Description;

            await repository.UpdateAsync(shift, cancellationToken);
            var shiftDto = new ShiftDto
            {
                Id = shift.Id,
                EmployeeUserId = shift.EmployeeUserId,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                MaximumAllowedLateMinutes = shift.MaximumAllowedLateMinutes,
                MaximumAllowedEarlyLeaveMinutes = shift.MaximumAllowedEarlyLeaveMinutes,
                Description = shift.Description,
                Status = shift.Status,
                CreatedAt = shift.CreatedAt,
                UpdatedAt = shift.UpdatedAt
            };
            return AppResponse<ShiftDto>.Success(shiftDto);
        }
        catch (ArgumentException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
    }
}
