using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Queries.Employees.GetEmployeeDevices;

public record GetEmployeeDevicesQuery(IEnumerable<Guid> DeviceIds) : IQuery<AppResponse<IEnumerable<EmployeeDto>>>;
