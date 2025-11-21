using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.CreateLeave;

public class CreateLeaveHandler(IRepository<Leave> leaveRepository)
    : ICommandHandler<CreateLeaveCommand, AppResponse<LeaveDto>>
{
    public async Task<AppResponse<LeaveDto>> Handle(CreateLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate date range
            if (request.StartDate >= request.EndDate)
            {
                return AppResponse<LeaveDto>.Error("Start date must be before end date");
            }

            // Validate dates are not in the past
            if (request.StartDate < DateTime.Now.Date)
            {
                return AppResponse<LeaveDto>.Error("Cannot create leave for past dates");
            }

            // Check for overlapping leaves
            var overlappingLeave = await leaveRepository.GetSingleAsync(
                l => l.ApplicationUserId == request.ApplicationUserId &&
                    l.Status != LeaveStatus.Rejected &&
                    l.Status != LeaveStatus.Cancelled &&
                    (
                        // New leave starts during an existing leave
                        (request.StartDate >= l.StartDate && request.StartDate < l.EndDate) ||
                        // New leave ends during an existing leave
                        (request.EndDate > l.StartDate && request.EndDate <= l.EndDate) ||
                        // New leave completely encompasses an existing leave
                        (request.StartDate <= l.StartDate && request.EndDate >= l.EndDate)
                    ),
                cancellationToken: cancellationToken
            );

            if (overlappingLeave != null)
            {
                return AppResponse<LeaveDto>.Error(
                    $"This leave overlaps with an existing leave from {overlappingLeave.StartDate:d} to {overlappingLeave.EndDate:d}");
            }

            var leave = new Leave
            {
                ApplicationUserId = request.ApplicationUserId,
                Type = request.Type,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                IsFullDay = request.IsFullDay,
                Reason = request.Reason,
                Status = LeaveStatus.Pending
            };

            var createdLeave = await leaveRepository.AddAsync(leave, cancellationToken);
            
            var leaveDto = new LeaveDto
            {
                Id = createdLeave.Id,
                ApplicationUserId = createdLeave.ApplicationUserId,
                Type = createdLeave.Type,
                StartDate = createdLeave.StartDate,
                EndDate = createdLeave.EndDate,
                IsFullDay = createdLeave.IsFullDay,
                Reason = createdLeave.Reason,
                Status = createdLeave.Status,
                CreatedAt = createdLeave.CreatedAt
            };

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
