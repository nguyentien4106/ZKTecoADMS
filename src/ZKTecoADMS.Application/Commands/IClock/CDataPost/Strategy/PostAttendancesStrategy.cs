using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

/// <summary>
/// Handles attendance log data uploads from device to server.
/// Format: [PIN]\t[Punch date/time]\t[Attendance State]\t[Verify Mode]\t[Workcode]\t[Reserved 1]\t[Reserved 2]
/// </summary>
public class PostAttendancesStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IAttendanceOperationService _attendanceOperationService = serviceProvider.GetRequiredService<IAttendanceOperationService>();
    private readonly IAttendanceService _attendanceService = serviceProvider.GetRequiredService<IAttendanceService>();
    private readonly ILogger<PostAttendancesStrategy> _logger = serviceProvider.GetRequiredService<ILogger<PostAttendancesStrategy>>();
    private readonly IShiftService _shiftService = serviceProvider.GetRequiredService<IShiftService>();

    public async Task<string> ProcessDataAsync(Device device, string body)
    {
        // Step 1: Parse and process attendances from device data
        var attendances = await _attendanceOperationService.ProcessAttendancesFromDeviceAsync(device, body);

        if (attendances.Count == 0)
        {
            _logger.LogWarning("Device-SN-{SN}: no valid attendance records to save from device {DeviceId}", device.SerialNumber, device.Id);
            return ClockResponses.Fail;
        }

        // Step 2: Persist attendances to database
        await _attendanceService.CreateAttendancesAsync(attendances);

        _logger.LogInformation("Device-SN-{SN}: successfully saved {Count} attendance records from device {DeviceId}", device.SerialNumber, attendances.Count, device.Id);

        await _attendanceService.UpdateShiftAttendancesAsync(attendances, device);
        
        return ClockResponses.Ok; 
    }
}