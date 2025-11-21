using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.CancelLeave;

public class CancelLeaveHandler(IRepository<Leave> leaveRepository)
    : ICommandHandler<CancelLeaveCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(CancelLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await leaveRepository.GetByIdAsync(request.LeaveId, cancellationToken: cancellationToken);

            if (leave == null)
            {
                return AppResponse<bool>.Error("Leave not found");
            }

            // Verify ownership
            if (leave.EmployeeUserId != request.ApplicationUserId)
            {
                return AppResponse<bool>.Error("You are not authorized to cancel this leave request");
            }

            // Only pending leaves can be cancelled
            if (leave.Status != LeaveStatus.Pending)
            {
                return AppResponse<bool>.Error($"Cannot cancel leave with status: {leave.Status}");
            }

            leave.Status = LeaveStatus.Cancelled;
            await leaveRepository.UpdateAsync(leave, cancellationToken);

            return AppResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return AppResponse<bool>.Error(ex.Message);
        }
    }
}
