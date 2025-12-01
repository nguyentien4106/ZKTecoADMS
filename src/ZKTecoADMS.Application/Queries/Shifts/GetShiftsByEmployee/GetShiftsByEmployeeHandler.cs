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
            filter: s => s.EmployeeUserId == request.EmployeeUserId && (!request.Status.HasValue || s.Status == request.Status.Value),
            orderBy: query =>  query.OrderByDescending(s => s.StartTime),
            includeProperties: [nameof(Shift.EmployeeUser)],
            cancellationToken: cancellationToken);


        return AppResponse<List<ShiftDto>>.Success(shifts.Adapt<List<ShiftDto>>());
    }
}
