using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public record CreateUserCommand(
    string PIN, 
    string FullName, 
    string? CardNumber, 
    string? Password, 
    int Privilege, 
    string? Email, 
    string? PhoneNumber, 
    string? Department,
    List<Guid> DeviceIds) : ICommand<AppResponse<List<AppResponse<UserDto>>>>;