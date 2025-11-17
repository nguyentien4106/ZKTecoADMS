using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.ApproveShift;

public class ApproveShiftHandler(IShiftRepository shiftRepository) 
    : ICommandHandler<ApproveShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(ApproveShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await shiftRepository.GetByIdAsync(request.Id, null, cancellationToken);
        if (shift == null)
        {
            return AppResponse<ShiftDto>.Error("Shift not found");
        }

        if (shift.Status != ShiftStatus.Pending)
        {
            return AppResponse<ShiftDto>.Error("Can only approve pending shifts");
        }

        shift.Status = ShiftStatus.Approved;
        shift.ApprovedByUserId = request.ApprovedByUserId;
        shift.ApprovedAt = DateTime.Now;
        shift.RejectionReason = null;

        await shiftRepository.UpdateAsync(shift, cancellationToken);
        
        var shiftDto = new ShiftDto
        {
            Id = shift.Id,
            ApplicationUserId = shift.ApplicationUserId,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            Description = shift.Description,
            Status = shift.Status,
            ApprovedByUserId = shift.ApprovedByUserId,
            ApprovedAt = shift.ApprovedAt,
            CreatedAt = shift.CreatedAt,
            UpdatedAt = shift.UpdatedAt
        };

        return AppResponse<ShiftDto>.Success(shiftDto);
    }
}
