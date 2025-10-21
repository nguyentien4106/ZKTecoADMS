using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Users.CreateUser;

public class CreateUserHandler(
    IDeviceService deviceService, 
    IRepository<User> userRepository,
    IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<CreateUserCommand, List<AppResponse<UserDto>>>
{
    public async Task<List<AppResponse<UserDto>>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var users = request.DeviceIds.Select(deviceId => new User
            {
                PIN = request.PIN,
                FullName = request.FullName,
                CardNumber = request.CardNumber,
                Password = request.Password,
                Privilege = request.Privilege,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Department = request.Department,
                DeviceId = deviceId
            })
            .ToList();
        var userResults = new List<AppResponse<UserDto>>();
        
        foreach (var user in users)
        {
            var validUser = await deviceService.IsValidUserAsync(user);
            if (!validUser.IsSuccess)
            {
                userResults.Add(AppResponse<UserDto>.Error(validUser.Errors));
                continue;
            }
            
            var userEntity = await userRepository.AddAsync(user, cancellationToken);
            userResults.Add(AppResponse<UserDto>.Success(userEntity.Adapt<UserDto>()));
            
            var cmd = new DeviceCommand
            {
                DeviceId = userEntity.DeviceId,
                Command = ClockCommandBuilder.BuildAddOrUpdateUserCommand(userEntity),
                Priority = 10,
                Status = CommandStatus.Created,
                CommandType = DeviceCommandTypes.AddUser,
                ObjectReferenceId = userEntity.Id
            };
            await deviceCmdRepository.AddAsync(cmd, cancellationToken);
        }
        
        return userResults;
    }
}