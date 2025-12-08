using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Shifts.UpdateShift;

public class UpdateShiftHandler(IShiftService shiftService) 
    : ICommandHandler<UpdateShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(UpdateShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var updatedShift = await shiftService.UpdateShiftAsync(
                request.Id,
                request.UpdatedByUserId,
                request.CheckInTime,
                request.CheckOutTime,
                cancellationToken);

            return AppResponse<ShiftDto>.Success(updatedShift.Adapt<ShiftDto>());
        }
        catch (InvalidOperationException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
    }
}
