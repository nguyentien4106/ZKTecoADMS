using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.ApproveLeave;

public class ApproveLeaveHandler(
    IRepository<Leave> leaveRepository,
    IRepository<Shift> shiftRepository
    )
    : ICommandHandler<ApproveLeaveCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await leaveRepository.GetByIdAsync(
                request.LeaveId,
                includeProperties: [nameof(Leave.EmployeeUser), nameof(Leave.Shift)],
                cancellationToken: cancellationToken);

            if (leave == null)
            {
                return AppResponse<bool>.Error("Leave not found");
            }

            if (leave.Status != LeaveStatus.Pending)
            {
                return AppResponse<bool>.Error($"Cannot approve leave with status: {leave.Status}");
            }

            leave.Status = LeaveStatus.Approved;
            leave.UpdatedAt = DateTime.Now;

            if (leave.Shift != null)
            {
                leave.Shift.Status = ShiftStatus.ApprovedLeave;
                leave.Shift.UpdatedAt = DateTime.Now;
                await shiftRepository.UpdateAsync(leave.Shift, cancellationToken);
            }
            
            await leaveRepository.UpdateAsync(leave, cancellationToken);

            return AppResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return AppResponse<bool>.Error(ex.Message);
        }
    }
}
