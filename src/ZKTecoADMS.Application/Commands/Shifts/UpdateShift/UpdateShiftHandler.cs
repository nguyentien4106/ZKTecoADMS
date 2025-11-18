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
                includeProperties: new[] { nameof(Shift.ApplicationUser), nameof(Shift.ApprovedByUser) },
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

            shift.StartTime = request.StartTime;
            shift.EndTime = request.EndTime;
            shift.Description = request.Description;

            await repository.UpdateAsync(shift, cancellationToken);
            
            return AppResponse<ShiftDto>.Success(shift.Adapt<ShiftDto>());
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
