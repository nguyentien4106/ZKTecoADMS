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
            var workingDates = request.WorkingDays.Select(i => i.StartTime.Date);
            var existingShifts = await shiftRepository.GetAllAsync(
                s => s.EmployeeUserId == request.EmployeeUserId &&
                     workingDates.Contains(s.StartTime.Date) &&
                     s.Status == ShiftStatus.Approved,
                cancellationToken: cancellationToken);

            if (existingShifts.Any())
            {
                return AppResponse<ShiftDto>.Error("Only one approved shift is allowed per day for an employee. Existing conflicting dates: " +
                    string.Join(", ", existingShifts.Select(s => s.StartTime.Date.ToString("yyyy-MM-dd"))));
            }

            var shifts = request.WorkingDays.Select(day =>
            {
                return new Shift
                {
                    EmployeeUserId = request.EmployeeUserId,
                    StartTime = day.StartTime,
                    EndTime = day.EndTime,
                    MaximumAllowedLateMinutes = request.MaximumAllowedLateMinutes,
                    MaximumAllowedEarlyLeaveMinutes = request.MaximumAllowedEarlyLeaveMinutes,
                    Description = request.IsManager ? "Assigned by manager. " + request.Description : request.Description,
                    Status = request.IsManager ? ShiftStatus.Approved : ShiftStatus.Pending,
                    IsActive = true
                };
            }).ToList();


            var createdShift = await shiftRepository.AddRangeAsync(shifts, cancellationToken);
            var shiftDto = createdShift.Adapt<ShiftDto>();
            
            return AppResponse<ShiftDto>.Success(shiftDto);
        }

        catch (Exception ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
    }
}
