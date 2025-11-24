namespace ZKTecoADMS.Application.DTOs.Employees;

public record UpdateEmployeeRequest(
    string PIN, 
    string Name,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Email,
    string? PhoneNumber,
    string? Department,
    Guid DeviceId);