using ZKTecoADMS.Application.Commands.GetRequest;
using ZKTecoADMS.Application.Commands.IClock.CDataPost;
using ZKTecoADMS.Application.Commands.IClock.DeviceCmdCommand;
using ZKTecoADMS.Application.Queries.IClock.CDataGet;

namespace ZKTecoADMS.API.Controllers;

[ApiController]
[Route("iclock")]
public class ClockController(
    ILogger<ClockController> logger,
    IMediator bus)
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
        var query = new CDataGetQuery(SN, options, pushver, language);
        var response = await bus.Send(query);
        
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
            var response = await bus.Send(command);
            
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
        var command = new GetRequestQuery(SN);
        var response = await bus.Send(command);    
        return Content(response, "text/plain");
    }

    /// <summary>
    /// Device reports command execution result
    /// POST /iclock/devicecmd?SN=XXX&ID=123
    /// </summary>
    [HttpPost("devicecmd")]
    public async Task<IActionResult> DeviceCmd([FromQuery] string SN)
    {
        var response = await bus.Send(new DeviceCmdCommand(SN, Request.Body));
            
        return Content(response, "text/plain");
    }

}