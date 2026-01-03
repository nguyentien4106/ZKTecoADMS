using ZKTecoADMS.Application.DTOs.Shifts;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Application.Queries.Shifts.GetMyExchangeRequests;

public class GetMyExchangeRequestsQuery : IQuery<AppResponse<List<ShiftExchangeRequestDto>>>
{
    public Guid EmployeeId { get; set; }
    
    public bool IncomingOnly { get; set; } = false;
}
