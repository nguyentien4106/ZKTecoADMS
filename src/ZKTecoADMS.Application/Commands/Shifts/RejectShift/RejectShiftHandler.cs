using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Shifts.RejectShift;

public class RejectShiftHandler(IShiftService shiftService) 
    : ICommandHandler<RejectShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(RejectShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var rejectedShift = await shiftService.RejectShiftAsync(
                request.Id,
                request.RejectedByUserId,
                request.RejectionReason,
                cancellationToken);
            
            return AppResponse<ShiftDto>.Success(rejectedShift.Adapt<ShiftDto>());
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
