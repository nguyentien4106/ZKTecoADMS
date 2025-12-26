using MediatR;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployees;

public class GetEmployeesHandler(
    IRepositoryPagedQuery<Employee> employeeRepository
    ) 
    : IRequestHandler<GetEmployeesQuery, AppResponse<PagedResult<EmployeeDto>>>
{
    public async Task<AppResponse<PagedResult<EmployeeDto>>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var pagedResult = await employeeRepository.GetPagedResultAsync(
            request.PaginationRequest,
            filter: e => e.ManagerId == request.ManagerId && 
                    (string.IsNullOrEmpty(request.SearchTerm) || 
                    e.EmployeeCode.Contains(request.SearchTerm) ||
                    e.FirstName.Contains(request.SearchTerm) ||
                    e.LastName.Contains(request.SearchTerm) ||
                    (e.PersonalEmail != null && e.PersonalEmail.Contains(request.SearchTerm)) ||
                    (e.CompanyEmail != null && e.CompanyEmail.Contains(request.SearchTerm))) && 
                    (string.IsNullOrEmpty(request.EmploymentType) || (int)e.EmploymentType == int.Parse(request.EmploymentType)) &&
                    (string.IsNullOrEmpty(request.WorkStatus) || (int)e.WorkStatus == int.Parse(request.WorkStatus))
        );

        var result = new PagedResult<EmployeeDto>(pagedResult.Items.Adapt<List<EmployeeDto>>(), pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);

        
        return AppResponse<PagedResult<EmployeeDto>>.Success(result);
    }
}
