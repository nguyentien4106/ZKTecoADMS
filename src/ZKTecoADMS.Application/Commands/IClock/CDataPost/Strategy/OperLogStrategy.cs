using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZKTecoADMS.Application.Constants;
using ZKTecoADMS.Application.Interfaces;

namespace ZKTecoADMS.Application.Commands.IClock.CDataPost.Strategy;

/// <summary>
/// Handles OPERLOG data (employee information) uploads from device to server.
/// Format: USER PIN=%s\tName=%s\tPasswd=%d\tCard=%d\tGrp=%d\tTZ=%s
/// </summary>
public class OperLogStrategy(IServiceProvider serviceProvider) : IPostStrategy
{
    private readonly IEmployeeOperationService _employeeOperationService = serviceProvider.GetRequiredService<IEmployeeOperationService>();
    private readonly IEmployeeService _employeeService = serviceProvider.GetRequiredService<IEmployeeService>();
    private readonly ILogger<OperLogStrategy> _logger = serviceProvider.GetRequiredService<ILogger<OperLogStrategy>>();

    public async Task<string> ProcessDataAsync(Device device, string body)
    {
        // Step 1: Parse and process employees from device data
        var employees = await _employeeOperationService.ProcessEmployeesFromDeviceAsync(device, body);

        if (employees.Count == 0)
        {
            _logger.LogWarning("No valid employee records to save from device {DeviceId}", device.Id);
            return ClockResponses.Ok;
        }

        // Step 2: Persist employees to database
        await _employeeService.CreateEmployeesAsync(device.Id, employees);

        _logger.LogInformation("Successfully saved {Count} employees from device {DeviceId}", employees.Count, device.Id);

        return ClockResponses.Ok;
    }
}