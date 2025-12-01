using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.UpdateLeave;

public class UpdateLeaveHandler(
    IRepository<Leave> leaveRepository,
    IRepository<Shift> shiftRepository
    ) : ICommandHandler<UpdateLeaveCommand, AppResponse<LeaveDto>>
{
    public async Task<AppResponse<LeaveDto>> Handle(UpdateLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var leave = await leaveRepository.GetByIdAsync(request.LeaveId, [nameof(Leave.Shift), $"{nameof(Leave.Shift)}.{nameof(Shift.EmployeeUser)}"], cancellationToken: cancellationToken);
            if (leave == null)
            {
                return AppResponse<LeaveDto>.Error("Leave request not found");
            }

            // Permission check: Regular users can only edit their own pending leaves
            if (!request.IsManager)
            {
                if (leave.EmployeeUserId != request.CurrentUserId)
                {
                    return AppResponse<LeaveDto>.Error("You can only edit your own leave requests");
                }

                if (leave.Status != LeaveStatus.Pending)
                {
                    return AppResponse<LeaveDto>.Error("You can only edit pending leave requests");
                }
            }
            // Managers can edit any leave regardless of status

            // Validate the new shift
            var shift = await shiftRepository.GetByIdAsync(request.ShiftId, [nameof(Shift.EmployeeUser)], cancellationToken: cancellationToken);
            if (shift == null)
            {
                return AppResponse<LeaveDto>.Error("Invalid shift");
            }

            if (!shift.IsActive)
            {
                return AppResponse<LeaveDto>.Error("Shift is not available for leave application");
            }

            // For non-managers, ensure the shift belongs to them
            if (!request.IsManager && shift.EmployeeUserId != request.CurrentUserId)
            {
                return AppResponse<LeaveDto>.Error("You can only create leave requests for your own shifts");
            }

            // Update leave properties
            leave.ShiftId = request.ShiftId;
            leave.StartDate = request.StartDate;
            leave.EndDate = request.EndDate;
            leave.Type = request.Type;
            leave.IsHalfShift = request.IsHalfShift;
            leave.Reason = request.Reason;
            leave.UpdatedAt = DateTime.Now;

            // Managers can update status
            if (request.IsManager && request.Status.HasValue)
            {
                leave.Status = request.Status.Value;
            }

            await leaveRepository.UpdateAsync(leave, cancellationToken);
            
            var leaveDto = leave.Adapt<LeaveDto>();
            
            return AppResponse<LeaveDto>.Success(leaveDto);
        }
        catch (ArgumentException ex)
        {
            return AppResponse<LeaveDto>.Error(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return AppResponse<LeaveDto>.Error(ex.Message);
        }
    }
}
