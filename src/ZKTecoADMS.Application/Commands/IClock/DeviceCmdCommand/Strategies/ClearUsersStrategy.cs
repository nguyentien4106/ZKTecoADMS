using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

[DeviceCommandStrategy(DeviceCommandTypes.ClearEmployees)]
public class ClearUsersStrategy(IRepository<Employee> userRepository) : IDeviceCommandStrategy
{
    public async Task ExecuteAsync(Device device, Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        if (response.IsSuccess)
        {
            await userRepository.DeleteAsync(u => u.DeviceId == device.Id, cancellationToken);
        }
    }
}
