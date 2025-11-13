using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Entities;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

[DeviceCommandStrategy(Domain.Enums.DeviceCommandTypes.SyncAttendances)]
public class SyncAttendancesStrategy : IDeviceCommandStrategy
{
    public Task ExecuteAsync(Device device, Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        // Implementation pending - currently no action required
        return Task.CompletedTask;
    }
}