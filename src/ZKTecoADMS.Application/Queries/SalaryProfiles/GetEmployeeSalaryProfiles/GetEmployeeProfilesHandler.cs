using ZKTecoADMS.Application.DTOs.SalaryProfiles;
using ZKTecoADMS.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ZKTecoADMS.Application.Queries.SalaryProfiles;

public class GetEmployeeProfilesHandler(
    IRepositoryPagedQuery<EmployeeSalaryProfile> repositoryPagedQuery,
    IRepositoryPagedQuery<Employee> employeeRepositoryPagedQuery
    ) : IQueryHandler<GetEmployeeProfilesQuery, AppResponse<PagedResult<EmployeeSalaryProfileDto>>>
{

    public async Task<AppResponse<PagedResult<EmployeeSalaryProfileDto>>> Handle(GetEmployeeProfilesQuery request, CancellationToken cancellationToken)
    {
        var res = await employeeRepositoryPagedQuery.GetPagedResultAsync(
            request.PaginationRequest,
            filter: e => e.ManagerId == request.ManagerId,
            cancellationToken: cancellationToken);
            
        var pagedResult = await repositoryPagedQuery.GetPagedResultWithIncludesAsync(
            request.PaginationRequest,
            filter: esp => esp.Employee.ManagerId == request.ManagerId,
            includes: query => query.Include(esp => esp.Employee),
            cancellationToken: cancellationToken);

        var result = new PagedResult<EmployeeSalaryProfileDto>
        {
            Items = pagedResult.Items.Adapt<List<EmployeeSalaryProfileDto>>(),
            TotalCount = pagedResult.TotalCount,
            PageSize = pagedResult.PageSize,
        };

        return AppResponse<PagedResult<EmployeeSalaryProfileDto>>.Success(result);
    }
}