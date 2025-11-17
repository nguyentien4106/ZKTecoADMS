using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Shifts.RejectShift;

public class RejectShiftHandler(IShiftRepository shiftRepository) 
    : ICommandHandler<RejectShiftCommand, AppResponse<ShiftDto>>
{
    public async Task<AppResponse<ShiftDto>> Handle(RejectShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await shiftRepository.GetByIdAsync(request.Id, null, cancellationToken);
        if (shift == null)
        {
            return AppResponse<ShiftDto>.Error("Shift not found");
        }

        if (shift.Status != ShiftStatus.Pending)
        {
            return AppResponse<ShiftDto>.Error("Can only reject pending shifts");
        }

        if (string.IsNullOrWhiteSpace(request.RejectionReason))
        {
            return AppResponse<ShiftDto>.Error("Rejection reason is required");
        }

        shift.Status = ShiftStatus.Rejected;
        shift.ApprovedByUserId = request.RejectedByUserId;
        shift.ApprovedAt = DateTime.Now;
        shift.RejectionReason = request.RejectionReason;

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
            RejectionReason = shift.RejectionReason,
            CreatedAt = shift.CreatedAt,
            UpdatedAt = shift.UpdatedAt
        };

        return AppResponse<ShiftDto>.Success(shiftDto);
    }
}
