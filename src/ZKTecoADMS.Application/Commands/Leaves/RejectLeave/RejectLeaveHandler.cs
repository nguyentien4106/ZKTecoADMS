using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.RejectLeave;

public class RejectLeaveHandler(
    IRepository<Leave> leaveRepository
    )
    : ICommandHandler<RejectLeaveCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(RejectLeaveCommand request, CancellationToken cancellationToken)
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
                return AppResponse<bool>.Error($"Cannot reject leave with status: {leave.Status}");
            }

            leave.Status = LeaveStatus.Rejected;
            leave.UpdatedAt = DateTime.Now;
            leave.RejectionReason = request.RejectionReason;

            await leaveRepository.UpdateAsync(leave, cancellationToken);

            return AppResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return AppResponse<bool>.Error(ex.Message);
        }
    }
}
