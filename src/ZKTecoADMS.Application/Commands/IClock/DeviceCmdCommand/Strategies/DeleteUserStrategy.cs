using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Models;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

/// <summary>
/// Strategy for handling DeleteUser command responses
/// </summary>
[DeviceCommandStrategy(DeviceCommandTypes.DeleteEmployee)]
public class DeleteUserStrategy(IRepository<Employee> userRepository) : IDeviceCommandStrategy
{
    public async Task ExecuteAsync(Device device, Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(objectRefId, cancellationToken: cancellationToken);
        if (user != null && response.IsSuccess)
        {
            await userRepository.DeleteAsync(user, cancellationToken);
        }
    }
}
