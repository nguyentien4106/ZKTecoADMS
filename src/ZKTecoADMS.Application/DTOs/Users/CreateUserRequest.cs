namespace ZKTecoADMS.Application.DTOs.Users;

public record CreateUserRequest(
    string Pin, 
    string Name,
    string? CardNumber,
    string? Password,
    int Privilege,
    string? Email,
    string? PhoneNumber,
    string? Department,
    List<Guid> DeviceIds,
    string FirstName,
    string LastName,
    string AccountPassword);