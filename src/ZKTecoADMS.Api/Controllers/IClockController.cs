using System.Text;
using ZKTecoADMS.Application.Commands.IClock.CDataPost;
using ZKTecoADMS.Application.Commands.IClock.ClockCmdResponse;
using ZKTecoADMS.Application.Commands.SendDeviceCommand;
using ZKTecoADMS.Domain.Entities;
using ZKTecoADMS.Application.Interfaces;
using ZKTecoADMS.Application.Queries.IClock;

namespace ZKTecoADMS.API.Controllers;


[ApiController]
[Route("iclock")]
public class IClockController(
    IDeviceService deviceService,
    IAttendanceService attendanceService,
    ILogger<IClockController> logger,
    IMediator _bus)
    : ControllerBase
{
    /// <summary>
    /// Device registration and heartbeat endpoint
    /// Called by device: GET /iclock/cdata?SN=XXX&options=all&pushver=2.2.2&language=69
    /// </summary>
    [HttpGet("cdata")]
    public async Task<IActionResult> CData([FromQuery] string SN, [FromQuery] string? options, 
        [FromQuery] string? pushver, [FromQuery] string? language)
    {
        var query = new HandshakeQuery(SN, options, pushver, language);
        var response = await _bus.Send(query);
        
        return Content(response, "text/plain");
    }

    /// <summary>
    /// Receive attendance data from device
    /// POST /iclock/cdata?SN=XXX&table=ATTLOG&Stamp=9999
    /// Body: PIN\tDateTime\tVerifyType\tVerifyState\tWorkCode\n
    /// </summary>
    [HttpPost("cdata")]
    public async Task<IActionResult> PostCData([FromQuery] string SN, [FromQuery] string table, 
        [FromQuery] string Stamp)
    {
        try
        {
            logger.LogInformation("Receiving data from device {SerialNumber}, Table: {Table}, Stamp: {Stamp}", 
                SN, table, Stamp);

            var command = new CDataPostCommand(SN, table, Stamp, Request.Body);
            var response = await _bus.Send(command);
            
            return Content(response, "text/plain");

            
            // Read request body
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing data from device {SerialNumber}", SN);
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
        var command = new SendDeviceCommands(SN);
        var response = await _bus.Send(command);    
        return Content(response, "text/plain");
    }

    /// <summary>
    /// Device reports command execution result
    /// POST /iclock/devicecmd?SN=XXX&ID=123
    /// </summary>
    [HttpPost("devicecmd")]
    public async Task<IActionResult> DeviceCmd([FromQuery] string SN)
    {
        var response = await _bus.Send(new ResultCommand(SN, Request.Body));
            
        return Content(response, "text/plain");
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
                logger.LogError(ex, "Error parsing attendance line: {Line}", line);
            }
        }

        if (logs.Any())
        {
            await attendanceService.SaveAttendanceLogsAsync(logs);
            logger.LogInformation("Saved {Count} attendance logs from device {DeviceId}", 
                logs.Count, device.Id);
        }
    }

    private async Task ProcessOperationLog(Device device, string data)
    {
        // Operation logs - can be stored separately or logged
        logger.LogInformation("Operation log from device {DeviceId}: {Data}", device.Id, data);
        await Task.CompletedTask;
    }

    private async Task ProcessUserData(Device device, string data)
    {
        // User synchronization data
        logger.LogInformation("User data from device {DeviceId}: {Data}", device.Id, data);
        await Task.CompletedTask;
    }

    #endregion
}