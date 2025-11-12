using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

[DeviceCommandStrategy(Domain.Enums.DeviceCommandTypes.SyncAttendances)]
public class SyncAttendancesStrategy : IDeviceCommandStrategy
{
    public Task ExecuteAsync(Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        // Implementation pending - currently no action required
        return Task.CompletedTask;
    }
}