using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.Users.DeleteUser;

public class DeleteUserHandler(
    IRepository<User> userRepository,
    IRepository<DeviceCommand> deviceCmdRepository) : ICommandHandler<DeleteUserCommand, AppResponse<Guid>>
{
    public async Task<AppResponse<Guid>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken: cancellationToken);

        user.IsActive = false;
        await userRepository.UpdateAsync(user, cancellationToken);
        
        var cmd = new DeviceCommand
        {
            DeviceId = user.DeviceId,
            Command = ClockCommandBuilder.BuildDeleteUserCommand(user.PIN),
            Status = CommandStatus.Created,
            CommandType = DeviceCommandTypes.DeleteUser,
            ObjectReferenceId = user.Id
        };
        
        await deviceCmdRepository.AddAsync(cmd, cancellationToken);
        
        return AppResponse<Guid>.Success(user.Id);
    }
}