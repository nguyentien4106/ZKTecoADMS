using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Queries.Shifts.GetMyShifts;

public class GetMyShiftsQuery() : IQuery<AppResponse<List<ShiftDto>>>
{
    public Guid EmployeeId { get; set; }

    public ShiftStatus? Status { get; set; }

    public int Month {get;set;}

    public int Year {get;set;}
    
}