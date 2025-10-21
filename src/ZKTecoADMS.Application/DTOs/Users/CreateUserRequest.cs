namespace ZKTecoADMS.Application.DTOs.Users;

public record CreateUserRequest(
    string PIN, 
    string FullName,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Email,
    string? PhoneNumber,
    string? Department,
    List<Guid> DeviceIds);