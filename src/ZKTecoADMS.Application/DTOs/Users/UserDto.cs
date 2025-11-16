namespace ZKTecoADMS.Application.DTOs.Users;

public record UserDto(
    Guid Id,
    string Pin,
    string Name,
    string? CardNumber,
    string? Password,
    int GroupId,
    int Privilege,
    int VerifyMode,
    DateTime? StartDatetime,
    DateTime? EndDatetime,
    bool IsActive,
    string? Email,
    string? PhoneNumber,
    string? Department,
    string? Position,
    Guid DeviceId,
    string? DeviceName
);