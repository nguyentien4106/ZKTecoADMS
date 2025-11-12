using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

/// <summary>
/// Strategy for handling InitialUsers command responses
/// </summary>
[DeviceCommandStrategy(DeviceCommandTypes.InitialUsers)]
public class InitialUsersStrategy : IDeviceCommandStrategy
{
    public Task ExecuteAsync(Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        // Implementation pending - currently no action required
        return Task.CompletedTask;
    }
}
