using System.Text;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Extensions;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;

public class DeviceCmdHandler(
    IDeviceCmdService deviceCmdService,
    IRepository<User> userRepository
    ) : ICommandHandler<DeviceCmdCommand, string>
{
    public async Task<string> Handle(DeviceCmdCommand request, CancellationToken cancellationToken)
    {
        var response = request.Body.ParseClockResponse();
        
        await deviceCmdService.UpdateCommandAfterExecutedAsync(response);
        var (commandType, objectRefId) = await deviceCmdService.GetCommandTypesAndIdAsync(response.CommandId);

        if (commandType == DeviceCommandTypes.AddUser)
        {
            var user = await userRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
            user.IsActive = response.IsSuccess;
            await userRepository.UpdateAsync(user, cancellationToken);
        }
        else if (commandType == DeviceCommandTypes.DeleteUser)
        {
            var user = await userRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
            if (response.IsSuccess)
            {
                await userRepository.DeleteAsync(user, cancellationToken);
            }
        }

        else if (commandType == DeviceCommandTypes.UpdateUser)
        {
            var user = await userRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
            user.IsActive = response.IsSuccess;
            await userRepository.UpdateAsync(user, cancellationToken);
        }
        
        else if(commandType == DeviceCommandTypes.InitialUsers)
        {
            
        }
        
        return ClockResponses.Ok;
    }
}