using Microsoft.EntityFrameworkCore;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Shifts.ApproveShifts;

public class ApproveShiftsHandler(
    IRepository<Shift> shiftRepository
    ) : ICommandHandler<ApproveShiftsCommand, AppResponse<List<Guid>>>
{
    public async Task<AppResponse<List<Guid>>> Handle(ApproveShiftsCommand request, CancellationToken cancellationToken)
    {
        var shifts = await shiftRepository.GetQuery()
            .Where(s => request.Ids.Contains(s.Id))
            .ToListAsync(cancellationToken);
        
        foreach (var shift in shifts)
        {
            shift.Status = ShiftStatus.Approved;
            shift.RejectionReason = null;
            await shiftRepository.UpdateAsync(shift, cancellationToken);
        }
        
        return AppResponse<List<Guid>>.Success(request.Ids);
    }
}