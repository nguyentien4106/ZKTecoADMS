namespace ZKTecoADMS.Application.DTOs.Employees;

public record CreateEmployeeRequest(
    string Pin, 
    string Name,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Department,
    List<Guid> DeviceIds);