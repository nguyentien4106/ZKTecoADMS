using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public class CreateShiftHandler(IRepository<Shift> shiftRepository) 
    : ICommandHandler<CreateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate time range
            if (request.StartTime >= request.EndTime)
            {
                return AppResponse<ShiftDto>.Error("Start time must be before end time");
            }

            // Get all shifts for the employee on the same date(s)
            var requestStartDate = request.StartTime.Date;
            var requestEndDate = request.EndTime.Date;
            
            var existingShiftsOnDate = await shiftRepository.GetAllAsync(
                s => s.EmployeeUserId == request.EmployeeUserId &&
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

            var shift = new Shift
            {
                EmployeeUserId = request.EmployeeUserId,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
                MaximumAllowedLateMinutes = request.MaximumAllowedLateMinutes,
                MaximumAllowedEarlyLeaveMinutes = request.MaximumAllowedEarlyLeaveMinutes,
                Description = request.IsManager ? "Assigned by manager. " + request.Description : request.Description,
                Status = request.IsManager ? ShiftStatus.Approved : ShiftStatus.Pending,
                IsActive = true
            };

            var createdShift = await shiftRepository.AddAsync(shift, cancellationToken);
            var shiftDto = new ShiftDto
            {
                Id = createdShift.Id,
                EmployeeUserId = createdShift.EmployeeUserId,
                StartTime = createdShift.StartTime,
                EndTime = createdShift.EndTime,
                MaximumAllowedLateMinutes = createdShift.MaximumAllowedLateMinutes,
                MaximumAllowedEarlyLeaveMinutes = createdShift.MaximumAllowedEarlyLeaveMinutes,
                Description = createdShift.Description,
                Status = createdShift.Status,
                CreatedAt = createdShift.CreatedAt
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
