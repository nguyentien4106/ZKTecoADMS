using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.DeleteShift;

public class DeleteShiftHandler(IShiftRepository shiftRepository) 
    : ICommandHandler<DeleteShiftCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await shiftRepository.GetByIdAsync(request.Id, null, cancellationToken);
        if (shift == null)
        {
            return AppResponse<bool>.Error("Shift not found");
        }

        // Only allow deletion of pending shifts
        if (shift.Status != ShiftStatus.Pending)
        {
            return AppResponse<bool>.Error("Can only delete pending shifts");
        }

        await shiftRepository.DeleteAsync(shift, cancellationToken);
        return AppResponse<bool>.Success(true);
    }
}
