using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.CQRS;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Repositories;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public class CreateUserHandler(
    IDeviceService deviceService, 
    IRepository<User> userRepository,
    IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateUserCommand, List<User>>
{
    public async Task<List<User>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var users = request.DeviceIds.Select(deviceId => new User
            {
                PIN = request.PIN,
                FullName = request.FullName,
                CardNumber = request.CardNumber,
                Password = request.Password,
                GroupId = request.GroupId,
                Privilege = request.Privilege,
                VerifyMode = request.VerifyMode,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                IsActive = true,
                DeviceId = deviceId
            })
            .ToList();

        foreach (var user in users)
        {
            var result = await userRepository.AddAsync(user, cancellationToken);
            if (result != null)
            {
                var cmd = new DeviceCommand
                {
                    DeviceId = result.DeviceId,
                    Command = ClockCommandBuilder.BuildAddUserCommand(result),
                    Priority = 10
                };
                await deviceCmdRepository.AddAsync(cmd, cancellationToken);
                
            }
        }
        
        return users;
    }
}