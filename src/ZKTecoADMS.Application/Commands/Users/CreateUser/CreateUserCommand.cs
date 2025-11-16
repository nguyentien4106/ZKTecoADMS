using ZKTecoADMS.Application.DTOs.Users;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public record CreateUserCommand(
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
    string AccountPassword) : ICommand<AppResponse<List<AppResponse<UserDto>>>>;