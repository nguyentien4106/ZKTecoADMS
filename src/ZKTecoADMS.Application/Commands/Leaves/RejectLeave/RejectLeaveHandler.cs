using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.RejectLeave;

public class RejectLeaveHandler(IRepository<Leave> leaveRepository)
    : ICommandHandler<RejectLeaveCommand, AppResponse<LeaveDto>>
{
    public async Task<AppResponse<LeaveDto>> Handle(RejectLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await leaveRepository.GetByIdAsync(
                request.LeaveId,
                includeProperties: new[] { "ApplicationUser" },
                cancellationToken: cancellationToken);

            if (leave == null)
            {
                return AppResponse<LeaveDto>.Error("Leave not found");
            }

            if (leave.Status != LeaveStatus.Pending)
            {
                return AppResponse<LeaveDto>.Error($"Cannot reject leave with status: {leave.Status}");
            }

            leave.Status = LeaveStatus.Rejected;
            leave.ApprovedByUserId = request.RejectedByUserId;
            leave.ApprovedAt = DateTime.UtcNow;
            leave.RejectionReason = request.RejectionReason;

            await leaveRepository.UpdateAsync(leave, cancellationToken);

            var leaveDto = new LeaveDto
            {
                Id = leave.Id,
                ApplicationUserId = leave.ApplicationUserId,
                EmployeeName = leave.ApplicationUser?.FirstName + " " + leave.ApplicationUser?.LastName ?? "",
                Type = leave.Type,
                StartDate = leave.StartDate,
                EndDate = leave.EndDate,
                IsFullDay = leave.IsFullDay,
                Reason = leave.Reason,
                Status = leave.Status,
                ApprovedByUserId = leave.ApprovedByUserId,
                ApprovedAt = leave.ApprovedAt,
                RejectionReason = leave.RejectionReason,
                CreatedAt = leave.CreatedAt,
                UpdatedAt = leave.UpdatedAt
            };

            return AppResponse<LeaveDto>.Success(leaveDto);
        }
        catch (Exception ex)
        {
            return AppResponse<LeaveDto>.Error(ex.Message);
        }
    }
}
