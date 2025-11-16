using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.DTOs.Users;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Users.UpdateUser;

public class UpdateUserHandler(
    IRepository<User> userRepository,
    IRepository<DeviceCommand> deviceCmdRepository
    ) : ICommandHandler<UpdateUserCommand, AppResponse<UserDto>>
{
    public async Task<AppResponse<UserDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);
        user.Pin = request.PIN;
        user.Name = request.FullName;
        user.Email = request.Email;
        user.CardNumber = request.CardNumber;
        user.Password = request.Password;
        user.Privilege = request.Privilege;
        user.PhoneNumber = request.PhoneNumber;
        user.Department = request.Department;
        user.IsActive = false;
        
        await userRepository.UpdateAsync(user, cancellationToken);
        
        var cmd = new DeviceCommand
        {
            DeviceId = user.DeviceId,
            Command = ClockCommandBuilder.BuildAddOrUpdateUserCommand(user),
            Priority = 10,
            Status = CommandStatus.Created,
            CommandType = DeviceCommandTypes.UpdateUser,
            ObjectReferenceId = user.Id,
        };
        await deviceCmdRepository.AddAsync(cmd, cancellationToken);
        
        return AppResponse<UserDto>.Success(user.Adapt<UserDto>());
    }
}