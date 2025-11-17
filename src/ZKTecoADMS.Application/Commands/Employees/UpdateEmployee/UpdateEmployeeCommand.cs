using ZKTecoADMS.Application.DTOs.Employees;

namespace ZKTecoADMS.Application.Commands.Employees.UpdateEmployee;

public record UpdateEmployeeCommand(
    Guid EmployeeId,
    string PIN, 
    string FullName, 
    string? CardNumber, 
    string? Password, 
    int Privilege, 
    string? Email, 
    string? PhoneNumber, 
    string? Department,
    Guid DeviceId) : ICommand<AppResponse<EmployeeDto>>;
