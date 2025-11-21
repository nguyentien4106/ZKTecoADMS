using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.ApproveLeave;

public class ApproveLeaveHandler(IRepository<Leave> leaveRepository)
    : ICommandHandler<ApproveLeaveCommand, AppResponse<bool>>
{
    public async Task<AppResponse<bool>> Handle(ApproveLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await leaveRepository.GetByIdAsync(
                request.LeaveId,
                includeProperties: ["ApplicationUser", "Shift"],
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
            
            await leaveRepository.UpdateAsync(leave, cancellationToken);

            return AppResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return AppResponse<bool>.Error(ex.Message);
        }
    }
}
