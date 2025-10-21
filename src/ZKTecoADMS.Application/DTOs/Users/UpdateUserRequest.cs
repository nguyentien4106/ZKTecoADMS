namespace ZKTecoADMS.Application.DTOs.Users;

public record UpdateUserRequest(
    string PIN, 
    string FullName,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Email,
    string? PhoneNumber,
    string? Department,
    Guid DeviceId);