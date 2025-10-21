using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Commands.Users.UpdateUser;

public record UpdateUserCommand(
    Guid UserId,
    string PIN, 
    string FullName, 
    string? CardNumber, 
    string? Password, 
    int Privilege, 
    string? Email, 
    string? PhoneNumber, 
    string? Department,
    Guid DeviceId) : ICommand<AppResponse<UserDto>>;