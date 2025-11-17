using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Commands.Employees.CreateEmployee;

public record CreateEmployeeCommand(
    string Pin, 
    string Name, 
    string? CardNumber, 
    string? Password, 
    int Privilege, 
    string? Department,
    List<Guid> DeviceIds) : ICommand<AppResponse<List<AppResponse<EmployeeDto>>>>;
