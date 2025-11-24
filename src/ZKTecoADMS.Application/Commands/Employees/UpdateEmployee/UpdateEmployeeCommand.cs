using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;

public record UpdateEmployeeCommand(
    Guid EmployeeId,
    string PIN, 
    string Name, 
    string? CardNumber, 
    string? Password, 
    int Privilege, 
    string? Email, 
    string? PhoneNumber, 
    string? Department,
    Guid DeviceId) : ICommand<AppResponse<EmployeeDto>>;
