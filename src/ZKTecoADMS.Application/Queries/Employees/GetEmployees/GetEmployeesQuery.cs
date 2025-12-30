using MediatR;
using ZKTecoADMS.Application.DTOs.Employees;
using ZKTecoADMS.Application.Models;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployees;

public class GetEmployeesQuery : IRequest<AppResponse<List<EmployeeDto>>>
{
    public string? SearchTerm { get; set; }
    public string? EmploymentType { get; set; }
    public string? WorkStatus { get; set; }

    public Guid ManagerId { get; set; }
}
