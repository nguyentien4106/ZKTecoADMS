using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;

public class GetShiftsByEmployeeHandler(IShiftRepository shiftRepository) 
    : IQueryHandler<GetShiftsByEmployeeQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetShiftsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftRepository.GetShiftsByApplicationUserIdAsync(request.ApplicationUserId, cancellationToken);
        
        var shiftDtos = shifts.Select(s => new ShiftDto
        {
            Id = s.Id,
            ApplicationUserId = s.ApplicationUserId,
            EmployeeName = s.ApplicationUser != null 
                ? $"{s.ApplicationUser.FirstName} {s.ApplicationUser.LastName}".Trim() 
                : string.Empty,
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
