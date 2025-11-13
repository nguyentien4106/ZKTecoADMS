using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

/// <summary>
/// Handles OPERLOG data (user information) uploads from device to server.
/// Format: USER PIN=%s\tName=%s\tPasswd=%d\tCard=%d\tGrp=%d\tTZ=%s
/// </summary>
public class OperLogStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IUserOperationService _userOperationService = serviceProvider.GetRequiredService<IUserOperationService>();
    private readonly IUserRepository _userRepository = serviceProvider.GetRequiredService<IUserRepository>();
    private readonly ILogger<OperLogStrategy> _logger = serviceProvider.GetRequiredService<ILogger<OperLogStrategy>>();

    public async Task<string> ProcessDataAsync(Device device, string body)
    {
        // Step 1: Parse and process users from device data
        var users = await _userOperationService.ProcessUsersFromDeviceAsync(device, body);

        if (users.Count == 0)
        {
            _logger.LogWarning("No valid user records to save from device {DeviceId}", device.Id);
            return ClockResponses.Ok;
        }

        // Step 2: Persist users to database
        await _userRepository.AddRangeAsync(users);

        _logger.LogInformation("Successfully saved {Count} users from device {DeviceId}", users.Count, device.Id);

        return ClockResponses.Ok;
    }
}