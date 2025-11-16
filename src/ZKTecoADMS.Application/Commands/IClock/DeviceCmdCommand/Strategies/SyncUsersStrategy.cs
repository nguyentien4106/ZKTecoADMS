using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand.Strategies;

/// <summary>
/// Strategy for handling InitialUsers command responses.
/// Processes user data received from device in response to sync command.
/// Format: USER PIN=%s\tName=%s\tPasswd=%d\tCard=%d\tGrp=%d\tTZ=%s
/// </summary>
public class SyncUsersStrategy(
    IRepository<User> userRepository,
    IUserOperationService userOperationService,
    ILogger<SyncUsersStrategy> logger) : IDeviceCommandStrategy
{
    public async Task ExecuteAsync(Device device, Guid objectRefId, ClockCommandResponse response, CancellationToken cancellationToken)
    {
        if (!response.IsSuccess)
        {
            logger.LogWarning("InitialUsers command failed for device {DeviceId}", device.Id);
            return;
        }

        var users = await userOperationService.ProcessUsersFromDeviceAsync(device, response.CMD);

        if (users.Count == 0)
        {
            logger.LogInformation("No user records to save from device {DeviceId} in InitialUsers response", device.Id);
            return;
        }

        await userRepository.AddRangeAsync(users, cancellationToken);
        logger.LogInformation("InitialUsers command completed successfully for device {DeviceId}", device.Id);
        
        // Note: User data is typically sent via separate OPERLOG/CData posts, not in the command response
        // This strategy confirms the command was acknowledged by the device
    }
}
