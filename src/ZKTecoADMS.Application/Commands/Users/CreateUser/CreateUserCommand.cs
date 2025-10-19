using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public record CreateUserCommand(string PIN, string FullName, string? CardNumber, string? Password, int GroupId, int Privilege, int VerifyMode, string? Email, string? PhoneNumber, string? Department, bool IsActive, List<Guid> DeviceIds) : ICommand<User>;