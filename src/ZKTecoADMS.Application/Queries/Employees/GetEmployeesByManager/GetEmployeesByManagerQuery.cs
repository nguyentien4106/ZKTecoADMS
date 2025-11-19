using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployeesByManager;

public record GetEmployeesByManagerQuery(Guid ManagerId) : IQuery<AppResponse<IEnumerable<AccountDto>>>;
