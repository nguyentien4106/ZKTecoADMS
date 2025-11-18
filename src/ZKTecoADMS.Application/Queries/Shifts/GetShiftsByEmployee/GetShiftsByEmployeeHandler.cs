using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Queries.Shifts.GetShiftsByEmployee;

public class GetShiftsByEmployeeHandler(IRepository<Shift> repository) 
    : IQueryHandler<GetShiftsByEmployeeQuery, AppResponse<List<ShiftDto>>>
{
    public async Task<AppResponse<List<ShiftDto>>> Handle(GetShiftsByEmployeeQuery request, CancellationToken cancellationToken)
    {
        var shifts = await repository.GetAllAsync(
            filter: s => s.ApplicationUserId == request.ApplicationUserId,
            orderBy: query => query.OrderByDescending(s => s.CreatedAt),
            includeProperties: new[] { nameof(Shift.ApplicationUser), nameof(Shift.ApprovedByUser) },
            cancellationToken: cancellationToken);


        return AppResponse<List<ShiftDto>>.Success(shifts.Adapt<List<ShiftDto>>());
    }
}
