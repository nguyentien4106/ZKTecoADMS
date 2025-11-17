using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Shifts.GetPendingShifts;

public class GetPendingShiftsHandler(IShiftRepository shiftRepository) 
    : IQueryHandler<GetPendingShiftsQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetPendingShiftsQuery request, CancellationToken cancellationToken)
    {
        var shifts = await shiftRepository.GetPendingShiftsAsync(cancellationToken);
        
        var shiftDtos = shifts.Select(s => new ShiftDto
        {
            Id = s.Id,
            ApplicationUserId = s.ApplicationUserId,
            EmployeeName = $"{s.ApplicationUser.FirstName} {s.ApplicationUser.LastName}".Trim(),
            StartTime = s.StartTime,
            EndTime = s.EndTime,
            Description = s.Description,
            Status = s.Status,
            CreatedAt = s.CreatedAt,
            UpdatedAt = s.UpdatedAt
        }).ToList();

        return AppResponse<List<ShiftDto>>.Success(shiftDtos);
    }
}
