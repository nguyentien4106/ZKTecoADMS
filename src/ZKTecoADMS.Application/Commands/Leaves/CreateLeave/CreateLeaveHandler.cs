using ZKTecoADMS.Application.DTOs.Leaves;
using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Leaves.CreateLeave;

public class CreateLeaveHandler(IRepository<Leave> leaveRepository,
    IRepository<Shift> shiftRepository)
    : ICommandHandler<CreateLeaveCommand, AppResponse<LeaveDto>>
{
    public async Task<AppResponse<LeaveDto>> Handle(CreateLeaveCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var shift = await shiftRepository.GetByIdAsync(request.ShiftId, cancellationToken: cancellationToken);
            if (shift == null)
            {
                return AppResponse<LeaveDto>.Error("Invalid shift for the specified employee");
            }   
            if(!shift.IsActive)
            {
                return AppResponse<LeaveDto>.Error("Shift is not available for leave application");
            }
            var leave = request.Adapt<Leave>();
            leave.Shift = shift;

            var createdLeave = await leaveRepository.AddAsync(leave, cancellationToken);
            
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
