using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.Shifts.ApproveShift;

public class ApproveShiftHandler(IShiftService shiftService) 
    : ICommandHandler<ApproveShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(ApproveShiftCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var approvedShift = await shiftService.ApproveShiftAsync(
                request.Id, 
                request.ApprovedByUserId, 
                cancellationToken);

            return AppResponse<ShiftDto>.Success(approvedShift.Adapt<ShiftDto>());
        }
        catch (InvalidOperationException ex)
        {
            return AppResponse<ShiftDto>.Error(ex.Message);
        }
    }
}
