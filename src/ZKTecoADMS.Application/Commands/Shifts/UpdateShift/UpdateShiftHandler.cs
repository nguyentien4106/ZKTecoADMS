using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.UpdateShift;

public class UpdateShiftHandler(IShiftRepository shiftRepository) 
    : ICommandHandler<UpdateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await shiftRepository.GetByIdAsync(request.Id, null, cancellationToken);
        if (shift == null)
        {
            return AppResponse<ShiftDto>.Error("Shift not found");
        }

        // Only allow updates to pending shifts
        if (shift.Status != ShiftStatus.Pending)
        {
            return AppResponse<ShiftDto>.Error("Can only update pending shifts");
        }

        // Validate dates
        if (request.StartTime >= request.EndTime)
        {
            return AppResponse<ShiftDto>.Error("Start time must be before end time");
        }

        if (request.StartTime < DateTime.Now)
        {
            return AppResponse<ShiftDto>.Error("Cannot update shift to past dates");
        }

        shift.StartTime = request.StartTime;
        shift.EndTime = request.EndTime;
        shift.Description = request.Description;

        await shiftRepository.UpdateAsync(shift, cancellationToken);
        
        var shiftDto = new ShiftDto
        {
            Id = shift.Id,
            ApplicationUserId = shift.ApplicationUserId,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            Description = shift.Description,
            Status = shift.Status,
            CreatedAt = shift.CreatedAt,
            UpdatedAt = shift.UpdatedAt
        };

        return AppResponse<ShiftDto>.Success(shiftDto);
    }
}
