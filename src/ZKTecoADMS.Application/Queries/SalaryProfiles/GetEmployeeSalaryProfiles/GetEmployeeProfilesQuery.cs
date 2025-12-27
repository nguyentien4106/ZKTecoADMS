using ZKTecoADMS.Application.DTOs.SalaryProfiles;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles;

public class GetEmployeeProfilesQuery : IQuery<AppResponse<PagedResult<EmployeeSalaryProfileDto>>>
{
    public PaginationRequest PaginationRequest {get;set;} = new();
    public Guid ManagerId { get; set; }
}