using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

/// <summary>
/// Strategy for handling DeleteUser command responses
/// </summary>
[DeviceCommandStrategy(DeviceCommandTypes.DeleteUser)]
public class DeleteUserStrategy(IRepository<User> userRepository) : IDeviceCommandStrategy
{
    public async Task ExecuteAsync(Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
        if (user != null && response.IsSuccess)
        {
            await userRepository.DeleteAsync(user, cancellationToken);
        }
    }
}
