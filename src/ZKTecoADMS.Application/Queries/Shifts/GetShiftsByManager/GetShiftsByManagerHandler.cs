using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByManager;

public class GetShiftsByManagerHandler(IShiftRepository shiftRepository) 
    : IQueryHandler<GetShiftsByManagerQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetShiftsByManagerQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftRepository.GetShiftsByManagerAsync(request.ManagerId, cancellationToken);
        
        var shiftDtos = shifts.Select(s => new ShiftDto
        {
            Id = s.Id,
            ApplicationUserId = s.ApplicationUserId,
            EmployeeName = $"{s.ApplicationUser.FirstName} {s.ApplicationUser.LastName}".Trim(),
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Description = s.Description,
            Status = s.Status,
            ApprovedByUserId = s.ApprovedByUserId,
            ApprovedByUserName = s.ApprovedByUser != null 
                ? $"{s.ApprovedByUser.FirstName} {s.ApprovedByUser.LastName}".Trim() 
                : null,
            ApprovedAt = s.ApprovedAt,
            RejectionReason = s.RejectionReason,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        return AppResponse<List<ShiftDto>>.Success(shiftDtos);
    }
}
