using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.CreateShift;

public class CreateShiftHandler(IShiftRepository shiftRepository) 
    : ICommandHandler<CreateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(CreateShiftCommand request, CancellationToken cancellationToken)
    {
        // Validate dates
        if (request.StartTime >= request.EndTime)
        {
            return AppResponse<ShiftDto>.Error("Start time must be before end time");
        }

        if (request.StartTime < DateTime.Now)
        {
            return AppResponse<ShiftDto>.Error("Cannot create shift for past dates");
        }

        var shift = new Shift
        {
            ApplicationUserId = request.ApplicationUserId,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            Description = request.Description,
            Status = ShiftStatus.Pending
        };

        var createdShift = await shiftRepository.AddAsync(shift, cancellationToken);
        
        var shiftDto = new ShiftDto
        {
            Id = createdShift.Id,
            ApplicationUserId = createdShift.ApplicationUserId,
            StartTime = createdShift.StartTime,
            EndTime = createdShift.EndTime,
            Description = createdShift.Description,
            Status = createdShift.Status,
            CreatedAt = createdShift.CreatedAt
        };

        return AppResponse<ShiftDto>.Success(shiftDto);
    }
}
