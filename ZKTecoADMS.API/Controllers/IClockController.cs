using ZKTeco.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using ZKTecoADMS.Core.Services;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Domain.Enums;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("iclock")]
public class IClockController : ControllerBase
{
    private readonly IDeviceService _deviceService;
    private readonly IAttendanceService _attendanceService;
    private readonly ILogger<IClockController> _logger;

    public IClockController(
        IDeviceService deviceService,
        IAttendanceService attendanceService,
        ILogger<IClockController> logger)
    {
        _deviceService = deviceService;
        _attendanceService = attendanceService;
        _logger = logger;
    }

    /// <summary>
    /// Device registration and heartbeat endpoint
    /// Called by device: GET /iclock/cdata?SN=XXX&options=all&pushver=2.2.2&language=69
    /// </summary>
    [HttpGet("cdata")]
    public async Task<IActionResult> CData([FromQuery] string SN, [FromQuery] string? options, 
        [FromQuery] string? pushver, [FromQuery] string? language)
    {
        try
        {
            _logger.LogInformation($"Device handshake received from SN: {SN}, options: {options}, pushver {pushver}, language: {language}");
            var device = await _deviceService.GetDeviceBySerialNumberAsync(SN);

            if (device == null)
            {
                return Content("FAIL", "text/plain");
            }
            
            await _deviceService.UpdateDeviceHeartbeatAsync(SN);

            var response = $"GET OPTION FROM: {SN}\r\n" +
                           "ATTLOGStamp=9999\r\n" + 
                           "OPERLOGStamp=9999\r\n" + 
                           "ErrorDelay=60\r\n" +
                           "Delay=5\r\n" +
                           "TransTimes=0\r\n" +
                           "TransInterval=0\r\n" +
                           "Delay=5\r\n" +
                           "TransFlag=1111100000\r\n" +
                           "Realtime=1\r\n" +
                           "TimeZone=+7:00\r\n" +
                           "Timeout=20\r\n" +
                           "ServerVer=3.2.0\r\n" +
                           "Encrypt=0";

            return Content(response, "text/plain");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing heartbeat for device {SerialNumber}", SN);
            return StatusCode(500, "ERROR");
        }
    }

    /// <summary>
    /// Receive attendance data from device
    /// POST /iclock/cdata?SN=XXX&table=ATTLOG&Stamp=9999
    /// Body: PIN\tDateTime\tVerifyType\tVerifyState\tWorkCode\n
    /// </summary>
    [HttpPost("cdata")]
    public async Task<IActionResult> PostCData([FromQuery] string SN, [FromQuery] string? table, 
        [FromQuery] int? Stamp)
    {
        try
        {
            _logger.LogInformation("Receiving data from device {SerialNumber}, Table: {Table}, Stamp: {Stamp}", 
                SN, table, Stamp);

            // Read request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                _logger.LogWarning("Empty body received from device {SerialNumber}", SN);
                return Content("OK", "text/plain");
            }

            _logger.LogDebug("Data received: {Data}", body);

            var device = await _deviceService.GetDeviceBySerialNumberAsync(SN);
            if (device == null)
            {
                _logger.LogError("Device not found: {SerialNumber}", SN);
                return Content("ERROR: Device not registered", "text/plain");
            }

            // Process based on table type
            switch (table?.ToUpper())
            {
                case "ATTLOG":
                    await ProcessAttendanceData(device, body);
                    break;
                case "OPERLOG":
                    await ProcessOperationLog(device, body);
                    break;
                case "OPLOG":
                    await ProcessOperationLog(device, body);
                    break;
                case "USER":
                    await ProcessUserData(device, body);
                    break;
                default:
                    _logger.LogWarning("Unknown table type: {Table}", table);
                    break;
            }

            return Content("OK", "text/plain");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing data from device {SerialNumber}", SN);
            return Content("ERROR", "text/plain");
        }
    }

    /// <summary>
    /// Device queries for new data/commands
    /// GET /iclock/getrequest?SN=XXX
    /// </summary>
    [HttpGet("getrequest")]
    public async Task<IActionResult> GetRequest([FromQuery] string SN)
    {
        try
        {
            _logger.LogInformation("Get request from device: {SerialNumber}", SN);

            var device = await _deviceService.GetDeviceBySerialNumberAsync(SN);
            if (device == null)
            {
                return Content("OK", "text/plain");
            }

            var commands = await _deviceService.GetPendingCommandsAsync(device.Id);
            
            if (commands.Any())
            {
                var response = new StringBuilder();
                foreach (var command in commands.OrderByDescending(c => c.Priority))
                {
                    // Format: C:ID:CommandType:CommandData
                    var cmd = $"C:{command.CommandId}:{command.CommandType}";
                    response.AppendLine(cmd);
                    
                    // Mark as sent
                    await _deviceService.MarkCommandAsSentAsync(command.Id);
                }
                _logger.LogInformation("Command: {cmd}", response.ToString());                    

                return Content(response.ToString(), "text/plain");
            }

            return Content("OK", "text/plain");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing get request for device {SerialNumber}", SN);
            return Content("OK", "text/plain");
        }
    }

    /// <summary>
    /// Device reports command execution result
    /// POST /iclock/devicecmd?SN=XXX&ID=123
    /// </summary>
    [HttpPost("devicecmd")]
    public async Task<IActionResult> DeviceCmd([FromQuery] string SN, [FromQuery] Guid? ID)
    {
        try
        {
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var body = await reader.ReadToEndAsync();

            _logger.LogInformation("Device command response from {SerialNumber}, CommandID: {ID}, Response: {Response}", 
                SN, ID, body);

            if (ID.HasValue)
            {
                var success = body.Contains("OK", StringComparison.OrdinalIgnoreCase);
                await _deviceService.UpdateCommandStatusAsync(ID.Value, success ? CommandStatus.Success : CommandStatus.Failed, body);
            }

            return Content("OK", "text/plain");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing device command response");
            return Content("OK", "text/plain");
        }
    }

    #region Private Helper Methods

    private async Task ProcessAttendanceData(Device device, string data)
    {
        // Parse attendance data format: PIN\tDateTime\tVerifyType\tVerifyState\tWorkCode\n
        var lines = data.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var logs = new List<AttendanceLog>();

        foreach (var line in lines)
        {
            try
            {
                var parts = line.Split('\t');
                if (parts.Length < 4) continue;

                var attendanceLog = new AttendanceLog
                {
                    DeviceId = device.Id,
                    PIN = parts[0].Trim(),
                    AttendanceTime = DateTime.Parse(parts[1].Trim()),
                    VerifyType = int.TryParse(parts[2].Trim(), out var vt) ? vt : 0,
                    VerifyState = int.TryParse(parts[3].Trim(), out var vs) ? vs : 0,
                    WorkCode = parts.Length > 4 ? parts[4].Trim() : null,
                    RawData = line
                };

                logs.Add(attendanceLog);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing attendance line: {Line}", line);
            }
        }

        if (logs.Any())
        {
            await _attendanceService.SaveAttendanceLogsAsync(logs);
            _logger.LogInformation("Saved {Count} attendance logs from device {DeviceId}", 
                logs.Count, device.Id);
        }
    }

    private async Task ProcessOperationLog(Device device, string data)
    {
        // Operation logs - can be stored separately or logged
        _logger.LogInformation("Operation log from device {DeviceId}: {Data}", device.Id, data);
        await Task.CompletedTask;
    }

    private async Task ProcessUserData(Device device, string data)
    {
        // User synchronization data
        _logger.LogInformation("User data from device {DeviceId}: {Data}", device.Id, data);
        await Task.CompletedTask;
    }

    #endregion
}