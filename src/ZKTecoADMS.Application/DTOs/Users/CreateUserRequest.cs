namespace ZKTecoADMS.Application.DTOs.Users;

public record CreateUserRequest(
    string Pin, 
    string Name,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Department,
    List<Guid> DeviceIds);