using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Shifts.AssignShift;

public class AssignShiftHandler(
    IRepository<Shift> shiftRepository
    ) : ICommandHandler<AssignShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(AssignShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var workingDates = request.WorkingDays.Select(i => i.StartTime.Date);
            var existingShifts = await shiftRepository.GetAllAsync(
                s => s.EmployeeId == request.EmployeeId &&
                     workingDates.Contains(s.StartTime.Date) &&
                     (s.Status == ShiftStatus.Approved),
                cancellationToken: cancellationToken);

            if (existingShifts.Any())
            {
                return AppResponse<ShiftDto>.Error("Only one approved shift is allowed per day for an employee. Existing conflicting dates: " +
                    string.Join(", ", existingShifts.Select(s => s.StartTime.Date.ToString("yyyy-MM-dd"))));
            }

            var shifts = request.WorkingDays.Select(day => new Shift
            {
                EmployeeId = request.EmployeeId,
                StartTime = day.StartTime,
                EndTime = day.EndTime,
                BreakTimeMinutes = request.BreakTimeMinutes,
                Description = "Assigned by manager. " + (request.Description ?? ""),
                Status = ShiftStatus.Approved,
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